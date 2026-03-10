using Battle;
using FixedPhysics.FixedCollider.Core;
using Framework.AdvancedLog;
using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

namespace Framework.GameManagerFramework.WorldScripts
{
    public class BattleWorld : GameManager.Core.World
    {
        //需要模拟逻辑帧更新
        private float _accLogicRunTime;
        private float _nextLogicFrameTime;
        public float LogicDeltaTime;
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if(!LogicFrameConfig.IsUseLocalLogicFrame)
                return;
            
            //逻辑帧的累计时间
            _accLogicRunTime += Time.deltaTime;
            
            //保证所有设备的逻辑帧率一致，并进行追帧操作
            while (_accLogicRunTime > _nextLogicFrameTime)
            {
                //更新逻辑帧
                OnLogicFrameUpdate();
                PhysicsManager3D.Instance.OnLogicFrameUpdate();
                
                
                //计算下一个逻辑帧运行的时间
                _nextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                //逻辑帧ID进行自增
                LogicFrameConfig.LogicFrameId++;
            }
            
            //逻辑帧 1秒15帧 渲染帧 1秒60帧
            
            

            LogicDeltaTime = (_accLogicRunTime + LogicFrameConfig.LogicFrameInterval - _nextLogicFrameTime) / LogicFrameConfig.LogicFrameInterval;
            
            
        }

        private void OnLogicFrameUpdate()
        {
            GetExitsLogicManager<BattlePlayerLogicManager>().OnLogicFrameUpdate();
        }
    }
}