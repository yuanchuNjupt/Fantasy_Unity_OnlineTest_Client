using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.GameManager.Core
{
    public partial class World
    {
    
        //储存各种层级类型的字典
        private static Dictionary<string, (ILogicBehaviour logicBehaviour , Type SourceWorld)> _logicBehaviours = new();
        private static Dictionary<string, (IDataBehaviour dataBehaviour , Type SourceWorld)> _dataBehaviours = new();
        private static Dictionary<string, (IMessageBehaviour messageBehaviour , Type SourceWorld)> _messageBehaviours = new();
    
        //生命周期
        public virtual void OnCreate()
        {
        
        }

        /// <summary>
        /// 世界销毁时触发
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        public virtual void OnUpdate()
        {
        }
    
        protected virtual void OnDestroyComplete()
        {
        
        }

        /// <summary>
        /// 销毁游戏世界
        /// </summary>
        public void DestroyWorld()
        {
            var removeKeys = _logicBehaviours.Where(x => x.Value.SourceWorld == GetType()).Select(x => x.Key).ToList();
            
        
            foreach (var key in removeKeys)
            {
                _logicBehaviours[key].logicBehaviour.OnDestroy();
                _logicBehaviours.Remove(key);
            }
        
            removeKeys = _dataBehaviours.Where(x => x.Value.SourceWorld == GetType()).Select(x => x.Key).ToList();
 
        
            foreach (var key in removeKeys)
            {
                _dataBehaviours[key].dataBehaviour.OnDestroy();
                _dataBehaviours.Remove(key);
            } 
        
            removeKeys = _messageBehaviours.Where(x => x.Value.SourceWorld == GetType()).Select(x => x.Key).ToList();
        
            foreach (var key in removeKeys)
            {
                _messageBehaviours[key].messageBehaviour.OnDestroy();
                _messageBehaviours.Remove(key);
            }
        
        
            OnDestroy();
            OnDestroyComplete();
        }
    
        public static T GetExitsLogicManager<T>() where T : class , ILogicBehaviour
        {
        
            if(_logicBehaviours.TryGetValue(typeof(T).Name, out var res))
            {
                return res.logicBehaviour as T;
            }
        
            Debug.LogError("不存在Logic Manager:" + typeof(T).Name);
            return null;
        }
    
        public static T GetExitsDataManager<T>() where T : class , IDataBehaviour
        {
        
            if(_dataBehaviours.TryGetValue(typeof(T).Name, out var res))
            {
                return res.dataBehaviour as T;
            }
        
            Debug.LogError("不存在Data Manager:" + typeof(T).Name);
            return null;
        }
    
        public static T GetExitsMessageManager<T>() where T : class , IMessageBehaviour
        {
        
            if(_messageBehaviours.TryGetValue(typeof(T).Name, out var res))
            {
                return res.messageBehaviour as T;
            }
        
            Debug.LogError("不存在Message Manager:" + typeof(T).Name);
            return null;
        }
        
        /// <summary>
        /// 根据类型获取Manager实例（非泛型版本，用于依赖注入）
        /// </summary>
        /// <param name="managerType">Manager的类型</param>
        /// <returns>Manager实例，如果未找到返回null</returns>
        public static object GetManagerByType(Type managerType)
        {
            if (managerType == null)
            {
                return null;
            }
        
            string typeName = managerType.Name;
        
            // 尝试从LogicBehaviour中查找
            if (typeof(ILogicBehaviour).IsAssignableFrom(managerType))
            {
                if (_logicBehaviours.TryGetValue(typeName, out var logicRes))
                {
                    return logicRes.logicBehaviour;
                }
            }
        
            // 尝试从DataBehaviour中查找
            if (typeof(IDataBehaviour).IsAssignableFrom(managerType))
            {
                if (_dataBehaviours.TryGetValue(typeName, out var dataRes))
                {
                    return dataRes.dataBehaviour;
                }
            }
        
            // 尝试从MessageBehaviour中查找
            if (typeof(IMessageBehaviour).IsAssignableFrom(managerType))
            {
                if (_messageBehaviours.TryGetValue(typeName, out var messageRes))
                {
                    return messageRes.messageBehaviour;
                }
            }
        
            return null;
        }
    
    
    }
}
