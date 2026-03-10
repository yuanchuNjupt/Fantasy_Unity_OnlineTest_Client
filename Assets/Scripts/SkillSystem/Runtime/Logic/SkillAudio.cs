using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    
    /// <summary>
    /// 音效逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateAudio()
    {
        if (_skillData.audioCfgList!=null&&_skillData.audioCfgList.Count>0)
        {
            foreach (var item in _skillData.audioCfgList)
            {
                //是否达到了当前音效配置的播放触发帧
                if (item.triggerFrame==_curLogicFrame)
                {
                    //播放音效
                    // AudioController.GetInstance().PlaySoundByAudioClip(item.skillAudio,item.isLoop,100);
                }

                //是否是循环音效，并且达到了循环音效的结束帧
                if (item.isLoop&&item.endFrame==_curLogicFrame)
                {
                    //停止当前音效的循环播放  
                    // AudioController.GetInstance().StopSound(item.skillAudio);
                }
            }
        }
    }
}
