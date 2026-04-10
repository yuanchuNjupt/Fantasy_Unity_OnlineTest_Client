using System;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Pool;

namespace Battle.FrameOperate
{
    public sealed class OneFrameCommandCache : IPool , IDisposable
    {
        
        public long FrameID { get; private set; }
        
        public FrameCommandType FrameType { get; private set; }
        
        private List<FrameOperationData> _frameOperationDataList = new List<FrameOperationData>();






        public static OneFrameCommandCache Create(long frameId, List<FrameOperationData> frameOperationDataList , FrameCommandType frameType)
        {
            var oneFrameCommandCache = Pool<OneFrameCommandCache>.Rent();
            oneFrameCommandCache.FrameID = frameId;
            oneFrameCommandCache.FrameType = frameType;
            oneFrameCommandCache._frameOperationDataList = frameOperationDataList;
            
            oneFrameCommandCache._frameOperationDataList.AddRange(frameOperationDataList);
            
            //TODO:根据权重排序
            
            return oneFrameCommandCache;

        }

        #region 扩展方法

        public static bool IsSameFrameOperationData(FrameOperationData a, FrameOperationData b)
        {
            return a.inputDir.x == b.inputDir.x &&
                   a.inputDir.y == b.inputDir.y &&
                   a.inputDir.z == b.inputDir.z &&
                   a.playerId == b.playerId &&
                   a.skillId == b.skillId &&
                   a.operateType == b.operateType;
        }
        
        public static FrameOperationData CloneFrameOperationData(FrameOperationData source)
        {
            return new FrameOperationData
            {
                inputDir = new CSFixIntVector3
                {
                    x = source.inputDir.x,
                    y = source.inputDir.y,
                    z = source.inputDir.z
                },
                playerId = source.playerId,
                skillId = source.skillId,
                operateType = source.operateType
            };
        }

        public static bool IsSameFrameCommand(OneFrameCommandCache a, OneFrameCommandCache b)
        {
            if(a.FrameID != b.FrameID || a._frameOperationDataList.Count != b._frameOperationDataList.Count) return false;
            for (int i = 0; i < a._frameOperationDataList.Count; i++)
                if(!IsSameFrameOperationData(a._frameOperationDataList[i] , b._frameOperationDataList[i])) return false;
            return true;
        }
        
        

        #endregion
        
        

        private bool _isPool;
        
        public bool IsPool()
        {
            return _isPool;
        }

        public void SetIsPool(bool isPool)
        {
            _isPool = isPool;
        }

        public void Dispose()
        {
            FrameID = 0;
            _frameOperationDataList.Clear();
            Pool<OneFrameCommandCache>.Return(this);
        }
    }
}