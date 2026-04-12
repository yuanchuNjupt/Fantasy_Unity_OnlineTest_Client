using System;
using Config;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using Framework.AdvancedLog;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;

namespace Battle
{
    public class BattlePlayerRenderLayer : RenderObject
    {
        private Vector2 _inputDir;

        private Animator _playerAnimator;

        // private PlayerState _renderState;

        private BattlePlayerInstance _instance;


        public void Init(BattlePlayerInstance instance)
        {
            _instance = instance;
            PlayAnim("Idle");
            // 为本地玩家启用客户端预测功能，改善弱网体验
            
            // bool isLocalPlayer = instance.playerType == PlayerType.Self;
            
            
            SetLogicObject(instance.logicLayer, true, isLocalPlayer);
            instance.logicLayer.onActionStateChange += SwitchState;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _playerAnimator = GetComponent<Animator>();
        }

        public override void OnRelease()
        {
            base.OnRelease();
            if (_instance != null && _instance.logicLayer != null)
            {
                _instance.logicLayer.onActionStateChange -= SwitchState;
            }
        }


        public override void PlayAnim(string clipName)
        {
            _playerAnimator.CrossFade(clipName, 0.2f);
        }


        private void SwitchState(LogicObjectActionState newState)
        {
            switch (newState)
            {
                case LogicObjectActionState.Idle:
                    PlayAnim(AnimationClipConfig.IDLE);
                    break;
                case LogicObjectActionState.Move:
                    PlayAnim(AnimationClipConfig.RUN_START);
                    break;
                // default:
                //     return;
            }

            Log.Info(LogColor.Cyan, "角色状态切换",
                $"角色UID:{_instance.uid},角色名称:{_instance.playerName},当前状态{newState.ToString()}");
        }

        public override void OnDeath()
        {
            isUpdatePosAndRotation = false;
            _playerAnimator.applyRootMotion = true;
            PlayAnim(AnimationClipConfig.DEATH);
        }
    }
}