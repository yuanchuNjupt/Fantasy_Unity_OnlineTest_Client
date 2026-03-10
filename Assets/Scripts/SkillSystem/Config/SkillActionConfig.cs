using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// 行动类型
/// </summary>
public enum MoveActionType
{
    [LabelText("指定目标位置")]TargetPos,
    [LabelText("增量移动(以角色朝向为Z轴)")]DeltaPos,
}
/// <summary>
/// 行动完成后的操作
/// </summary>
public enum MoveActionFinishOption
{ 
    None,
    Skill,
    Buff,
}

[Serializable]
public class DeltaMoveData
{
    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("增量移动值(以角色朝向为Z轴)")]
    public Vector3 deltaPos;
}



[System.Serializable]
public class SkillActionConfig
{

    private bool _showDeltaMoveData;
    private bool _showTargetMoveData;
    
    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("移动方式") , OnValueChanged("OnMoveActionTypeChange")]
    public MoveActionType moveActionType;
    [LabelText("目标位置"), ShowIf("_showTargetMoveData")]
    public Vector3 moveData;
    [LabelText("移动完成所需帧数") , ShowIf("_showTargetMoveData")]
    public int durationFrame;
    
    
    
    
    [LabelText("移动增量列表") , ShowIf("_showDeltaMoveData")]
    public List<DeltaMoveData> deltaMoveData;

#if UNITY_EDITOR
    [LabelText("Root Motion 动画切片"), ShowIf("_showDeltaMoveData")]
    [InfoBox("请拖入带有 Root Motion 位移的动画切片，点击下方按钮自动提取每逻辑帧的增量位移")]
    public AnimationClip rootMotionClip;

    [Button("📐 提取 Root Motion 数据", ButtonSizes.Medium), GUIColor(0.4f, 0.8f, 1f)]
    [ShowIf("_showDeltaMoveData")]
    private void ExtractRootMotionData()
    {
        if (rootMotionClip == null)
        {
            Debug.LogWarning("[Root提取] 请先指定 Root Motion 动画切片！");
            return;
        }

        // 创建临时 GameObject 进行采样
        
        //获取SkillCharacter来采集动画数据
        var samplePrefab = SkillComplierWindow.GetWindow().character.skillCharacter;
            
        var tempGo = GameObject.Instantiate(samplePrefab);
        tempGo.name= tempGo.name.Replace("(Clone)","");
        tempGo.GetComponent<Animator>().applyRootMotion = true;
        
        try
        {
            deltaMoveData = new List<DeltaMoveData>();

            float duration = rootMotionClip.length;
            float frameInterval = LogicFrameConfig.LogicFrameInterval; // 0.066s
            // 与 SkillCharacterConfig.MaxLogicFrame 保持完全一致：截断取整
            // CeilToInt 会多出一个永远不会被触发的幽灵帧，必须用 (int) 截断
            int totalFrames = (int)(duration / frameInterval);

            // 采样第 0 帧，记录起始位置
            rootMotionClip.SampleAnimation(tempGo, 0f);
            Vector3 prevPos = tempGo.transform.position;

            for (int frame = 1; frame <= totalFrames; frame++)
            {
                float t = Mathf.Min(frame * frameInterval, duration);
                rootMotionClip.SampleAnimation(tempGo, t);
                Vector3 curPos = tempGo.transform.position;

                Vector3 delta = curPos - prevPos;
                prevPos = curPos;

                // 只记录有实际位移的帧，避免零帧噪声
                if (delta.sqrMagnitude > 0.00001f)
                {
                    deltaMoveData.Add(new DeltaMoveData
                    {
                        triggerFrame = frame,
                        deltaPos = delta
                    });
                }
            }

            Debug.Log($"[Root提取] 完成！动画时长:{duration:F3}s  总逻辑帧:{totalFrames}  有效位移帧:{deltaMoveData.Count}");
        }
        finally
        {
            // 无论是否异常都清理临时对象
            GameObject.DestroyImmediate(tempGo);
        }
    }
#endif
    
    public void OnMoveActionTypeChange(MoveActionType type)
    {
        _showDeltaMoveData = type is MoveActionType.DeltaPos;
        _showTargetMoveData = type is MoveActionType.TargetPos;
    }
    
    
    
}
