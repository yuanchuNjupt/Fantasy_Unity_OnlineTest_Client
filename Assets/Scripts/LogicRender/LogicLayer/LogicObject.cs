using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
using System;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.FixedCollider.Colliders._3D;

//LogicObject 同时代表 怪物和英雄同时具有的基础属性
/// <summary>
/// 只负责最基础的属性和接口的提供，不负责具体方法的实现
/// </summary>
public abstract class LogicObject
{
    
    private FixedIntVector3 _logicPos = new FixedIntVector3(0,0,0);
    private FixedIntVector3 _logicForwardDir = new FixedIntVector3(0,0,1);
    
    
    private FixedInt _logicMoveSpeed=3;
    private FixedIntVector3 _isActive;
    private bool _isForceAllowMove = false;
    private bool _isForceNotAllowModifyDir = false;
    
    
    //对象是否有新位置
    public bool objectHasNewPos=false;
    
    
    
    //公开属性
    //逻辑对象逻辑位置
    public FixedIntVector3 LogicPos { get { return _logicPos; } set { _logicPos = value; objectHasNewPos = true; } }//逻辑对象逻辑位置
    
    //逻辑对象朝向
    public FixedIntVector3 LogicForwardDir { get { return _logicForwardDir; } set { _logicForwardDir = value; } }//逻辑对象朝向
    
    public FixedIntVector3 LogicRightDir => new FixedIntVector3(_logicForwardDir.Z, 0, -_logicForwardDir.X);//逻辑对象右方向
    
    public FixedIntVector3 LogicUpDir => new FixedIntVector3(0, 1, 0);//逻辑对象上方向
    
    

    /// <summary>
    /// 当前逻辑朝向与世界正方向(0,0,1)的Y轴夹角（角度，顺时针为正），用于初始化技能碰撞体旋转
    /// </summary>
    public FixedInt LogicRotationY => FixedIntMathf.Atan2(_logicForwardDir.X, _logicForwardDir.Z) * FixedIntMathf.Rad2Deg;


    //逻辑对象移动速度
    public FixedInt LogicMoveSpeed { get { return _logicMoveSpeed; } set { _logicMoveSpeed = value; } }//逻辑对象移动速度
    
    //逻辑轴向
    // public FixedInt LogicXAxis { get { return _logicXAxis; } set { _logicXAxis = value; } }//逻辑轴向
    
    //当前逻辑对象是否激活
    public FixedIntVector3 IsActive { get { return _isActive; } set { _isActive = value; } }//当前逻辑对象是否激活
    
    //是否强制允许移动
    public bool IsForceAllowMove { get { return _isForceAllowMove; } set { Debug.Log("isForceAllowMove:"+ _isForceAllowMove); _isForceAllowMove = value; } }//是否强制允许移动
    
    //是否允许修改朝向
    public bool IsForceNotAllowModifyDir { get { return _isForceNotAllowModifyDir; } set { Debug.Log("isForceAlllowModifyDir:" + _isForceAllowMove); _isForceNotAllowModifyDir = value; } }//是否允许修改朝向

    /// <summary>
    /// 渲染对象
    /// </summary>
    public RenderObject RenderObj { get; protected set; }
    /// <summary>
    /// 定点数碰撞体
    /// </summary>
    public FixedIntCollider3D Collider { get; protected set; }
    /// <summary>
    /// 逻辑对象状态
    /// </summary>
    public LogicObjectState ObjectState { get; set; }
    /// <summary>
    /// 逻辑对象类型
    /// </summary>
    public LogicObjectType ObjectType { get; set; }

    
    private LogicObjectActionState _actionState = LogicObjectActionState.Idle;
    
    /// <summary>
    /// 逻辑对象行动状态
    /// </summary>
    public LogicObjectActionState ActionState
    {
        get => _actionState;
        set
        {
            if (_actionState != value)
            {
                _actionState = value;
                onActionStateChange?.Invoke(_actionState);
            }
        }
    }

    public Action<LogicObjectActionState> onActionStateChange;
    
    
    
    /// <summary>
    /// 死亡回调
    /// </summary>
    public Action OnDeathCallBack;
    /// <summary>
    /// 初始化接口
    /// </summary>
    public virtual void OnCreate()
    {

    }
    /// <summary>
    /// 逻辑帧更新接口
    /// </summary>
    public virtual void OnLogicFrameUpdate()
    {

    }
    /// <summary>
    /// 逻辑对象释放接口
    /// </summary>
    public virtual void OnDestroy()
    {
        
    }
}
public enum LogicObjectActionState
{
    Idle,
    Move,
    ReleasingSkillBefore,
    ReleasingSkillAfter,
    OnHit,
}

public enum LogicObjectType
{
    Hero,
    Monster,
    Effect,
}

public enum LogicObjectState
{
    Survival,//存活中
    Death,//死亡
}