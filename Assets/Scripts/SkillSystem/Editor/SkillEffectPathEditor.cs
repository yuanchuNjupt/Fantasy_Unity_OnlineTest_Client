using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SkillEffectPathEditor
{

    public static string[] skillDataCfgPathArr = new string[] { "Assets/GameData/Game/SkillSystem/SkillData/", "Assets/GameData/Game/SkillSystem/BuffData/" };
    [MenuItem("Tools/SyncCfgPrefabPath")]
    public static void SyncCfgPrefabPath()
    {
        for (int i = 0; i < skillDataCfgPathArr.Length; i++)
        {
            string path = skillDataCfgPathArr[i];
            string[] filePathArr = Directory.GetFiles(path, "*");
            foreach (var filePath in filePathArr)
            {
                if (filePath.EndsWith(".asset"))
                {
                    if (i == 0)//技能配置
                    {
                        SkillDataConfig skillData = AssetDatabase.LoadAssetAtPath<SkillDataConfig>(filePath);
                        skillData.skillCfg.GetObjectPath(skillData.skillCfg.skillHitEffect);
                        foreach (SkillEffectConfig effectCfg in skillData.effectCfgList)
                        {
                            effectCfg.GetObjectPath(effectCfg.skillEffect);
                        }
                        foreach (SkillBulletConfig bulletCfg in skillData.bulletCfgList)
                        {
                            bulletCfg.GetBulletObjectPath(bulletCfg.bulletPrefab);
                            bulletCfg.GetHitEffectObjectPath(bulletCfg.hitEffect);
                        }
                        skillData.SaveAsset();
                    }
                    else
                    {
                        //处理buff配置
                        BuffConfig buffData = AssetDatabase.LoadAssetAtPath<BuffConfig>(filePath);
                        buffData.GetObjectPath(buffData.buffHitEffectObj);
                        if (buffData.effectConfig!=null)
                            buffData.effectConfig.GetObjectPath(buffData.effectConfig.effect);
                        buffData.SaveAsset();
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}
