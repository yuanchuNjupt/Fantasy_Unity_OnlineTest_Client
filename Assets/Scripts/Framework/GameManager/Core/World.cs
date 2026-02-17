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
        
            Debug.LogError("No Exits Logic Manager:" + typeof(T).Name);
            return null;
        }
    
        public static T GetExitsDataManager<T>() where T : class , IDataBehaviour
        {
        
            if(_dataBehaviours.TryGetValue(typeof(T).Name, out var res))
            {
                return res.dataBehaviour as T;
            }
        
            Debug.LogError("No Exits Data Manager:" + typeof(T).Name);
            return null;
        }
    
        public static T GetExitsMessageManager<T>() where T : class , IMessageBehaviour
        {
        
            if(_messageBehaviours.TryGetValue(typeof(T).Name, out var res))
            {
                return res.messageBehaviour as T;
            }
        
            Debug.LogError("No Exits Message Manager:" + typeof(T).Name);
            return null;
        }
    
    
    }
}
