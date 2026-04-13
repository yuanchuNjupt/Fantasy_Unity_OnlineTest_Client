using System.Collections.Generic;
using System.Linq;
using Battle.CustomCollider;
using Battle.FrameCommand;
using Fantasy;
using Framework.AdvancedLog;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using Log = Framework.AdvancedLog.Log;

namespace Framework.GameManager.DataManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class FrameCommandDataManager : IDataBehaviour
    {
        
        /// <summary>
        /// 帧指令缓存，Key: 帧ID，Value: 帧指令数据
        /// </summary>
        private SortedDictionary<long, OneFrameCommandCache> _caches;
        
        
        /// <summary>
        /// 世界快照缓存，Key: 帧ID，Value: 世界快照数据
        /// </summary>
        private SortedDictionary<long , WorldSnapshot> _snapshots;
        
        public long MaxSnapshotId { get;private set; }
        
        public long MinSnapshotId { get;private set; }
        
        
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
        
        
        public List<FrameOperationData> CloneCurrentFrameOperationData()
        {
            var cloneList = new List<FrameOperationData>(currentFrameOperationData.Count);
            foreach (var data in currentFrameOperationData)
            {
                cloneList.Add(OneFrameCommandCache.Clone(data));
            }
            return cloneList;
        }
        
        
        public void OnCreate()
        {
            _caches = new SortedDictionary<long, OneFrameCommandCache>();
            _snapshots = new SortedDictionary<long, WorldSnapshot>();

            MaxSnapshotId = 0;
            MinSnapshotId = 0;
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

        
        
        /// <summary>
        /// 采集当前帧的世界快照数据，供回滚使用
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="logicManager"></param>
        public void CaptureSnapshot(long frameId, BattlePlayerLogicManager logicManager)
        {
            if(_snapshots.ContainsKey(frameId))
            {
                Log.Warning("[状态快照]" , "已经存在帧ID为 " + frameId + " 的快照，无需重复采集");
                return;
            }

            if (_snapshots.Count >= LogicFrameConfig.MaxWorldSnapshotBufferSize)
            {
                //如果快照缓存已满，移除最旧的快照
                //从1帧开始，采集到81帧 ， 淘汰1帧
                if (!_snapshots.Remove(frameId - LogicFrameConfig.MaxWorldSnapshotBufferSize))
                {
                    Log.Warning( "[状态快照]" , "尝试移除帧ID为 " + (frameId - LogicFrameConfig.MaxWorldSnapshotBufferSize) + " 的快照，但未找到该快照");
                    Log.Warning("[状态快照]" , "转换为移除缓存中第一个快照");
                    if (!_snapshots.Remove(_snapshots.First().Key))
                    {
                        Log.Error("[状态快照]" , "尝试移除缓存中第一个快照，但未找到任何快照，可能出现了严重错误");
                    }
                }
            }
            
            
            
            var worldSnapshot = WorldSnapshot.Create();
            worldSnapshot.Capture(logicManager);
            _snapshots[frameId] = worldSnapshot;
            MaxSnapshotId = frameId;
            MinSnapshotId = _snapshots.First().Key;

            if (MinSnapshotId + LogicFrameConfig.MaxWorldSnapshotBufferSize != MaxSnapshotId)
            {
                Log.Warning("[状态快照]" , "当前快照缓存中最小帧ID为 " + MinSnapshotId + "，最大帧ID为 " + MaxSnapshotId + "，两者之差不等于配置的快照缓存大小 " + LogicFrameConfig.MaxWorldSnapshotBufferSize);
            }
        }


        public bool TryGetSnapshot(long rollbackFrameId, out WorldSnapshot snapshot)
        {
           return _snapshots.TryGetValue(rollbackFrameId, out snapshot);
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