using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParticlesAgent : MonoBehaviour
{
#if UNITY_EDITOR

    private ParticleSystem[] mParticleArr;
    private double mLastRunTime;
    public void InitPlayAnim(Transform trans)
    {
        mParticleArr= trans.GetComponentsInChildren<ParticleSystem>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }
    public void OnUpdate()
    {
        if (mLastRunTime == 0)
        {
            mLastRunTime = EditorApplication.timeSinceStartup;
        }
        //获取当前运行的时间
        double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

        if (mParticleArr!=null)
        {
            foreach (var item in mParticleArr)
            {
                if (item!=null)
                {
                    //停止所有粒子动效的播放
                    item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //关闭由随机种子播放的粒子特效
                    item.useAutoRandomSeed = false;
                    //模拟粒子动效的播放
                    item.Simulate((float)curRunTime);
                }
            }
        }
    }
#endif
}
