using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
namespace Framework.GameManagerFramework.Editor
{
    [HideMonoScript]
    [Serializable]
    public class CreateModelConfig
    {
        #region 创建区域
        
        [VerticalGroup("Container")]
        [HorizontalGroup("Container/Create")]
        [VerticalGroup("Container/Create/World")]
        [BoxGroup("Container/Create/World/Box", ShowLabel = true, CenterLabel = true, LabelText = "World")]
        [LabelText("World名称")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string worldName;
        
        [BoxGroup("Container/Create/World/Box")]
        [Button(ButtonSizes.Large, Name = "创建 World")]
        [GUIColor(0.7f, 0.9f, 1f)]
        [PropertySpace(10)]
        public void CreateWorld()
        {
            if (GameFrameworkDataManager.Worlds.Contains(worldName))
            {
                EditorUtility.DisplayDialog("创建 World 失败", worldName + "已存在！", "确定");
                return;
            }
            
            if(string.IsNullOrEmpty(worldName))
                return;
            
            //创建一个World
            GameManagerGenerator.GenerateWorld(worldName);
            //加入到列表
            GameFrameworkDataManager.Worlds.Add(worldName);
            
        }
        
        [HorizontalGroup("Container/Create")]
        [VerticalGroup("Container/Create/Logic")]
        [BoxGroup("Container/Create/Logic/Box", ShowLabel = true, CenterLabel = true, LabelText = "Logic")]
        [LabelText("Logic名称")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string logicLabel;

        [BoxGroup("Container/Create/Logic/Box")] 
        [LabelText("从属世界")]
        [ValueDropdown("GetWorldList")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string logicSourceWorld;   
        
        
        [BoxGroup("Container/Create/Logic/Box")]
        [Button(ButtonSizes.Large, Name = "创建 Logic")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [PropertySpace(10)]
        public void CreateLogic()
        {
            for (int i = 0; i < GameFrameworkDataManager.LogicManagers.Count; i++)
            {
                if (GameFrameworkDataManager.LogicManagers[i].name == logicLabel)
                {
                    EditorUtility.DisplayDialog("创建 LogicManager 失败", logicLabel + "已存在！", "确定");
                    return;
                }
            }
  
            
            if (string.IsNullOrEmpty(logicLabel))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(logicSourceWorld))
            {
                return;
            }
            
            
            // 创建 Logic Manager
            GameManagerGenerator.GenerateManager(logicLabel , logicSourceWorld , GameManagerGenerator.GenerateType.LogicManager);            
            
            // 加入到列表（名称，世界）
            var dataAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>("Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset");
            if (dataAsset != null)
            {
                dataAsset.logicManagers.Add(new ManagerInfo(logicLabel, logicSourceWorld));
                GameFrameworkDataManager.SaveData();
            }
            
            EditorUtility.DisplayDialog("创建成功", $"Logic Manager '{logicLabel}' 已创建在世界 '{logicSourceWorld}' 下", "确定");
        }
        
        [HorizontalGroup("Container/Create")]
        [VerticalGroup("Container/Create/Data")]
        [BoxGroup("Container/Create/Data/Box", ShowLabel = true, CenterLabel = true, LabelText = "Data")]
        [LabelText("Data名称")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string dataName;
        
        [BoxGroup("Container/Create/Data/Box")]
        [LabelText("从属世界")]
        [ValueDropdown("GetWorldList")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string dataSourceWorld;
        
        [BoxGroup("Container/Create/Data/Box")]
        [Button(ButtonSizes.Large, Name = "创建 Data")]
        [GUIColor(1f, 0.9f, 0.7f)]
        [PropertySpace(10)]
        public void CreateData()
        {
            if (string.IsNullOrEmpty(dataName))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(dataSourceWorld))
            {
                return;
            }
            
            // 创建 Data Manager
            GameManagerGenerator.GenerateManager(dataName , dataSourceWorld , GameManagerGenerator.GenerateType.DataManager);            
            // 加入到列表（名称，世界）
            var dataAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>("Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset");
            if (dataAsset != null)
            {
                dataAsset.dataManagers.Add(new ManagerInfo(dataName, dataSourceWorld));
                GameFrameworkDataManager.SaveData();
            }
            
            EditorUtility.DisplayDialog("创建成功", $"Data Manager '{dataName}' 已创建在世界 '{dataSourceWorld}' 下", "确定");
        }
        
        [HorizontalGroup("Container/Create")]
        [VerticalGroup("Container/Create/Message")]
        [BoxGroup("Container/Create/Message/Box", ShowLabel = true, CenterLabel = true, LabelText = "Message")]
        [LabelText("Message名称")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string messageName;
        
        [BoxGroup("Container/Create/Message/Box")]
        [LabelText("从属世界")]
        [ValueDropdown("GetWorldList")]
        [LabelWidth(0)]
        [PropertySpace(10)]
        public string messageSourceWorld;
        
        [BoxGroup("Container/Create/Message/Box")]
        [Button(ButtonSizes.Large, Name = "创建 Message")]
        [GUIColor(1f, 0.7f, 0.9f)]
        [PropertySpace(10)]
        public void CreateMessage()
        {
            if (string.IsNullOrEmpty(messageName))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(messageSourceWorld))
            {
                return;
            }
            

            
            // 创建 Message Manager
            GameManagerGenerator.GenerateManager(messageName , messageSourceWorld , GameManagerGenerator.GenerateType.MessageManager);
            // 加入到列表（名称，世界）
            var dataAsset = AssetDatabase.LoadAssetAtPath<GameManagerData>("Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset");
            if (dataAsset != null)
            {
                dataAsset.messageManagers.Add(new ManagerInfo(messageName, messageSourceWorld));
                GameFrameworkDataManager.SaveData();
            }
            
            EditorUtility.DisplayDialog("创建成功", $"Message Manager '{messageName}' 已创建在世界 '{messageSourceWorld}' 下", "确定");
        }
        
        #endregion
        
        #region 管理区域
        
        [VerticalGroup("Container")]
        [HorizontalGroup("Container/Manage")]
        [VerticalGroup("Container/Manage/World")]
        [BoxGroup("Container/Manage/World/Box", ShowLabel = true, CenterLabel = true, LabelText = "World 列表")]
        [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false, DraggableItems = false)]
        [PropertySpace(10)]
        public List<string> worldList = GameFrameworkDataManager.Worlds;
        
        [HorizontalGroup("Container/Manage")]
        [VerticalGroup("Container/Manage/Logic")]
        [BoxGroup("Container/Manage/Logic/Box", ShowLabel = true, CenterLabel = true, LabelText = "Logic 列表")]
        [LabelText("筛选世界")]
        [ValueDropdown("GetWorldListWithAll")]
        [PropertySpace(5)]
        [OnValueChanged("RefreshLogicList")]
        public string logicFilterWorld = "全部";
        
        [BoxGroup("Container/Manage/Logic/Box")]
        [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false, DraggableItems = true, ShowItemCount = true)]
        [PropertySpace(10)]
        [ShowInInspector]
        [HideLabel]
        public List<string> FilteredLogicList
        {
            get
            {
                if (logicFilterWorld == "全部")
                    return GameFrameworkDataManager.LogicManagers.ConvertAll(x => $"{x.name} ({x.world})");
                return GameFrameworkDataManager.LogicManagers
                    .FindAll(x => x.world == logicFilterWorld)
                    .ConvertAll(x => $"{x.name} ({x.world})");
            }
        }
        
        [HorizontalGroup("Container/Manage")]
        [VerticalGroup("Container/Manage/Data")]
        [BoxGroup("Container/Manage/Data/Box", ShowLabel = true, CenterLabel = true, LabelText = "Data 列表")]
        [LabelText("筛选世界")]
        [ValueDropdown("GetWorldListWithAll")]
        [PropertySpace(5)]
        [OnValueChanged("RefreshDataList")]
        public string dataFilterWorld = "全部";
        
        [BoxGroup("Container/Manage/Data/Box")]
        [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false, DraggableItems = true, ShowItemCount = true)]
        [PropertySpace(10)]
        [ShowInInspector]
        [HideLabel]
        public List<string> FilteredDataList
        {
            get
            {
                if (dataFilterWorld == "全部")
                    return GameFrameworkDataManager.DataManagers.ConvertAll(x => $"{x.name} ({x.world})");
                return GameFrameworkDataManager.DataManagers
                    .FindAll(x => x.world == dataFilterWorld)
                    .ConvertAll(x => $"{x.name} ({x.world})");
            }
        }
        
        [HorizontalGroup("Container/Manage")]
        [VerticalGroup("Container/Manage/Message")]
        [BoxGroup("Container/Manage/Message/Box", ShowLabel = true, CenterLabel = true, LabelText = "Message 列表")]
        [LabelText("筛选世界")]
        [ValueDropdown("GetWorldListWithAll")]
        [PropertySpace(5)]
        [OnValueChanged("RefreshMessageList")]
        public string messageFilterWorld = "全部";
        
        [BoxGroup("Container/Manage/Message/Box")]
        [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false, DraggableItems = true, ShowItemCount = true)]
        [PropertySpace(10)]
        [ShowInInspector]
        [HideLabel]
        public List<string> FilteredMessageList
        {
            get
            {
                if (messageFilterWorld == "全部")
                    return GameFrameworkDataManager.MessageManagers.ConvertAll(x => $"{x.name} ({x.world})");
                return GameFrameworkDataManager.MessageManagers
                    .FindAll(x => x.world == messageFilterWorld)
                    .ConvertAll(x => $"{x.name} ({x.world})");
            }
        }
        
        #endregion
        
        #region 辅助方法
        
        // 提供 World 列表供下拉选择
        private IEnumerable<string> GetWorldList()
        {
            return GameFrameworkDataManager.Worlds;
        }
        
        // 提供带"全部"选项的 World 列表
        private IEnumerable<string> GetWorldListWithAll()
        {
            var list = new List<string> { "全部" };
            list.AddRange(GameFrameworkDataManager.Worlds);
            return list;
        }
        
        // 刷新列表（用于触发界面更新）
        private void RefreshLogicList() { }
        private void RefreshDataList() { }
        private void RefreshMessageList() { }
        
        #endregion
    }
}