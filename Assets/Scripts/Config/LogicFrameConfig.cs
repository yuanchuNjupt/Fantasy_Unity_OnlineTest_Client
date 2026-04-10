using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrameConfig 
{
    //服务器权威逻辑帧ID
    public static long ServerLogicFrameId;
    
    //本地预测逻辑帧ID
    public static long LocalPredictedLogicFrameId;
    
    
    //实际逻辑帧间隔
    public const float LogicFrameInterval = 0.066f; //一秒15帧

    //毫秒级逻辑帧间隔，用来计算当前逻辑帧累加时间
    public const int LogicFrameIntervalMs = 66; //一秒15帧

    public const float InputSampleInterval = 0.033f; //输入采样间隔，一秒30帧
    
    public const int MaxCachedLogicFrameCount = 100; //最大缓存逻辑帧数量，超过后会丢弃最早的逻辑帧
    
    
    
    
    /// <summary>
    /// 是否使用本地逻辑帧
    /// </summary>
    public const bool IsUseLocalLogicFrame = true;

    /// <summary>
    /// 最大预测逻辑帧次数
    /// </summary>
    public const int PreMaxMoveLogicFrameCount = 5;
}
