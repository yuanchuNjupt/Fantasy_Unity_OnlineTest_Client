using System;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Pool;

namespace Battle.FrameCommand
{
    public sealed class OneFrameCommandCache : IPool , IDisposable
    {
        
        public long FrameID { get; private set; }
        
        public FrameCommandType FrameType { get; private set; }
        
        private readonly List<FrameOperationData> _frameOperationDataList = new List<FrameOperationData>();
        
        public IEnumerable<FrameOperationData> FrameOperationDataList => _frameOperationDataList;
        

        public static OneFrameCommandCache Create(long frameId, List<FrameOperationData> frameOperationDataList , FrameCommandType frameType)
        {
            var oneFrameCommandCache = Pool<OneFrameCommandCache>.Rent();
            oneFrameCommandCache.FrameID = frameId;
            oneFrameCommandCache.FrameType = frameType;
            
            
            if(frameOperationDataList == null || frameOperationDataList.Count == 0) return oneFrameCommandCache;
            
            
            oneFrameCommandCache._frameOperationDataList.AddRange(frameOperationDataList);
            
            oneFrameCommandCache.Sort();
            
            return oneFrameCommandCache;

        }

        #region 扩展方法

        public static bool IsSameFrameOperationData(FrameOperationData a, FrameOperationData b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;

            bool inputDirSame = ReferenceEquals(a.inputDir, b.inputDir) ||
                                (a.inputDir != null && b.inputDir != null &&
                                 a.inputDir.x == b.inputDir.x &&
                                 a.inputDir.y == b.inputDir.y &&
                                 a.inputDir.z == b.inputDir.z);

            return inputDirSame &&
                   a.playerId == b.playerId &&
                   a.skillId == b.skillId &&
                   a.operateType == b.operateType;
        }
        
        public static FrameOperationData Clone(FrameOperationData source)
        {
            if (source == null)
            {
                Log.Warning("尝试克隆一个空的FrameOperationData");
                return null;
            }
            
            
            var clone = new FrameOperationData
            {
                playerId = source.playerId,
                skillId = source.skillId,
                operateType = source.operateType
            };
            if(source.inputDir != null)
            {
                clone.inputDir = new CSFixIntVector3()
                {
                    x = source.inputDir.x,
                    y = source.inputDir.y,
                    z = source.inputDir.z,
                };
            }

            return clone;
        }

        public static bool IsSameFrameCommand(OneFrameCommandCache a, OneFrameCommandCache b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;
            if (a.FrameID != b.FrameID || a._frameOperationDataList.Count != b._frameOperationDataList.Count) return false;

            for (int i = 0; i < a._frameOperationDataList.Count; i++)
            {
                if (!IsSameFrameOperationData(a._frameOperationDataList[i], b._frameOperationDataList[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void Add(FrameOperationData frameOperationData)
        {
            if (frameOperationData == null)
            {
                return;
            }
            _frameOperationDataList.Add(frameOperationData);
        }

        /// <summary>
        /// 根据操作权重进行排序
        /// </summary>
        public void Sort()
        {
            _frameOperationDataList.Sort((a, b) =>
            {
                if (ReferenceEquals(a, b)) return 0;
                if (a == null) return 1;
                if (b == null) return -1;

                int operateTypeCompare = b.operateType.CompareTo(a.operateType);
                if (operateTypeCompare != 0)
                {
                    return operateTypeCompare;
                }

                return a.playerId.CompareTo(b.playerId);
            });
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