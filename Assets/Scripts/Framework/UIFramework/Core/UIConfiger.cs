using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using GGG.Tool.Singleton;

namespace UIFramework.Core
{
    
    /// <summary>
    /// UI系统配置器
    /// 提供UI管理器的初始化配置和编辑器工具
    /// </summary>
    [System.Serializable]
    public class LayerConfig
    {
        [LabelText("层级名称")]
        public string layerName;

        [LabelText("排序顺序")]
        [Tooltip("数值越大显示越靠前")]
        public int sortingOrder;

        [LabelText("是否默认创建")]
        public bool createOnStart = true;
    }
    
    [System.Serializable]
    public class PanelConfig
    {
        [LabelText("面板预制体")]
        // [AssetsOnly, AssetSelector(Paths = "Resources")]
        [AssetsOnly]
        public GameObject panelPrefab;

        [LabelText("所属层级")]
        // [ValueDropdown(nameof(GetAvailableLayerNames))]
        public string targetLayer;

        [LabelText("面板Key")]
        [Tooltip("留空则使用类型名")]
        public string panelKey;

        [LabelText("启动时添加")]
        [Tooltip("如果为true，则会在游戏启动时添加面板到层级中")]
        public bool addOnStart = false;

        [LabelText("启动时显示")]
        [Tooltip("如果为true，则会在游戏启动时显示面板")]
        public bool showOnStart = false;
        
    }
    public class UIConfiger : Singleton<UIConfiger>
    {
        #region 基础配置

        [TitleGroup("基础配置")]
        [LabelText("UI根节点")]
        [Required("必须指定UI根节点")]
        [SerializeField]
        private Canvas rootCanvas;

        [TabGroup("基础配置/配置", "层级配置")]
        [LabelText("默认层级配置")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "layerName")]
        [SerializeField]
        private List<LayerConfig> defaultLayers = new List<LayerConfig>
        {
            new LayerConfig { layerName = "Background", sortingOrder = 0, createOnStart = true },
            new LayerConfig { layerName = "Main", sortingOrder = 100, createOnStart = true },
            new LayerConfig { layerName = "Popup", sortingOrder = 200, createOnStart = true },
            new LayerConfig { layerName = "Loading", sortingOrder = 300, createOnStart = true },
            new LayerConfig { layerName = "Tips", sortingOrder = 400, createOnStart = true },
            new LayerConfig { layerName = "Top", sortingOrder = 9999, createOnStart = true }
        };

        [TabGroup("基础配置/配置", "面板配置")]
        [LabelText("默认面板配置")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "panelKey")]
        [SerializeField]
        private List<PanelConfig> defaultPanels = new List<PanelConfig>();

        /// <summary>
        /// 获取默认层级配置列表（只读）
        /// </summary>
        // public IReadOnlyList<LayerConfig> DefaultLayers => defaultLayers;

        [TitleGroup("基础配置")]
        [Button("验证配置")]
        [GUIColor(0.6f, 1f, 0.6f)]
        private void ValidateConfiguration()
        {
            bool isValid = true;

            if (rootCanvas == null)
            {
                Debug.LogError("根Canvas未配置！");
                isValid = false;
            }

            // 验证层级配置
            var layerNames = new HashSet<string>();
            foreach (var layer in defaultLayers)
            {
                if (string.IsNullOrEmpty(layer.layerName))
                {
                    Debug.LogError("存在空的层级名称！");
                    isValid = false;
                }
                else if (layerNames.Contains(layer.layerName))
                {
                    Debug.LogError($"重复的层级名称：{layer.layerName}");
                    isValid = false;
                }
                else
                {
                    layerNames.Add(layer.layerName);
                }
            }

            // 验证面板配置
            foreach (var panel in defaultPanels)
            {
                if (panel.panelPrefab == null)
                {
                    Debug.LogError($"面板预制体为空：{panel.panelKey}");
                    isValid = false;
                }
                else if (panel.panelPrefab.GetComponent<IUIPanel>() == null)
                {
                    Debug.LogError($"预制体缺少IUIPanel组件：{panel.panelPrefab.name}");
                    isValid = false;
                }

                if (string.IsNullOrEmpty(panel.targetLayer))
                {
                    Debug.LogError($"面板目标层级为空：{panel.panelKey}");
                    isValid = false;
                }
                else if (!layerNames.Contains(panel.targetLayer))
                {
                    Debug.LogWarning($"面板目标层级不在默认层级中：{panel.targetLayer}");
                }
            }

            if (isValid)
            {
                Debug.Log("配置验证通过！");
            }
            else
            {
                Debug.LogError("配置验证失败，请检查错误信息！");
            }
        }

        #endregion
        
        #region 初始化选项

        [TitleGroup("初始化选项")]

        [BoxGroup("初始化选项/清理设置", centerLabel: true)]
        [LabelText("清空现有层级")]
        [Tooltip("是否清空当前RootCanvas中的所有Layer")]
        [SerializeField]
        private bool isClearLayer;

        [BoxGroup("初始化选项/清理设置", centerLabel: true)]
        [LabelText("清空现有面板")]
        [Tooltip("是否在游戏启动时清空Layer中的Panel")]
        [SerializeField]
        private bool isClearPanel;

        [BoxGroup("初始化选项/自动配置", centerLabel: true)]
        [LabelText("自动添加层级")]
        [Tooltip("是否基于当前RootCanvas对象创建Layer")]
        [SerializeField]
        private bool isAddLayer;

        [BoxGroup("初始化选项/自动配置", centerLabel: true)]
        [LabelText("自动添加面板")]
        [Tooltip("是否在游戏启动时添加Layer中的Panel")]
        [SerializeField]
        private bool isAddPanel;

        [BoxGroup("初始化选项/自动配置", centerLabel: true)]
        [LabelText("创建默认层级")]
        [Tooltip("是否创建配置中的默认层级")]
        [SerializeField]
        private bool createDefaultLayers = true;

        [BoxGroup("初始化选项/自动配置", centerLabel: true)]
        [LabelText("添加默认面板")]
        [Tooltip("是否添加配置中的默认面板")]
        [SerializeField]
        private bool addDefaultPanels = true;

        #endregion
        
        #region 调试信息
        


        [TitleGroup("调试信息")]
        [ShowInInspector, ReadOnly]
        [LabelText("UI管理器状态")]
        private string UIManagerStatus => _uiManager?.IsInitialized == true ? "已初始化" : "未初始化";

        [ShowInInspector, ReadOnly]
        [LabelText("层级数量")]
        private int LayerCount => _uiManager?.LayerCount ?? 0;

        [ShowInInspector, ReadOnly]
        [LabelText("面板总数")]
        private int PanelCount => _uiManager?.GetAllPanelCount() ?? 0;

        [ShowInInspector, ReadOnly]
        [LabelText("活跃面板数")]
        private int ActivePanelCount => _uiManager?.GetAllActivePanels()?.Length ?? 0;

        #endregion

        #region 初始化方法

        private IUIManager _uiManager;

        protected override void Awake()
        {
            base.Awake();
            InitializeUIManager();

            if (_uiManager == null || !_uiManager.IsInitialized)
            {
                Debug.LogError("UI管理器初始化失败！");
                return;
            }

            // 执行清理操作
            if (isClearLayer) ClearExistingLayers();
            if (isClearPanel) ClearExistingPanels();

            // 执行配置操作
            if (createDefaultLayers) CreateDefaultLayers();
            if (isAddLayer) AddExistingLayers();
            if (addDefaultPanels) AddDefaultPanels();
            if (isAddPanel) AddExistingPanels();

            Debug.Log($"UI系统初始化完成！层级数：{_uiManager.LayerCount}，面板数：{_uiManager.GetAllPanelCount()}");
        }
        
        
        private void InitializeUIManager()
        {

            _uiManager = UIManager.MainInstance;

            if (rootCanvas == null)
            {
                Debug.LogError("Root Canvas 未配置！初始化失败");
                return;
            }

            _uiManager.Initialize(rootCanvas);
        }
        
        private void ClearExistingLayers()
        {
            for (int i = rootCanvas.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(rootCanvas.transform.GetChild(i).gameObject);
            }

            Debug.Log("已清空RootCanvas中所有Layer");
        }

        private void ClearExistingPanels()
        {
            for (int i = 0; i < rootCanvas.transform.childCount; i++)
            {
                GameObject layer = rootCanvas.transform.GetChild(i).gameObject;
                for (int j = layer.transform.childCount - 1; j >= 0; j--)
                {
                    GameObject panelObj = layer.transform.GetChild(j).gameObject;
                    if (panelObj.TryGetComponent(out IUIPanel panel))
                    {
                        DestroyImmediate(panelObj);
                    }
                }
            }

            Debug.Log("已清空RootCanvas下所有Layer中的Panel");
        }
        
        private void CreateDefaultLayers()
        {
            foreach (var layerConfig in defaultLayers.Where(l => l.createOnStart))
            {
                if (!_uiManager.HasLayer(layerConfig.layerName))
                {
                    _uiManager.AddLayer(layerConfig.layerName, layerConfig.sortingOrder);
                    Debug.Log($"创建默认层级：{layerConfig.layerName} (排序：{layerConfig.sortingOrder})");
                }
            }
        }

        private void AddExistingLayers()
        {
            for (int i = 0; i < rootCanvas.transform.childCount; i++)
            {
                GameObject layerObj = rootCanvas.transform.GetChild(i).gameObject;
                if (layerObj.TryGetComponent(out Canvas canvas))
                {
                    if (!_uiManager.HasLayer(layerObj.name))
                    {
                        _uiManager.AddLayer(canvas);
                        Debug.Log($"添加现有层级：{layerObj.name}");
                    }
                }
            }
        }

        private void AddDefaultPanels()
        {
            foreach (var panelConfig in defaultPanels.Where(p => p.addOnStart))
            {
                if (panelConfig.panelPrefab == null || string.IsNullOrEmpty(panelConfig.targetLayer))
                {
                    Debug.LogWarning($"面板配置不完整：{panelConfig.panelKey}");
                    continue;
                }

                try
                {
                    var panelComponent = panelConfig.panelPrefab.GetComponent<IUIPanel>();
                    if (panelComponent == null)
                    {
                        Debug.LogWarning($"预制体 {panelConfig.panelPrefab.name} 没有实现 IUIPanel 接口");
                        continue;
                    }

                    string key = string.IsNullOrEmpty(panelConfig.panelKey) ? panelComponent.GetType().Name : panelConfig.panelKey;

                    if (!_uiManager.HasPanel(key))
                    {
                        var panel = _uiManager.AddPanel<IUIPanel>(panelConfig.panelPrefab, panelConfig.targetLayer, key);
                        if (panel != null)
                        {
                            Debug.Log($"添加默认面板：{key} 到层级 {panelConfig.targetLayer}");

                            if (panelConfig.showOnStart)
                            {
                                _uiManager.ShowPanel(key);
                                Debug.Log($"显示默认面板：{key}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"添加默认面板失败：{panelConfig.panelKey}，错误：{ex.Message}");
                }
            }
        }

        private void AddExistingPanels()
        {
            for (int i = 0; i < rootCanvas.transform.childCount; i++)
            {
                GameObject layer = rootCanvas.transform.GetChild(i).gameObject;
                for (int j = 0; j < layer.transform.childCount; j++)
                {
                    GameObject panelObj = layer.transform.GetChild(j).gameObject;
                    if (panelObj.TryGetComponent(out IUIPanel panel))
                    {
                        string key = panel.GetType().Name;
                        if (!_uiManager.HasPanel(key))
                        {
                            _uiManager.AddPanel(panel, layer.name);
                            Debug.Log($"添加现有面板：{key} 到层级 {layer.name}");
                        }
                    }
                }
            }
        }
        
        
        

        #endregion
        
        
        #region 面板管理工具

        [TitleGroup("面板管理")]
        [LabelText("目标面板Key")]
        [SerializeField]
        private string targetPanelKey;

        [TitleGroup("面板管理")]
        [LabelText("显示参数")]
        [SerializeField]
        private string showArgument;

        [HorizontalGroup("面板管理/面板操作")]
        [Button("显示面板")]
        [EnableIf("@!string.IsNullOrEmpty(targetPanelKey)")]
        private void ShowTargetPanel()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetPanelKey))
            {
                Debug.LogWarning("UI管理器未初始化或面板Key为空");
                return;
            }

            var panel = _uiManager.ShowPanel(targetPanelKey, showArgument);
            if (panel != null)
            {
                Debug.Log($"显示面板：{targetPanelKey}，参数：{showArgument}");
            }
            else
            {
                Debug.LogWarning($"显示面板失败：{targetPanelKey}");
            }
        }

        [HorizontalGroup("面板管理/面板操作")]
        [Button("隐藏面板")]
        [EnableIf("@!string.IsNullOrEmpty(targetPanelKey)")]
        private void HideTargetPanel()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetPanelKey))
            {
                Debug.LogWarning("UI管理器未初始化或面板Key为空");
                return;
            }

            _uiManager.HidePanel(targetPanelKey, showArgument);
            Debug.Log($"隐藏面板：{targetPanelKey}");
        }

        [HorizontalGroup("面板管理/面板操作")]
        [Button("移除面板")]
        [EnableIf("@!string.IsNullOrEmpty(targetPanelKey)")]
        [GUIColor(1f, 0.6f, 0.6f)]
        private void RemoveTargetPanel()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetPanelKey))
            {
                Debug.LogWarning("UI管理器未初始化或面板Key为空");
                return;
            }

            _uiManager.RemovePanel(targetPanelKey);
            Debug.Log($"移除面板：{targetPanelKey}");
        }

        #endregion

        #region 层级管理工具

        [TitleGroup("层级管理")]
        [LabelText("目标层级名")]
        [SerializeField]
        private string targetLayerName;

        [TitleGroup("层级管理")]
        [LabelText("排序顺序")]
        [SerializeField]
        private int targetSortingOrder = 100;

        [HorizontalGroup("层级管理/层级操作")]
        [Button("创建层级")]
        [EnableIf("@!string.IsNullOrEmpty(targetLayerName)")]
        private void CreateTargetLayer()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetLayerName))
            {
                Debug.LogWarning("UI管理器未初始化或层级名为空");
                return;
            }

            if (_uiManager.HasLayer(targetLayerName))
            {
                Debug.LogWarning($"层级已存在：{targetLayerName}");
                return;
            }

            _uiManager.AddLayer(targetLayerName, targetSortingOrder);
            Debug.Log($"创建层级：{targetLayerName}，排序：{targetSortingOrder}");
        }

        [HorizontalGroup("层级管理/层级操作")]
        [Button("弹出所有面板")]
        [EnableIf("@!string.IsNullOrEmpty(targetLayerName)")]
        private void PopAllPanelsInLayer()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetLayerName))
            {
                Debug.LogWarning("UI管理器未初始化或层级名为空");
                return;
            }

            _uiManager.HideAllPanel(targetLayerName);
            Debug.Log($"弹出层级 {targetLayerName} 中的所有面板");
        }

        [HorizontalGroup("层级管理/层级操作")]
        [Button("移除层级")]
        [EnableIf("@!string.IsNullOrEmpty(targetLayerName)")]
        [GUIColor(1f, 0.6f, 0.6f)]
        private void RemoveTargetLayer()
        {
            if (_uiManager == null || string.IsNullOrEmpty(targetLayerName))
            {
                Debug.LogWarning("UI管理器未初始化或层级名为空");
                return;
            }

            _uiManager.RemoveLayer(targetLayerName);
            Debug.Log($"移除层级：{targetLayerName}");
        }

        #endregion

        #region 编辑器工具按钮

        [TitleGroup("编辑器工具")]

        [HorizontalGroup("编辑器工具/统计按钮")]
        [Button("统计所有面板")]
        private void DebugAllPanels()
        {
            if (_uiManager == null)
            {
                Debug.LogWarning("UI管理器未初始化");
                return;
            }

            var panels = _uiManager.GetAllPanels();
            Debug.Log($"=== 所有面板信息 (共{panels.Length}个) ===");
            foreach (var panel in panels)
            {
                var state = _uiManager.GetPanelState(panel.GetType().Name);
                Debug.Log($"面板：{panel.GetType().Name}，状态：{state}，GameObject：{panel.GetType().Name}");
            }
        }

        [HorizontalGroup("编辑器工具/统计按钮")]
        [Button("统计所有层级")]
        private void DebugAllLayers()
        {
            if (_uiManager == null)
            {
                Debug.LogWarning("UI管理器未初始化");
                return;
            }

            Debug.Log($"=== 所有层级信息 (共{_uiManager.LayerCount}个) ===");
            foreach (var layerConfig in defaultLayers)
            {
                bool exists = _uiManager.HasLayer(layerConfig.layerName);
                var canvas = exists ? _uiManager.GetLayer(layerConfig.layerName) : null;
                int activePanels = exists ? _uiManager.GetActivePanelCount(layerConfig.layerName) : 0;

                Debug.Log($"层级：{layerConfig.layerName}，存在：{exists}，排序：{canvas?.sortingOrder ?? -1}，活跃面板：{activePanels}");
            }
        }

        [HorizontalGroup("编辑器工具/统计按钮")]
        [Button("统计面板状态")]
        private void ShowStateStatistics()
        {
            if (_uiManager == null)
            {
                Debug.LogWarning("UI管理器未初始化");
                return;
            }

            var statistics = _uiManager.GetPanelStateStatistics();
            Debug.Log("=== 面板状态统计 ===");
            foreach (var kvp in statistics)
            {
                Debug.Log($"{kvp.Key}：{kvp.Value} 个面板");
            }
        }

        [Button("清理所有面板", ButtonSizes.Medium)]
        [GUIColor(1f, 0.6f, 0.6f)]
        private void ClearAllPanels()
        {
            if (_uiManager == null)
            {
                Debug.LogWarning("UI管理器未初始化");
                return;
            }

            _uiManager.ClearAllPanels();
            Debug.Log("已清理所有面板");
        }

        #endregion
        
        
        
        
        
        
        
    }
}