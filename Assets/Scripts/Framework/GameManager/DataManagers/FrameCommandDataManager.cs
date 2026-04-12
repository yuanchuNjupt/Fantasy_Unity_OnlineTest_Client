using System.Collections.Generic;
using System.Linq;
using Battle.FrameCommand;
using Fantasy;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using Log = Framework.AdvancedLog.Log;

namespace Framework.GameManager.DataManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class FrameCommandDataManager : IDataBehaviour
    {
        private SortedDictionary<long, OneFrameCommandCache> _caches;
        
        public OneFrameCommandCache this[long frameId]
        {
            get
            {
                if (_caches.TryGetValue(frameId, out var cache))
                {
                    return cache;
                }
                return null;
            }
        }
        
        //当前采集到的帧操作数据
        public List<FrameOperationData> currentFrameOperationData = new List<FrameOperationData>();
        
        
        
        
        
        public void OnCreate()
        {
            _caches = new SortedDictionary<long, OneFrameCommandCache>();
            
        }
        
        
        public bool AddCommand(long frameId, FrameCommandType frameType, List<FrameOperationData> frameOperationDataList)
        {
            var cache = OneFrameCommandCache.Create(frameId ,frameOperationDataList , frameType);
            return AddCommand(cache);
        }
        
        

        /// <summary>
        /// 添加帧操作数据
        /// </summary>
        /// <param name="cache">添加的帧操作数据</param>
        /// <returns>是否需要进行回滚判断</returns>
        public bool AddCommand(OneFrameCommandCache cache)
        {
            var frameId = cache.FrameID;
            var frameType = cache.FrameType;
            if (_caches.TryGetValue(frameId, out var commandCache))
            {
                //如果存在，就是权威帧，直接覆盖
                
                //二次校验
                if(frameType < commandCache.FrameType)
                {
                    Log.Error("[FrameCommandDataManager] AddCommand failed, the new coming command is outdated, frameId: " + frameId + 
                              ", new coming frameType: " + 
                              frameType + ", cached frameType: " + 
                              commandCache.FrameType);
                    return false;
                }

                if (OneFrameCommandCache.IsSameFrameCommand(commandCache, cache))
                {
                    //如果完全一样，就不需要覆盖了，也不需要进行回滚判断了
                    return false;
                }
                
                
                //进入这里说明是同一帧的命令数据，但不完全一样，可能是补帧数据或者追帧数据，需要覆盖原来的数据，并且需要进行回滚判断
                
                _caches[frameId] = cache;
                return true;
            }

            if (_caches.Count >= LogicFrameConfig.MaxCachedLogicFrameCount)
            {
                if (_caches.Remove(_caches.First().Key, out var removedCache))
                {
                    removedCache.Dispose();
                }
            }
            
            _caches.Add(frameId, cache);
            return frameType is FrameCommandType.Authoritative;
        }


        public bool TryGetCommand(long frameId, out OneFrameCommandCache cache)
        {
            return _caches.TryGetValue(frameId, out cache);
        }

        public void Clear()
        {
            foreach (var cache in _caches.Values)
            {
                cache.Dispose();
            }
            _caches.Clear();
        }


        
        

        public void OnDestroy()
        {
            Clear();
        }
    }
}