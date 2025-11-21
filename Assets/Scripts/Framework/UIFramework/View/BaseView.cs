using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UIFramework.View
{
    public class BaseView : MonoBehaviour , IView
    {
        [ShowInInspector]
        //全局唯一标识符，用于定位View
        public string Guid { get; set; }

        
        /// <summary>
        /// 查找组件
        /// </summary>
        /// <param name="name">组件名称</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        protected T GetComponentByName<T>(string name) where T : Component
        {
            Transform target = GetTransformByName(name);
            if (target == null)
            {
                return null;
            }
            T component = target.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError("没有找到组件：" + name);
            }
            return component;
        }



        /// <summary>
        /// 广度优先查找
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        protected Transform GetTransformByName(string childName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(transform);

            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                //检测取出的Transform名称是否匹配，若匹配则返回
                if (current.name == childName)
                {
                    return current;
                }
                
                //将该对象中的所有子对象填充至队列
                for (int i = 0; i < current.childCount; i++)
                {
                    queue.Enqueue(current.GetChild(i));
                }
                
            }
            
            Debug.LogError("没有找到子对象：" + childName);
            return null;
        }

        /// <summary>
        /// 获取或创建组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetOrCreateComponent<T>() where T : Component
        {
            if (!TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 获取指定对象中所有子对象的View组件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public BaseView[] GetAllViews(Transform target , bool includeInactive = false)
        {
            return target.GetComponentsInChildren<BaseView>(includeInactive);
        }
        
        /// <summary>
        /// 获取指定对象中所有子对象的指定类型的View组件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="includeInactive"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetViews<T>(Transform target, bool includeInactive = false) where T : BaseView
        {
            return target.GetComponentsInChildren<T>(includeInactive);
        }

        public T GetView<T>(string guid, bool includeInactive = false) where T : BaseView
        {
            IEnumerable<T> views = transform.GetComponentsInChildren<T>(includeInactive);
            foreach (T view in views)
            {
                if (view.Guid == guid)
                {
                    return view;
                }
            }
            
            Debug.LogError($"没有找到Guid为 \"{guid}\" 的View：");
            return null;
        }
        
        /// <summary>
        /// 获取指定对象中指定Guid的View组件
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="guid">对象Guid</param>
        /// <param name="includeInactive">是否包含未激活的对象</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>View组件</returns>
        public T GetView<T>(string guid, Transform target, bool includeInactive) where T : BaseView
        {
            IEnumerable<T> views = target.transform.GetComponentsInChildren<T>(includeInactive);
            foreach (T view in views)
            {
                if (view.Guid == guid)
                {
                    return view;
                }
            }

            Debug.LogError($"没有找到Guid为 \"{guid}\" 的View：");
            return null;
        }
        
        /// <summary>
        /// 尝试获取自身对象中指定Guid的View组件
        /// </summary>
        /// <param name="guid">对象Guid</param>
        /// <param name="view">View组件</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>是否获取成功</returns>
        public bool TryGetView<T>(string guid, out T view) where T : BaseView
        {
            view = GetView<T>(guid);
            return view != null;
        }
        
        /// <summary>
        /// 尝试获取指定对象中指定Guid的View组件
        /// </summary>
        /// <param name="guid">对象Guid</param>
        /// <param name="target">目标对象</param>
        /// <param name="view">View组件</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>是否获取成功</returns>
        public bool TryGetView<T>(string guid, Transform target, out T view) where T : BaseView
        {
            view = GetView<T>(guid, target);
            return view != null;
        }
        
        
    }
}