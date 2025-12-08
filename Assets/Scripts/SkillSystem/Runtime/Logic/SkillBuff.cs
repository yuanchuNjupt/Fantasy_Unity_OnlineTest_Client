using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill 
{
    /// <summary>
    /// buff逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateBuff()
    {
        if (mSkillData.buffCfgList != null && mSkillData.buffCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.buffCfgList.Count; i++)
            {
                SkillBuffConfig buffcfg= mSkillData.buffCfgList[i];
                if (mCurLogicFrame== buffcfg.triggerFrame)
                {
                    BuffSystem.MainInstance.AttachBuff(buffcfg.buffid,mSkillCreater,mSkillCreater,this);
                }
            }
        }
    }
}
