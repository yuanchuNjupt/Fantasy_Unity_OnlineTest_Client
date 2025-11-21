using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UIFramework.Editor
{
    
    

    
    /// <summary>
    /// 负责构建UIView的模板
    /// 会返回一个模板字符串
    /// </summary>
    public class UIViewTemplate
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string namespaceName = "UIFramework.Panel";
        

        
        /// <summary>
        /// 查找对象数据
        /// </summary>
        public List<ViewInfo> objViewInfoList;

        public UIViewTemplate(string namespaceName)
        {

            this.namespaceName = namespaceName;
            objViewInfoList = new List<ViewInfo>();
        }

        public void PresWindowNodeData(Transform node)
        {

            
            Queue<Transform> queue = new ();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                OnTransformHandler(current);

                for (int i = 0; i < current.childCount; i++)
                {
                    queue.Enqueue(current.GetChild(i));
                }
            }
            
            Debug.Log($"检测到{objViewInfoList.Count}个需处理组件");

            return;

            void OnTransformHandler(Transform transform)
            {
                string name = transform.name;
                if (name.Contains("[") && name.Contains("]"))
                {
                    int index = name.IndexOf("]") + 1;
                    string filedName = name.Substring(index, name.Length - index);
                    string fieldType = name.Substring(1, index - 2);

                    objViewInfoList.Add(new ViewInfo(transform.gameObject.GetInstanceID(), filedName, fieldType));
                }
            }
            
        }

        public string BuildViewTemplate(Transform root)
        {

            if (string.IsNullOrEmpty(namespaceName))
            {
                Debug.LogError("命名空间不能为空");
                return null;
            }
            
            
            PresWindowNodeData(root);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//此文件由UIViewTemplate自动生成，任何手动修改将会被下一次生成覆盖，若需手动修改请避免自动生成");
            sb.AppendLine("//Author : 原初z");
            sb.AppendLine();
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UnityEngine.UI;");
            sb.AppendLine("using UIFramework.Core;");
            sb.AppendLine("using TMPro;");

            sb.AppendLine();
            
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class " + root.name + "View " + ": BaseUIPanelView");
            sb.AppendLine("\t{");
            
            
            
            
            //核心组件区域
            
            sb.AppendLine("\t\t[Header(\"可绑定组件\")]");
            for (int i = 0; i < objViewInfoList.Count; i++)
            {
                sb.AppendLine($"\t\tpublic {objViewInfoList[i].fieldType} {objViewInfoList[i].fieldName};");
            }

            sb.AppendLine();
            
            //绑定组件区域
            
            //自动绑定组件，不需要代码Find绑定了
            
            // sb.AppendLine("\t\tpublic void BindComponents()");
            // sb.AppendLine("\t\t{");
            // for (int i = 0; i < objViewInfoList.Count; i++)
            // {
            //     sb.AppendLine(
            //         $"\t\t\t{objViewInfoList[i].fieldName} = GetComponentByName<{objViewInfoList[i].fieldType}>(\"[{objViewInfoList[i].fieldType}]{objViewInfoList[i].fieldName}\");");
            // }
            // sb.AppendLine("\t\t}");
            
            
            
            
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string BuildPresenterTemplate(Transform root)
        {
            
            
            return "";
        }
        
    }
}