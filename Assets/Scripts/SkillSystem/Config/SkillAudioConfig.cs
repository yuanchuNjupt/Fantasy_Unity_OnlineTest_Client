using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SkillAudioConfig  
{
    [AssetList]
    [BoxGroup("音效文件"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("OnAudioChange")]
    public AudioClip skillAudio;

    [LabelText("音效文件名称"),BoxGroup("音效文件"),ReadOnly,GUIColor("green")]
    public string audioName;

    [BoxGroup("参数配置"),LabelText("触发帧"),GUIColor("green")]
    public int triggerFrame;

    [ToggleGroup("isLoop","是否循环")]
    public bool isLoop = false;

    [ToggleGroup("isLoop","结束帧")]
    public int endFrame;

    public void OnAudioChange()
    {
        if (skillAudio!=null)
        {
            audioName = skillAudio.name;
        }
    }
}
