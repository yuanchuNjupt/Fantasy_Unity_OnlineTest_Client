using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UIFramework.View;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UIFramework.Editor
{ 
    
    /// <summary>
    /// 用于绑定View组件的类
    /// </summary>
    public class ViewBinder
    {
        private readonly Object _context;

        public ViewBinder(Object context)
        {
            this._context = context;
        }

        public void Bind()
        {
            BaseView view = _context as BaseView;
            if (view == null)
                return;
            
            Type viewType = _context.GetType();
            var infos = viewType
                .GetFields(BindingFlags.Public | BindingFlags.Instance);

            GameObject go = view.gameObject;
            
            // 找到所有合规的组件
            Dictionary<string, GameObject> dict = new ();
            
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(go);
            while (queue.Count > 0)
            {
                GameObject current = queue.Dequeue();

                if (current.name.Contains("[") && current.name.Contains("]"))
                {
                    // 合规的组件
                    int index = current.name.IndexOf("]") + 1;
                    string name = current.name.Substring(index, current.name.Length - index);
                    
                    // 确保名称唯一性
                    if (!dict.ContainsKey(name))
                    {
                        dict.Add(name, current);
                    }
                    else
                    {
                        Debug.LogWarning($"重复的组件名称: {name} 在 {current.name}");
                    }
                }

                for (int i = 0; i < current.transform.childCount; i++)
                {
                    queue.Enqueue(current.transform.GetChild(i).gameObject);
                }
            }

            // 遍历所有字段进行绑定
            foreach (FieldInfo info in infos)
            {
                // 只处理符合条件的字段
                if (dict.TryGetValue(info.Name, out GameObject targetObj))
                {
                    try
                    {
                        // 根据字段类型进行不同的绑定处理
                        if (info.FieldType == typeof(GameObject))
                        {
                            // 直接绑定GameObject
                            info.SetValue(view, targetObj);
                        }
                        else if (info.FieldType == typeof(Transform))
                        {
                            info.SetValue(view, targetObj.transform);
                        }
                        else if (info.FieldType.IsSubclassOf(typeof(Component)))
                        {
                            // 绑定组件
                            Component component = targetObj.GetComponent(info.FieldType);
                            if (component != null)
                            {
                                info.SetValue(view, component);
                            }
                            else
                            {
                                Debug.LogError($"对象 {targetObj.name} 上找不到组件: {info.FieldType.Name}");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"字段 {info.Name} 的类型 {info.FieldType.Name} 不是GameObject或Component");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"绑定字段 {info.Name} 失败: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"未找到匹配的组件: {info.Name}");
                }
            }

            EditorUtility.SetDirty(view);
        }
    }
}