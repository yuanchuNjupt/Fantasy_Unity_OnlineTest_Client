
using Config;
using Fantasy;
using Framework.GameManagerFramework.LogicManagers;
using Helper;
using UnityEngine;


public enum PlayerType
{
    Self,
    Other,
}

public enum PlayerState
{
    Idle = 1,
    Run = 2,
    Sprint = 3,
}


public partial class LobbyPlayer : MonoBehaviour
{
    
    public PlayerType playerType;
    
    public PlayerState state;
    
    
    public string PlayerName;
    
    private Animator _animator;
    
    private Vector2 _inputDir;
    
    public float smoothPosSpeed = 5f;
    
    
    public float smoothRotSpeed = 7.5f;
    
    private Vector3 _renderDir = new Vector3(0 , 0 , 1);
    
    private Transform _playerCameraTransform;
    private Vector2 _cameraForward = new Vector2(0 , 0);
    
    

    #region 状态同步数据
    
    public Vector3 syncTargetPos;

    public Vector3 syncTargetDir;
    
    public PlayerState syncTargetState;
    
    /// <summary>
    /// 上一次的状态，用于判断停止动画
    /// </summary>
    private PlayerState _lastState;
    
    /// <summary>
    /// 当前状态同步计数
    /// </summary>
    private int _syncStateCurrentCount;
    
    private Vector2 _lastInput;

    

    #endregion
    
    public void Init(string playerName ,PlayerType type)
    {
        playerType = type;
        _animator = GetComponent<Animator>();
        
        PlayerName = playerName;
        _playerCameraTransform = World.GetExitsLogicManager<TP_CameraLogicManager>().cameraControl.transform;

        state = PlayerState.Idle;
        _lastState = PlayerState.Idle;
        syncTargetState = PlayerState.Idle;
        PlayAnimation("Idle");
        OnLobbyPlayerInputInit();
    }

    private void PlayAnimation(string animName)
    {
        _animator.CrossFade(animName , 0.2f);
    }

    public void SyncPos(CSVector3 position, CSVector3 inputDir , PlayerState playerState)
    {
        syncTargetPos = position.ToVector3();
        syncTargetDir = inputDir.ToVector3();
        syncTargetState = playerState;
    }
    
    //初始化生成的位置
    public void InitPos(CSVector3 position, CSVector3 renderDir)
    {
        transform.position = position.ToVector3();
        Quaternion targetRotation = Quaternion.LookRotation(renderDir.ToVector3());
        transform.rotation = targetRotation;
        
        syncTargetPos = position.ToVector3();
        _renderDir = renderDir.ToVector3();
    }

    private void UpdatePos()
    {
        transform.position = Vector3.Lerp(transform.position , syncTargetPos , Time.deltaTime * smoothPosSpeed);
    }

    private void UpdateDir()
    {
        // 如果有移动方向
        if (syncTargetDir != Vector3.zero)
        {
            _renderDir = syncTargetDir;
        }
        
        // 计算目标旋转角度（朝向移动方向）
        Quaternion targetRotation = Quaternion.LookRotation(_renderDir);
        
        // 平滑插值到目标旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotSpeed);
    }

    public void UpdateState()
    {
        // 只在 syncTargetState 发生变化时才处理
        if (syncTargetState == _lastState)
        {
            return;
        }
        
        // 判断是否从移动状态变为 Idle
        if (syncTargetState == PlayerState.Idle && _lastState != PlayerState.Idle)
        {
            // 根据上一次的状态播放对应的停止动画
            if (_lastState == PlayerState.Run)
            {
                PlayAnimation("RunStop");
            }
            else if (_lastState == PlayerState.Sprint)
            {
                PlayAnimation("SprintStop");
            }
        }
        else
        {
            // 其他状态变化，直接播放对应动画
            switch (syncTargetState)
            {
                case PlayerState.Idle:
                    PlayAnimation("Idle");
                    break;
                case PlayerState.Run:
                    PlayAnimation("Run");
                    break;
                case PlayerState.Sprint:
                    PlayAnimation("Sprint");
                    break;
            }
        }
        
        // 更新上一次的状态为当前的 syncTargetState
        _lastState = syncTargetState;
    }


    private void Update()
    {
        OnLobbyPlayerInputUpdate();
        
        UpdatePos();
        UpdateDir();
        UpdateState();
    }
    


    private void FixedUpdate()
    {
        _syncStateCurrentCount++;
        
        //每隔100ms同步一次位置和方向
        if (_syncStateCurrentCount == StateSyncConfig.MaxSyncStateCount)
        {
    
            _syncStateCurrentCount = 0;
            

            if (Vector2.Equals(_lastInput, Vector2.zero) && Vector2.Equals(_inputDir, Vector2.zero))
            {
                //没有输入 不需要同步
                return;
            }
            
            StateSyncData stateSyncData = new StateSyncData()
            {
                inputDir = new CSVector3()
                {
                    x = _inputDir.x,
                    y = 0,
                    z = _inputDir.y
                },
                playerState = (int)state,
            };
            
            //发送状态同步数据请求
            World.GetExitsLogicManager<LobbyPlayerLogicManager>().SyncRoleState(stateSyncData);
            
            
            _lastInput = _inputDir;
            
        }
    }
    
    
}
