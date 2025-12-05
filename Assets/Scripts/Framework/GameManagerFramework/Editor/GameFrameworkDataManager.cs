using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework.GameManagerFramework.Editor
{
    public class GameFrameworkDataManager
    {
#if UNITY_EDITOR
        private static GameManagerData _dataAsset;
        private const string DataAssetPath = "Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset";
        
        // 初始化数据资产
        private static GameManagerData DataAsset
        {
            get
            {
                if (_dataAsset == null)
                {
                //     _dataAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>(DataAssetPath);
                //     
                //     // 如果资产不存在，创建一个新的
                //     if (_dataAsset == null)
                //     {
                //         _dataAsset = ScriptableObject.CreateInstance<GameManagerData>();
                //         
                //         // 确保目录存在
                //         string directory = System.IO.Path.GetDirectoryName(DataAssetPath);
                //         if (!System.IO.Directory.Exists(directory))
                //         {
                //             System.IO.Directory.CreateDirectory(directory);
                //         }
                //         
                //         AssetDatabase.CreateAsset(_dataAsset, DataAssetPath);
                //         AssetDatabase.SaveAssets();
                //     }
                }
                return _dataAsset;
            }
        }
        
        // World 列表
        public static List<string> Worlds
        {
            get => DataAsset.worlds;
        }
        
        // Logic Managers 列表
        public static List<(string name, string world)> LogicManagers
        {
            get => DataAsset.logicManagers.Select(x => (x.name, x.world)).ToList();
        }
        
        // Data Managers 列表
        public static List<(string name, string world)> DataManagers
        {
            get => DataAsset.dataManagers.Select(x => (x.name, x.world)).ToList();
        }
        
        // Message Managers 列表
        public static List<(string name, string world)> MessageManagers
        {
            get => DataAsset.messageManagers.Select(x => (x.name, x.world)).ToList();
        }
        
        // 保存数据
        public static void SaveData()
        {
            EditorUtility.SetDirty(DataAsset);
            AssetDatabase.SaveAssets();
        }

        public static readonly string WorldGeneratePath = Application.dataPath + "/Scripts/Framework/GameManagerFramework/WorldScripts/";

        public static readonly string LogicManagerGeneratePath = Application.dataPath + "/Scripts/Framework/GameManagerFramework/LogicManagers/";

        public static readonly string DataManagerGeneratePath = Application.dataPath + "/Scripts/Framework/GameManagerFramework/DataManagers/";
        
        public static readonly string MessageManagerGeneratePath = Application.dataPath + "/Scripts/Framework/GameManagerFramework/MessageManagers/";
        
#endif
    }
}