using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimationAgnet  
{
#if UNITY_EDITOR
    private Animation mAnim;
    private double mLastRunTime;
    public void InitPlayAnim(Transform trans)
    {
        mAnim= trans.GetComponentInChildren<Animation>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }
    public void OnUpdate()
    {
        if (mAnim!=null&&mAnim.clip!=null)
        {
            if (mLastRunTime == 0)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }
            //获取当前运行的时间
            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

            //计算动画播放进度
            float curAnimNormalizationValue = (float)curRunTime / mAnim.clip.length;
            //采样动画，进行动画播放
            mAnim.clip.SampleAnimation(mAnim.gameObject, (float)curRunTime);
        }
    
    }
#endif
}
