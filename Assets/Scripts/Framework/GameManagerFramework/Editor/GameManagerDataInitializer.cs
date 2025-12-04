using UnityEditor;
using UnityEngine;

namespace Framework.GameManagerFramework.Editor
{
    /// <summary>
    /// 初始化 GameManager 数据资产的工具
    /// </summary>
    public static class GameManagerDataInitializer
    {
        private const string DataAssetPath = "Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset";
        
        [MenuItem("Framework/GameManager/Initialize Data Asset")]
        public static void InitializeDataAsset()
        {
            var existingAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>(DataAssetPath);
            
            if (existingAsset != null)
            {
                Debug.Log("GameManagerData 资产已存在: " + DataAssetPath);
                Selection.activeObject = existingAsset;
                EditorGUIUtility.PingObject(existingAsset);
                return;
            }
            
            // 创建目录（如果不存在）
            string directory = System.IO.Path.GetDirectoryName(DataAssetPath);
            if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            
            // 创建新的数据资产
            var dataAsset = ScriptableObject.CreateInstance<GameManagerData>();
            AssetDatabase.CreateAsset(dataAsset, DataAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Selection.activeObject = dataAsset;
            EditorGUIUtility.PingObject(dataAsset);
            
            Debug.Log("GameManagerData 资产已创建: " + DataAssetPath);
        }
        
        [MenuItem("Framework/GameManager/Clear All Data")]
        public static void ClearAllData()
        {
            if (EditorUtility.DisplayDialog("清空所有数据", 
                "确定要清空所有 World 和 Manager 数据吗？此操作不可撤销！", 
                "确定", "取消"))
            {
                var dataAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>(DataAssetPath);
                if (dataAsset != null)
                {
                    dataAsset.worlds.Clear();
                    dataAsset.logicManagers.Clear();
                    dataAsset.dataManagers.Clear();
                    dataAsset.messageManagers.Clear();
                    
                    EditorUtility.SetDirty(dataAsset);
                    AssetDatabase.SaveAssets();
                    
                    Debug.Log("所有数据已清空");
                }
                else
                {
                    Debug.LogWarning("未找到 GameManagerData 资产");
                }
            }
        }
    }
}

