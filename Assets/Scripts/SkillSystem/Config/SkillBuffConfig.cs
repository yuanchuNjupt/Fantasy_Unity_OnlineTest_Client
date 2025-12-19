using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkillBuffConfig 
{
    [LabelText("触发帧"), GUIColor("green")]
    public int triggerFrame;


    [LabelText("附加BuffId")]
    public int buffid;

}
