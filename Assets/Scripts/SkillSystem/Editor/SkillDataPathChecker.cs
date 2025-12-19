#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SkillSystem.Editor
{
    public class SkillDataPathChecker
    {
        [MenuItem("Tools/技能系统/检查技能数据路径")]
        public static void CheckSkillDataPaths()
        {
            string oldPath = Application.dataPath + "/Resources/Skills";
            string newPath = Application.dataPath + "/Resources/Skills/SkillData";
            
            Debug.Log("=== 技能数据路径检查 ===");
            
            // 检查新路径是否存在
            if (!Directory.Exists(newPath))
            {
                Debug.LogWarning($"SkillData 文件夹不存在，正在创建: {newPath}");
                Directory.CreateDirectory(newPath);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log($"✓ SkillData 文件夹存在: {newPath}");
            }
            
            // 检查旧路径中的技能文件
            if (Directory.Exists(oldPath))
            {
                string[] files = Directory.GetFiles(oldPath, "*.asset");
                if (files.Length > 0)
                {
                    Debug.LogWarning($"发现 {files.Length} 个旧格式的技能文件:");
                    foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        // 只检查数字命名的文件（如 1001.asset）
                        if (int.TryParse(Path.GetFileNameWithoutExtension(fileName), out int skillId))
                        {
                            Debug.LogWarning($"  - {fileName} (技能ID: {skillId})");
                            Debug.LogWarning($"    建议删除此文件并重新创建为: SkillData/SkillData_{skillId}.asset");
                        }
                    }
                }
            }
            
            // 列出新路径中的所有技能文件
            if (Directory.Exists(newPath))
            {
                string[] newFiles = Directory.GetFiles(newPath, "SkillData_*.asset");
                if (newFiles.Length > 0)
                {
                    Debug.Log($"✓ 找到 {newFiles.Length} 个正确格式的技能配置文件:");
                    foreach (var file in newFiles)
                    {
                        Debug.Log($"  ✓ {Path.GetFileName(file)}");
                    }
                }
                else
                {
                    Debug.LogWarning("⚠ 未找到任何技能配置文件！");
                    Debug.LogWarning("请使用技能编辑器创建技能配置。");
                }
            }
            
            Debug.Log("=== 检查完成 ===");
        }
        
        [MenuItem("Tools/技能系统/创建默认技能配置文件夹")]
        public static void CreateDefaultSkillDataFolder()
        {
            string path = Application.dataPath + "/Resources/Skills/SkillData";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
                Debug.Log($"✓ 已创建技能数据文件夹: {path}");
            }
            else
            {
                Debug.Log($"文件夹已存在: {path}");
            }
        }
    }
}
#endif

