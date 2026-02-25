using Config;
using Fantasy;
using Framework.AdvancedLog;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;

namespace Battle
{
    public class BattlePlayerRenderLayer : RenderObject
    {
        private Vector2 _inputDir;

        private Animator _playerAnimator;

        private PlayerState _renderState;

        private BattlePlayerInstance _instance;


        public void Init(BattlePlayerInstance instance)
        {
            _instance = instance;
            PlayAnim("Idle");
            _renderState = PlayerState.Idle;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _playerAnimator = GetComponent<Animator>();
        }

        public override void PlayAnim(string clipName)
        {
            _playerAnimator.CrossFade(clipName, 0.2f);
        }


        private bool SwitchState(PlayerState newState)
        {
            if (_renderState == newState)
            {
                Log.Error("当前状态已经是" + newState + "，无需切换!");
                return false;
            }

            _renderState = newState;

            switch (_renderState)
            {
                case PlayerState.Idle:
                    PlayAnim(AnimationClipConfig.IDLE);
                    break;
                case PlayerState.Run:
                    PlayAnim(AnimationClipConfig.RUN_START);
                    break;
            }

            Log.Info(LogColor.Cyan, "角色状态切换", 
                    $"角色UID:{_instance.uid}", 
                 $"角色名称:{_instance.playerName}",
                             $"当前状态{_renderState.ToString()}");
            return true;
        }


        /// <summary>
        /// 根据网络传输过来的输入方向更新渲染层的状态
        /// </summary>
        public void UpdateInputDir(CSFixIntVector3 csInputDir)
        {
            Vector2 newInputDir = new Vector2(csInputDir.x, csInputDir.z);
            
            if(_inputDir == Vector2.zero && newInputDir != Vector2.zero)
            {
                //从静止切换到移动
                SwitchState(PlayerState.Run);
            }
            else if(_inputDir != Vector2.zero && newInputDir == Vector2.zero)
            {
                //从移动切换到静止
                SwitchState(PlayerState.Idle);
            }
            
            //更新输入方向
            _inputDir = newInputDir;
        }
    }
}