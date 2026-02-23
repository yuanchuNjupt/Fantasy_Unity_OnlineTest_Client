using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.FixedCollider.Colliders._3D;
using UnityEngine;

[System.Serializable]
public class SkillDamageConfig  
{
    [LabelText("触发帧")]
    public int triggerFrame;//触发帧
    
    [LabelText("结束帧")]
    public int endFrame;//结束帧
    
    [LabelText("触发间隔（ value=0 默认一次，>0则为间隔）")]
    public int triggerIntervalFrame;//触发间隔（毫秒 value=0 默认一次，>0则为间隔）
    
    [LabelText("是否跟随特效移动")]
    public bool isFollowEffect;//碰撞体是否跟随特效移动
    
    [LabelText("伤害倍率")]
    public int damageRate;//伤害倍率
    
    [LabelText("伤害检测方式"),OnValueChanged("OnDetectionValueChange")]
    public DamageDetectionMode detectionMode;//伤害检测方式
    
    [LabelText("Box碰撞体宽高"),ShowIf("_showBox3D"),OnValueChanged("OnBoxValueChange")]
    public Vector3 boxSize = new Vector3(1, 1, 1);//Box碰撞的大小
    
    [LabelText("Box碰撞体偏移"), ShowIf("_showBox3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 boxOffset = new Vector3(0, 0, 0);//Box碰撞体偏移值
    
    [LabelText("圆球碰撞体偏移值"), ShowIf("_showSphere3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 sphereOffset = new Vector3(0,0.9f,0);//圆球碰撞体偏移值
    
    [LabelText("圆球伤害检测半径"), ShowIf("_showSphere3D"),OnValueChanged("OnRadiusValueChange")]
    public float radius = 1;//圆球伤害检查半径
    
    [LabelText("圆球检测半径高度"), ShowIf("_showSphere3D")]
    public float radiusHeight = 0;//圆球检测半径高度
    
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType = ColliderPosType.FollowDir;//碰撞体位置类型
    
    [LabelText("伤害触发目标")]
    public TargetType targetType;//伤害触发目标
    
    
#if UNITY_EDITOR
    private bool _showBox3D;//是否显示3DBox碰撞体
    private bool _showSphere3D;//是否显示3D圆球碰撞体
    private FixedIntBoxCollider _boxCollider;
    private FixedIntSphereCollider _sphereCollider;
    private int _curLogicFrame;//当前执行到的逻辑帧
    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="newDetectionMode"></param>
    public void OnDetectionValueChange(DamageDetectionMode newDetectionMode)
    {
        _showBox3D = newDetectionMode == DamageDetectionMode.Box3D;
        _showSphere3D = newDetectionMode == DamageDetectionMode.Sphere3D ;
        CreateCollider();
    }
    /// <summary>
    /// 圆球碰撞体检测半径发生变化
    /// </summary>
    public void OnRadiusValueChange(float newRadius)
    {
        // if (_sphereCollider!=null)
        //     _sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        // else
        //     Debug.LogError("_sphereCollider is Null！");
    }
    /// <summary>
    /// 碰撞体中心点发生变化
    /// </summary>
    public void OnColliderOffsetChange(Vector3 newCenter)
    {
        // if (detectionMode == DamageDetectionMode.Box3D&& _boxCollider!=null)
        // {
        //     _boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
        // }
        // else if (detectionMode == DamageDetectionMode.Sphere3D&& _sphereCollider!=null)
        // {
        //     _sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        // }
    }
    /// <summary>
    /// Box碰撞体宽高发生变化
    /// </summary>
    public void OnBoxValueChange(Vector3 size)
    {
        // if (_boxCollider!=null)
        //     _boxCollider.SetBoxData(GetColliderOffsetPos(), size,colliderPosType == ColliderPosType.FollowPos);
        // else
        //     Debug.LogError("_boxCollider is Null！");
    }
    /// <summary>
    /// 获取碰撞体的偏移值
    /// </summary>
    /// <returns></returns>
    public Vector3 GetColliderOffsetPos()
    {
        Vector3 characterPos= SkillComplierWindow.GetCharacterPos();
        if (detectionMode == DamageDetectionMode.Box3D)
        {
            return characterPos + boxOffset;
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D)
        {
            return characterPos + sphereOffset;
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        // DestroyCollider();
        // if (detectionMode== DamageDetectionMode.Box3D)
        // {
        //     _boxCollider = new FixIntBoxCollider(boxSize, GetColliderOffsetPos());
        //     _boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize,colliderPosType== ColliderPosType.FollowPos);
        // }
        // else if (detectionMode== DamageDetectionMode.Sphere3D)
        // {
        //     _sphereCollider = new FixIntSphereCollider(radius, GetColliderOffsetPos());
        //     _sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        // }
    }
    public void DestroyCollider()
    {
        // if (_boxCollider != null)
        // {
        //     _boxCollider.OnRelease();
        // }
        // if (_sphereCollider != null)
        // {
        //     _sphereCollider.OnRelease();
        // }
    }
    /// <summary>
    /// 当前窗口初始化
    /// </summary>
    public void OnInit()
    {
        CreateCollider();
    }
    /// <summary>
    /// 当前窗口关闭
    /// </summary>
    public void OnRelease()
    {
        DestroyCollider();
    }

    public void PlaySkillStart()
    {
        _curLogicFrame = 0;
        DestroyCollider();
    }

    public void PlaySkillEnd()
    {
        DestroyCollider();
    }
    public void OnLogicFrameUpdate()
    {
        //是否到达触发帧
        if (_curLogicFrame == triggerFrame)
        {
            CreateCollider();
        }
        else if (_curLogicFrame == endFrame)
        {
            DestroyCollider();
        }
        _curLogicFrame++;
    }

#endif
}
public enum TargetType
{
    [LabelText("未配置")] None,//未配置
    [LabelText("队友")] Teammate,//队友
    [LabelText("敌人")] Enemy,//敌人
    [LabelText("自身")] Self,//自身
    [LabelText("所有对象")] AllObject,//所有对象
}

public enum ColliderPosType
{
    [LabelText("跟随角色朝向")] FollowDir,//跟随角色朝向
    [LabelText("跟随角色位置")] FollowPos,//跟随角色位置
    [LabelText("中心坐标")] CenterPos,//中心坐标
    [LabelText("目标位置")] TargetPos,//目标位置
}

public enum DamageType
{
    [LabelText("无伤害")]None,//无伤害
    [LabelText("物理伤害")] ADDamage,//物理伤害
    [LabelText("魔法伤害")] APDamage,//魔法伤害
}

public enum DamageDetectionMode
{
    [LabelText("无配置")] None,//无配置
    [LabelText("3DBox碰撞检测")] Box3D,//3DBox碰撞检测
    [LabelText("3D圆球碰撞检测")] Sphere3D,//3D圆球碰撞检测
    [LabelText("3D圆柱体碰撞检测")] Cylinder3D,//3D圆柱体碰撞检测
    [LabelText("半径的距离")] RadiusDistance,//半径的距离 （代码搜索）
    [LabelText("所有目标")] AllTarget,//通过代码搜索的所有目标
}