using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

[System.Serializable]
public class SkillDamageConfig  
{
    [LabelText("触发帧")]
    public int triggerFrame;//触发帧
    
    [LabelText("结束帧")]
    public int endFrame;//结束帧
    
    [LabelText("是否跟随特效移动")]
    public bool isFollowEffect;
    
    [LabelText("伤害倍率")]
    public int damageRate;//伤害倍率
    
    [LabelText("伤害检测方式"),OnValueChanged("OnDetectionValueChange")]
    public DamageDetectionMode detectionMode;

    #region Box碰撞体参数

    [LabelText("Box碰撞体宽高"),ShowIf("_showBox3D"),OnValueChanged("OnBoxValueChange")]
    public Vector3 boxSize = new Vector3(1, 1, 1);
    
    [LabelText("Box碰撞体偏移"), ShowIf("_showBox3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 boxOffset = new Vector3(0, 0, 0);

    #endregion

    #region Sphere碰撞体参数

    [LabelText("圆球碰撞体偏移值"), ShowIf("_showSphere3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 sphereOffset = new Vector3(0,0.9f,0);

    [LabelText("圆球伤害检测半径"), ShowIf("_showSphere3D"), OnValueChanged("OnRadiusValueChange")]
    public float sphereRadius = 1;

    #endregion

    #region Cylinder碰撞体参数

    [LabelText("圆柱体碰撞体偏移值"), ShowIf("_showCylinder3D"),OnValueChanged("OnColliderOffsetChange")]
    public Vector3 cylinderOffset = new Vector3(0,0.9f,0);
    
    [LabelText("圆柱体伤害检测半径"), ShowIf("_showCylinder3D"),OnValueChanged("OnRadiusValueChange")]
    public float cylinderRadius = 1;
    
    [LabelText("圆柱体伤害检测高度"), ShowIf("_showCylinder3D"),OnValueChanged("OnCylinderHeightValueChange")]
    public float cylinderHeight = 1;
    

    #endregion
    
    [LabelText("技能命中特效")]
    public GameObject hitEffectPrefab;
    
    [LabelText("技能命中特效持续时间（毫秒）")]
    public int hitEffectSurvivalTimeMs;
    
    [LabelText("技能命中音效")]
    public AudioClip hitAudioClip;
    
    
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType = ColliderPosType.FixedDir;
    
    [LabelText("伤害触发目标")]
    public TargetType targetType;
    
    
#if UNITY_EDITOR
    private bool _showBox3D;//是否显示3DBox碰撞体
    private bool _showSphere3D;//是否显示3D圆球碰撞体
    private bool _showCylinder3D;//是否显示3D圆柱体碰撞体
    
    private FixedIntBoxCollider _boxCollider;
    private FixedIntSphereCollider _sphereCollider;
    private FixedIntCylinderCollider _cylinderCollider;
    
    private int _curLogicFrame;//当前执行到的逻辑帧
    

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
        else if (detectionMode == DamageDetectionMode.Cylinder3D)
        {
            return  characterPos + cylinderOffset;
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        DestroyCollider();
        if (detectionMode == DamageDetectionMode.Box3D)
        {
            _boxCollider = new FixedIntBoxCollider(
                GetColliderOffsetPos(),
                FixedIntVector3.zero,
                new FixedIntVector3(boxSize),
                0,
                FixedIntCollider3DType.AABB);
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D)
        {
            _sphereCollider = new FixedIntSphereCollider(
                GetColliderOffsetPos(),
                FixedIntVector3.zero,
                sphereRadius);
        }
        else if (detectionMode == DamageDetectionMode.Cylinder3D)
        {
            _cylinderCollider = new FixedIntCylinderCollider(
                cylinderRadius,
                cylinderHeight,
                GetColliderOffsetPos(),
                FixedIntVector3.zero);
        }
    }
    public void DestroyCollider()
    {
        _boxCollider?.OnDestroy();

        _sphereCollider?.OnDestroy();

        _cylinderCollider?.OnDestroy();
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


    #region 变化检测方法

    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="newDetectionMode"></param>
    public void OnDetectionValueChange(DamageDetectionMode newDetectionMode)
    {
        _showBox3D = newDetectionMode == DamageDetectionMode.Box3D;
        _showSphere3D = newDetectionMode == DamageDetectionMode.Sphere3D ;
        _showCylinder3D = newDetectionMode == DamageDetectionMode.Cylinder3D ;
        CreateCollider();
    }
    /// <summary>
    /// 圆球/圆柱体碰撞体检测半径或高度发生变化
    /// </summary>
    public void OnRadiusValueChange(float newValue)
    {
        if (detectionMode == DamageDetectionMode.Sphere3D)
        {
            if (_sphereCollider != null)
                _sphereCollider.UpdateRadius(sphereRadius);
            else
                Debug.LogError("_sphereCollider is Null！");
        }
        else if (detectionMode == DamageDetectionMode.Cylinder3D)
        {
            if (_cylinderCollider != null)
            {
                _cylinderCollider.UpdateRadius(cylinderRadius);
            }
            else
                Debug.LogError("_cylinderCollider is Null！");
        }
    }
    
    public void OnCylinderHeightValueChange(float newValue)
    {
        if (detectionMode == DamageDetectionMode.Cylinder3D)
        {
            if (_cylinderCollider != null)
            {
                _cylinderCollider.UpdateHeight(cylinderHeight);
            }
            else
                Debug.LogError("_cylinderCollider is Null！");
        }
    }
    
    /// <summary>
    /// 碰撞体中心点偏移发生变化
    /// </summary>
    public void OnColliderOffsetChange(Vector3 newCenter)
    {
        if (detectionMode == DamageDetectionMode.Box3D && _boxCollider != null)
        {
            _boxCollider.UpdatePosition(GetColliderOffsetPos());
        }
        else if (detectionMode == DamageDetectionMode.Sphere3D && _sphereCollider != null)
        {
            _sphereCollider.UpdatePosition(GetColliderOffsetPos());
        }
        else if (detectionMode == DamageDetectionMode.Cylinder3D && _cylinderCollider != null)
        {
            _cylinderCollider.UpdatePosition(GetColliderOffsetPos());
        }
        else
        {
            Debug.LogError("Collider is Null！");
        }
    }
    /// <summary>
    /// Box碰撞体宽高发生变化
    /// </summary>
    public void OnBoxValueChange(Vector3 size)
    {
        if (_boxCollider != null)
        {
            _boxCollider.UpdateSize(new FixedIntVector3(size));
            _boxCollider.UpdatePosition(GetColliderOffsetPos());
        }
        else
            Debug.LogError("_boxCollider is Null！");
    }

    #endregion
    
    

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
    [LabelText("位置固定且与角色朝向同向")] FixedDir,//固定与角色朝向同向
    [LabelText("跟随角色位置但不旋转")] FollowPos,//跟随角色位置
}


public enum DamageDetectionMode
{
    [LabelText("无配置")] None,//无配置
    [LabelText("3DBox碰撞检测")] Box3D,//3DBox碰撞检测
    [LabelText("3D圆球碰撞检测")] Sphere3D,//3D圆球碰撞检测
    [LabelText("3D圆柱体碰撞检测")] Cylinder3D,//3D圆柱体碰撞检测
    [LabelText("所有目标")] AllTarget,//通过代码搜索的所有目标
}