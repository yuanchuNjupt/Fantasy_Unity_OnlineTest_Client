using System;
using System.Collections.Generic;
using System.Linq;
using Battle.FrameCommand;
using Fantasy;
using Framework.GameManager.Base;
using Framework.GameManager.Core;
using Framework.GameManager.DataManagers;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;

namespace Framework.GameManagerFramework.LogicManagers.FrameCommand
{
    [WorldSource(typeof(BattleWorld))]
    public class FrameCommandLogicManager : ILogicBehaviour
    {
        
        [Inject] private FrameCommandDataManager _frameCommandDataManager;
        [Inject] private BattleDataManager _battleDataManager;
        [Inject] private BattleMessageManager _battleMessageManager;
        
        [Inject] private BattlePlayerLogicManager _battlePlayerLogicManager;
        
        private BattleLogicManager _battleLogicManager;


        

        #region 固定预测窗口 + 时间缩放

        private float _timeAccumulator; //时间累积器;
        private long _lastServerTick; //上次服务器帧的时间戳
        private Queue<long> _delays;
        private int _averageDelay; //平均网络延迟
        
        public int AverageDelay => _averageDelay;


        public void OnCreate()
        {
            _timeAccumulator = 0;
            _lastServerTick = 0;
            _delays = new Queue<long>();
        }

        public void OnDestroy()
        {
        }

        public void Init()
        {
            _battleLogicManager = World.GetExitsLogicManager<BattleLogicManager>();
        }


        public int FrameUpdate(float deltaTime)
        {
            _timeAccumulator += deltaTime * 1000;

            //计算需要执行的帧数
            int frameCount = (int)(_timeAccumulator / LogicFrameConfig.LogicFrameIntervalMs);
            _timeAccumulator -= frameCount * LogicFrameConfig.LogicFrameIntervalMs; //扣除已经执行的帧时间
            return frameCount;
        }

        public void OnInputFrameOperateData(long serverTick)
        {
            //校准客户端的帧数与服务器的帧数差距
            long frameDiff = LogicFrameConfig.LocalPredictedLogicFrameId - LogicFrameConfig.ServerLogicFrameId;
            if (frameDiff > 1 || frameDiff < -1)
            {
                //渐进式调整客户端的帧数，使其逐渐接近服务器的帧数，避免突然跳帧导致的游戏体验问题
                
                //1.客户端太慢了：frameDiff < -1，说明客户端的帧数落后于服务器，需要加快帧数
                //2.客户端太快了：frameDiff > 1，说明客户端的帧数领先于服务器，需要减慢帧数
                
                _timeAccumulator -= Math.Sign(frameDiff) *
                                    Math.Min(Math.Abs(frameDiff) * LogicFrameConfig.LogicFrameIntervalMs,
                                        LogicFrameConfig.LogicFrameIntervalMs);
            }
            if (_lastServerTick == 0)
            {
                _lastServerTick = serverTick;
                return;
            }
            
            long tickDiff = serverTick - _lastServerTick;

            if (tickDiff > 0)
            {
                //66ms + 网络延迟的时间差
                long networkDelay = tickDiff - LogicFrameConfig.LogicFrameIntervalMs;
                _delays.Enqueue(networkDelay);
                if (_delays.Count > LogicFrameConfig.DelayBufferSize)
                    _delays.Dequeue();

                //计算平均网络延迟
                _averageDelay = (int)_delays.Average();
            }

            _lastServerTick = serverTick;
        }

        #endregion


        #region 预测指令

        public void ExecutePredictionFrameCommand()
        {
            //预测当前客户端帧 + 预测窗口帧
            //得到当前的采集数据
            var operationDataList = _frameCommandDataManager.currentFrameOperationData;
            
            //发送给服务器，进行预测
            long currentFrameId = LogicFrameConfig.LocalPredictedLogicFrameId + LogicFrameConfig.PredictionWindowSize;
            SendOneFrameCommandToServer(currentFrameId, operationDataList);

            if (_frameCommandDataManager.TryGetCommand(currentFrameId, out var commandCache))
            {
                if (commandCache.FrameType is FrameCommandType.Authoritative)
                {
                    //已经有权威帧了，不需要预测了
                    return;
                }
            }
            
            
            
            //预测本地玩家的操作
            OneFrameCommandCache command = OneFrameCommandCache.Create(currentFrameId, operationDataList , FrameCommandType.Prediction);
            
            
            //预测其他玩家的操作
            //采取的方法为： 获取这个玩家的上一次权威帧操作进行预测。
            if (_frameCommandDataManager.TryGetCommand(LogicFrameConfig.ServerLogicFrameId, out var cache))
            {
                foreach (FrameOperationData frameOperationData in cache.FrameOperationDataList)
                {
                    if (frameOperationData.playerId == _battleDataManager.CurrentPlayerIdInBattle) continue;
                    
                    command.Add(OneFrameCommandCache.Clone(frameOperationData));
                }
            }
            
            //排序
            command.Sort();
            
            
            //添加到缓存中
            _frameCommandDataManager.AddCommand(command);

            foreach (var data in command.FrameOperationDataList)
            {
                var playerLogic = _battlePlayerLogicManager.GetBattlePlayerInstance(data.playerId).logicLayer;
                switch ((OperateTypeEnum)data.operateType)
                {
                    case OperateTypeEnum.InputMove:
                        playerLogic.ApplyMoveOperation(data.inputDir);
                        break;
                    
                    //暂时先不考虑技能释放的预测了，技能释放的预测需要考虑更多的因素，比如技能的施法时间、技能的目标选择等，目前先只预测移动操作
                    
                }
            }
            
            //执行一次逻辑帧更新
            _battlePlayerLogicManager.OnLogicFrameUpdate();
            
            
            operationDataList.Clear();
            //采集快照
            _frameCommandDataManager.CaptureSnapshot(currentFrameId , _battlePlayerLogicManager);
            
            
            
        }
        
        private void SendOneFrameCommandToServer(long frameId , List<FrameOperationData> operationDataList)
        {
            long battleId = _battleDataManager.BattleId;
            _battleMessageManager.SendFrameOperateEventMessage(battleId, frameId, operationDataList);
        }
        
        

        #endregion
        
    }
        
}