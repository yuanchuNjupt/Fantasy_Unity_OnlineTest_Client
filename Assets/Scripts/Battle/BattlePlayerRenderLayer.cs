using Fantasy;
using UnityEngine;

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


        public override void Update()
        {
            UpdateState();
            base.Update();
        }

        public override void PlayAnim(string clipName)
        {
            _playerAnimator.CrossFade(clipName , 0.2f);
        }

        private void UpdateState()
        {
            if(_inputDir != Vector2.zero && _renderState != PlayerState.Run)
            {
                PlayAnim("RunStart");
                _renderState = PlayerState.Run;
            }
            else if (_inputDir == Vector2.zero && _renderState != PlayerState.Idle)
            {
                PlayAnim("Idle");
                _renderState = PlayerState.Idle;
            }
        }

        
        /// <summary>
        /// 根据网络传输过来的输入方向更新渲染层的状态
        /// </summary>
        public void UpdateInputDir(CSFixIntVector3 csInputDir)
        {
            
            _inputDir = new Vector2(csInputDir.x, csInputDir.z);
        }
    }
}