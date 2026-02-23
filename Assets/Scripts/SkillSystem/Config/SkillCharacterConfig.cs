using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillCharacterConfig 
{
    [AssetList]
    [LabelText("角色模型")]
    [PreviewField(70,ObjectFieldAlignment.Center)]
    public GameObject skillCharacter;
    
    [LabelText("技能动画")]
    [TitleGroup("技能渲染","所有英雄渲染数据会在技能开始释放时触发")]
    [OnValueChanged("OnAnimationClipChanged")]
    public AnimationClip skillAnim;

    [BoxGroup("动画数据")]
    [ProgressBar(0,"_maxAnimationLength",r:0,g:255,b:0,Height =30)]
    [HideLabel]
    [OnValueChanged("OnAnimProgressValueChange")]
    public int animProgress = 0;
    
    [BoxGroup("动画数据")] 
    [LabelText("最大逻辑帧数")]
    [ReadOnly]
    public int MaxLogicFrame;
    
    
    [LabelText("逻辑帧数")]
    [BoxGroup("动画数据"),HideIf("isSetCustomLogicFrame")]
    public int logicFrame = 0;
    
    [LabelText("是否设置自定义逻辑帧数")]
    [BoxGroup("动画数据")]
    public bool isSetCustomLogicFrame=false;
    
    [LabelText("自定义逻辑帧数")]
    [BoxGroup("动画数据"),ShowIf("isSetCustomLogicFrame")]
    public int customLogicFame = 0;

    private GameObject mTempCharacter;
    
    private int _maxAnimationLength;
    
    private bool _isPlayAnim=false;//是否播放动画，用来控制暂停动画
    
    private double _lastRunTime = 0;//上次运行的时间
    
    private Animator _animator;

#if UNITY_EDITOR
    [GUIColor(0.4f, 0.8f, 1)]
    [ButtonGroup("按钮数组")]
    [Button("播放",  ButtonSizes.Large)]
    public void Play()
    {
        if (skillCharacter!=null)
        {
            //先从场景中查找技能对象，如果查找不到，就主动克隆一个
            string charactorName= skillCharacter.name;
            mTempCharacter= GameObject.Find(charactorName);
            if (mTempCharacter == null)
            {
                mTempCharacter = GameObject.Instantiate(skillCharacter);
                mTempCharacter.name= mTempCharacter.name.Replace("(Clone)","");
            }
            //计算逻辑帧长度（个数）
            logicFrame = (int)(skillAnim.length / LogicFrameConfig.LogicFrameInterval);
            
            _lastRunTime = 0;
            //开始播放角色动画
            _isPlayAnim = true;
            SkillComplierWindow window= SkillComplierWindow.GetWindow();
            window?.StartPlaySkill();
        }
    }
    [ButtonGroup("按钮数组")]
    [Button("暂停", ButtonSizes.Large)]
    public void Pause()
    {
        _isPlayAnim = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SkillPause();
    }
    [GUIColor(0, 1, 0)]
    [ButtonGroup("按钮数组")]
    [Button("保存配置", ButtonSizes.Large)]
    public void SaveAssets()
    {
        SkillComplierWindow.GetWindow().SaveSKillData();
    }

    public void OnUpdate(System.Action progressUpdateCallback)
    {
        if (_isPlayAnim)
        {
            if (_lastRunTime==0)
            {
                _lastRunTime = EditorApplication.timeSinceStartup;
            }
            //获取当前运行的时间
            double curRunTime = EditorApplication.timeSinceStartup - _lastRunTime;

            //计算动画播放进度
            animProgress = Mathf.Clamp((int)(curRunTime * 1000f) , 0 , _maxAnimationLength);
            //计算逻辑帧
            logicFrame = (int)(curRunTime / LogicFrameConfig.LogicFrameInterval);
            //采样动画，进行动画播放
            skillAnim.SampleAnimation(mTempCharacter,animProgress / 1000f);

            if (animProgress==_maxAnimationLength)
            {
                //动画播放完成
                PlaySkillEnd();
            }
            //触发窗口聚焦回调，刷新窗口
            progressUpdateCallback?.Invoke();
        }
    }
    /// <summary>
    /// 动画进度改变监听
    /// </summary>
    /// <param name="value"></param>
    public void OnAnimProgressValueChange(int value)
    {
        //先从场景中查找技能对象，如果查找不到，就主动克隆一个
        string characterName = skillCharacter.name;
        mTempCharacter = GameObject.Find(characterName);
        if (mTempCharacter == null)
        {
            mTempCharacter = GameObject.Instantiate(skillCharacter);
            mTempCharacter.name = mTempCharacter.name.Replace("(Clone)", "");
        }

        //根据当前动画进度进行动画采样
        logicFrame = value / LogicFrameConfig.LogicFrameIntervalMs;
        //采样动画，进行动画播放
        skillAnim.SampleAnimation(mTempCharacter, value / 1000f);
    }
    
    public void OnAnimationClipChanged(AnimationClip clip)
    {
        if(clip == null) return;
        _maxAnimationLength = (int)(clip.length * 1000);
        MaxLogicFrame = (int)(clip.length / LogicFrameConfig.LogicFrameInterval);
    }
    
    
    
    
    public void PlaySkillEnd()
    {
        _isPlayAnim = false;

        SkillComplierWindow window= SkillComplierWindow.GetWindow();
        window?.PlaySkillEnd();
    }

    /// <summary>
    /// 在读取配置文件的时候初始化
    /// </summary>
    public void Init()
    {
        _maxAnimationLength = (int)(skillAnim.length * 1000);
        if (skillAnim != null)
        {
            MaxLogicFrame = (int)(skillAnim.length / LogicFrameConfig.LogicFrameInterval);
        }
    }
    
    
#endif
}
