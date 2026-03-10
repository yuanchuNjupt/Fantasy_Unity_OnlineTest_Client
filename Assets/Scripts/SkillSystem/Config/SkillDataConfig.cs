using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
# if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[CreateAssetMenu(fileName ="SkillConfig",menuName ="SkillConfig", order =0)]
public class SkillDataConfig : ScriptableObject
{
    //角色数据配置
    public SkillCharacterConfig character;
    //技能基础数据配置
    public SkillConfig skillCfg;
    //技能伤害配置列表
    public List<SkillDamageConfig> damageCfgList;
    //技能特效配置列表
    public List<SkillEffectConfig> effectCfgList;
    //技能音效配置列表
    public List<SkillAudioConfig> audioCfgList;
    //行动配置列表
    public List<SkillActionConfig> actionCfgList;

    
#if UNITY_EDITOR
    public static void SaveSkillData(SkillCharacterConfig characterCfg,SkillConfig skillCfg,List<SkillDamageConfig> damageCfgList, List<SkillEffectConfig> effectCfgList
        , List<SkillAudioConfig> audioCfgList, List<SkillActionConfig> actionCfgList)
    {
        //通过代码创建SkillDataConfig的实例，并对字段进行赋值储存
        SkillDataConfig skillDataCfg= CreateInstance<SkillDataConfig>();
        skillDataCfg.character = characterCfg;
        skillDataCfg.skillCfg = skillCfg;
        skillDataCfg.damageCfgList = damageCfgList;
        skillDataCfg.effectCfgList = effectCfgList;
        skillDataCfg.audioCfgList = audioCfgList;
        skillDataCfg.actionCfgList = actionCfgList;

        //把当前实例储存为.asset资源文件，当作技能配置
        
        // 使用相对于项目根目录的路径（AssetDatabase 要求）
        // 路径需要与 LoadPathConfig.SkillLoadPath 匹配：Resources/Skills/
        string folderPath = "Assets/Resources/Skills/";
        string fileName = skillCfg.skillid + ".asset";
        string assetPath = folderPath + "/" + fileName;
        
        string physicalFolderPath = Application.dataPath + "/Resources/Skills/";
        if (!System.IO.Directory.Exists(physicalFolderPath))
        {
            System.IO.Directory.CreateDirectory(physicalFolderPath);
            AssetDatabase.Refresh();
        }
        
        //如果资源对象已存在，先进行删除，在进行创建
        if (AssetDatabase.LoadAssetAtPath<SkillDataConfig>(assetPath) != null)
        {
            AssetDatabase.DeleteAsset(assetPath);
        }
        
        AssetDatabase.CreateAsset(skillDataCfg, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"技能数据已保存: {assetPath}");
    }

    [Button("配置技能",ButtonSizes.Large),GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        SkillComplierWindow window= SkillComplierWindow.ShowWindow();
        window.LoadSkillData(this);
    }

    public  void SaveAsset()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}

