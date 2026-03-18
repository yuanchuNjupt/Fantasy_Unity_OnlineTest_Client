using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrameConfig 
{
    //逻辑帧id 自增
    public static long LogicFrameId;
    
    
    //实际逻辑帧间隔
    public const float LogicFrameInterval = 0.066f; //一秒15帧

    //毫秒级逻辑帧间隔，用来计算当前逻辑帧累加时间
    public const int LogicFrameIntervalMs = 66; //一秒15帧

    public const float InputSampleInterval = 0.033f; //输入采样间隔，一秒30帧
    
    
    
    
    /// <summary>
    /// 是否使用本地逻辑帧
    /// </summary>
    public const bool IsUseLocalLogicFrame = false;

    /// <summary>
    /// 最大预测逻辑帧次数
    /// </summary>
    public const int PreMaxMoveLogicFrameCount = 5;
}
