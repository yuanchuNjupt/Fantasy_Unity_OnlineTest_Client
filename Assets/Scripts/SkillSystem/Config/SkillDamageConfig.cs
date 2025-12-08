using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixIntPhysics;

[System.Serializable]
public class SkillDamageConfig  
{
    [LabelText("触发帧")]
    public int triggerFrame;//触发帧
    [LabelText("结束帧")]
    public int endFrame;//结束帧
    [LabelText("触发间隔（毫秒 value=0 默认一次，>0则为间隔）")]
    public int triggerIntervalMs;//触发间隔（毫秒 value=0 默认一次，>0则为间隔）
    [LabelText("是否跟随特效移动")]
    public bool isFollowEffect;//碰撞体是否跟随特效移动
    [LabelText("伤害配置")]
    public DamageType damageType;//伤害配置
    [LabelText("伤害倍率")]
    public int damageRate;//伤害倍率
    [LabelText("伤害检测方式"),OnValueChanged("OnDectectionValueChange")]
    public DamageDetectionMode detectionMode;//伤害检测方式
    [LabelText("Box碰撞体宽高"),ShowIf("mShowBox3D"),OnValueChanged("OnBoxValueChange")]
    public Vector3 boxSize = new Vector3(1, 1, 1);//Box碰撞的大小
    [LabelText("Box碰撞体偏移"), ShowIf("mShowBox3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 boxOffset = new Vector3(0, 0, 0);//Box碰撞体偏移值
    [LabelText("圆球碰撞体偏移值"), ShowIf("mShowShpere3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 sphereOffset = new Vector3(0,0.9f,0);//圆球碰撞体偏移值
    [LabelText("圆球伤害检测半径"), ShowIf("mShowShpere3D"),OnValueChanged("OnRaduisValueChange")]
    public float raduis = 1;//圆球伤害检查半径
    [LabelText("圆球检测半径高度"), ShowIf("mShowShpere3D")]
    public float raduisHeight = 0;//圆球检测半径高度
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType = ColliderPosType.FollowDir;//碰撞体位置类型
    [LabelText("伤害触发目标")]
    public TargetType targetType;//伤害触发目标
    [TitleGroup("附加Buff","伤害生效的一瞬间，附加指定的多个buff")]
    public int[] addBuffs;//技能附加BUff
    [TitleGroup("触发后续技能", "造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillid;//触发技能id
#if UNITY_EDITOR
    private bool mShowBox3D;//是否显示3Dbox碰撞体
    private bool mShowShpere3D;//是否显示3D圆球碰撞体
    private FixIntBoxCollider boxCollider;
    private FixIntSphereCollider sphereCollider;
    private int mCurLogicFrame=0;//当前执行到的逻辑帧
    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="detectionMode"></param>
    public void OnDectectionValueChange(DamageDetectionMode detectionMode)
    {
        mShowBox3D = detectionMode == DamageDetectionMode.BOX3D;
        mShowShpere3D = detectionMode == DamageDetectionMode.Sphere3D ;
        CreateCollider();
    }
    /// <summary>
    /// 圆球碰撞体检测半径发生变化
    /// </summary>
    /// <param name="raduis"></param>
    public void OnRaduisValueChange(float raduis)
    {
        if (sphereCollider!=null)
            sphereCollider.SetBoxData(raduis, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        else
            Debug.LogError("sphereCollider is Null！");
    }
    /// <summary>
    /// 碰撞体中心点发生变化
    /// </summary>
    public void OnColliderOffsetChange(Vector3 conter)
    {
        if (detectionMode == DamageDetectionMode.BOX3D&& boxCollider!=null)
        {
            boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D&& sphereCollider!=null)
        {
            sphereCollider.SetBoxData(raduis, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        }
    }
    /// <summary>
    /// Box碰撞体宽高发生变化
    /// </summary>
    public void OnBoxValueChange(Vector3 size)
    {
        if (boxCollider!=null)
            boxCollider.SetBoxData(GetColliderOffsetPos(), size,colliderPosType == ColliderPosType.FollowPos);
        else
            Debug.LogError("boxCollider is Null！");
    }
    /// <summary>
    /// 获取碰撞体的偏移值
    /// </summary>
    /// <returns></returns>
    public Vector3 GetColliderOffsetPos()
    {
        Vector3 charaterPos= SkillComplierWindow.GetCharaterPos();
        if (detectionMode == DamageDetectionMode.BOX3D)
        {
            return charaterPos + boxOffset;
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D)
        {
            return charaterPos + sphereOffset;
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        DestroyCollider();
        if (detectionMode== DamageDetectionMode.BOX3D)
        {
            boxCollider = new FixIntBoxCollider(boxSize, GetColliderOffsetPos());
            boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize,colliderPosType== ColliderPosType.FollowPos);
        }
        else if (detectionMode== DamageDetectionMode.Sphere3D)
        {
            sphereCollider = new FixIntSphereCollider(raduis, GetColliderOffsetPos());
            sphereCollider.SetBoxData(raduis, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        }
    }
    public void DestroyCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.OnRelease();
        }
        if (sphereCollider != null)
        {
            sphereCollider.OnRelease();
        }
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
        mCurLogicFrame = 0;
        DestroyCollider();
    }

    public void PlaySkilEnd()
    {
        DestroyCollider();
    }
    public void OnLogicFrameUpdate()
    {
        //是否到达触发帧
        if (mCurLogicFrame == triggerFrame)
        {
            CreateCollider();
        }
        else if (mCurLogicFrame == endFrame)
        {
            DestroyCollider();
        }
        mCurLogicFrame++;
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
    [LabelText("中心坐标")] ConterPos,//中心坐标
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
    [LabelText("3DBox碰撞检测")] BOX3D,//3DBox碰撞检测
    [LabelText("3D圆球碰撞检测")] Sphere3D,//3D圆球碰撞检测
    [LabelText("3D圆柱体碰撞检测")] Cylinder3D,//3D圆柱体碰撞检测
    [LabelText("半径的距离")] RadiusDistance,//半径的距离 （代码搜索）
    [LabelText("所有目标")] AllTarget,//通过代码搜索的所有目标
}