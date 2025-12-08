using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRender : RenderObject
{
    
    
    private LogicActor mAttachTarget;

    // private HeroRender mHeroRender;
    /// <summary>
    /// Buff配置
    /// </summary>
    private BuffConfig mBuffCfg;
    /// <summary>
    /// 输入位置
    /// </summary>
    private FixIntVector3 mInputPos;
    public void InitBuffRender(LogicActor logicObj,LogicActor attachTarget ,BuffConfig buffcfg,FixIntVector3 targetPos)
    {
        base.SetLogicObject(logicObj);
        // mHeroRender = logicObject.RenderObj as HeroRender;
        mBuffCfg = buffcfg;
        mInputPos = targetPos;
        //1.处理音效的播放
        if (buffcfg.buffAudio!=null)
        {
            // AudioController.GetInstance().PlaySoundByAudioClip(buffcfg.buffAudio,false,2);
        }

        //2.处理特效位置以及附加节点
        if (buffcfg.effectConfig.attachType== EffectAttachType.Hand)
        {
            // transform.SetParent(mHeroRender.GetTransParent( TransParentType.LeftHand));
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            switch (mBuffCfg.buffPosType)
            {
                case BuffPosType.None:
                    break;
                case BuffPosType.HitTargetPos:
                    transform.position = attachTarget.LogicPos.ToVector3();
                    break;
                case BuffPosType.ReleaserPos:
                    transform.position = logicObj.LogicPos.ToVector3();
                    break;
                case BuffPosType.UIInputPos:
                    transform.position = mInputPos.ToVector3();
                    break;
            }
        }
        //3.重置粒子特效播放状态
        PlayParticle();
    }


    public override void Update()
    {
        if (mBuffCfg!=null)
        {
            if (mBuffCfg.buffPosType== BuffPosType.FollowTarget)
            {
                transform.position = mAttachTarget.RenderObj.transform.position;
            }
        }
    }

    public void PlayParticle()
    {
        ParticleSystem[] particlesArr= transform.GetComponents<ParticleSystem>();
        foreach (var item in particlesArr)
        {
            item.Play();
        }
    }

    public override void OnRelease()
    {
        base.OnRelease();
        mBuffCfg = null;
        mAttachTarget = null;
        // ZM.AssetFrameWork.ZMAsset.Release(gameObject);
        GameObject.Destroy(gameObject);
    }
}
