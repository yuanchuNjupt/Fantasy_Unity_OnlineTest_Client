using System;
using System.Collections.Generic;
using System.Linq;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIFramework.Core
{
    public class UIManager : Singleton<UIManager>, IUIManager
    {
        private Canvas _rootCanvas;

        private readonly Dictionary<string, Canvas> _canvasLayerDict;

        private readonly Dictionary<string, UIPanelInfo> _panelDict;

        private readonly Dictionary<string, List<UIPanelInfo>> _activePanelDict;

        public event Action<string, IUIPanel> OnPanelAdded;

        public event Action<string, IUIPanel> OnPanelRemoved;

        public event Action<string, IUIPanel> OnPanelShown;

        public event Action<string, IUIPanel> OnPanelHidden;

        public event Action<string, IUIPanel, UIPanelState, UIPanelState> OnPanelStateChanged;

        public event Action<string, Canvas> OnLayerAdded;

        public event Action<string, Canvas> OnLayerRemoved;

        public UIManager()
        {
            IsInitialized = false;
            _canvasLayerDict = new Dictionary<string, Canvas>();
            _panelDict = new Dictionary<string, UIPanelInfo>();
            _activePanelDict = new Dictionary<string, List<UIPanelInfo>>();
        }

        public Canvas RootCanvas
        {
            get
            {
                if (_rootCanvas != null)
                {
                    return _rootCanvas;
                }

                GameObject uiRoot = new GameObject("UIRoot");
                Canvas canvas = uiRoot.AddComponent<Canvas>();
                uiRoot.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 0;
                _rootCanvas = canvas;

                return _rootCanvas;
            }
        }

        public bool IsInitialized { get; private set; }

        public bool Initialize(Canvas rootCanvas)
        {
            if (IsInitialized)
            {
                return true;
            }

            if (rootCanvas == null)
            {
                throw new NullReferenceException("根Canvas不能为空");
            }

            _rootCanvas = rootCanvas;
            IsInitialized = true;
            return true;
        }

        #region 层级管理

        public int LayerCount => _canvasLayerDict.Count;

        public void AddLayer(string layerName, int sortingOrder)
        {
            // 如果层级已存在，则不重复添加层级
            if (_canvasLayerDict.ContainsKey(layerName))
            {
                Debug.LogWarning($"Canvas layer: {layerName} already exists");
                return;
            }

            // 创建Canvas对象并设置到根节点下
            GameObject canvasObject = new GameObject(layerName);
            canvasObject.transform.SetParent(RootCanvas.transform);

            // 添加Canvas组件并设置渲染顺序
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
            canvas.vertexColorAlwaysGammaSpace = true;
            canvasObject.AddComponent<GraphicRaycaster>();

            // 设置Canvas的RectTransform属性
            RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            // 记录层级信息
            _canvasLayerDict.Add(layerName, canvas);
            // 初始化该层级的面板列表
            _activePanelDict.Add(layerName, new List<UIPanelInfo>());

            // 触发层级添加事件
            OnLayerAdded?.Invoke(layerName, canvas);
        }

        public void AddLayer(Canvas canvas)
        {
            AddLayer(canvas, canvas.gameObject.name);
        }

        public void AddLayer(Canvas canvas, string layerName)
        {
            // 如果canvas为空，则不添加层级
            if (canvas == null)
            {
                Debug.LogWarning(this + " canvas is null");
                return;
            }

            // 如果层级已存在，则不重复添加层级
            if (_canvasLayerDict.ContainsKey(layerName))
            {
                Debug.LogWarning($"Canvas layer: {layerName} already exists");
                return;
            }

            // 将canvas设置为根节点的子节点
            canvas.transform.SetParent(RootCanvas.transform);
            _canvasLayerDict.Add(layerName, canvas);
            _activePanelDict.Add(layerName, new List<UIPanelInfo>());

            // 触发层级添加事件
            OnLayerAdded?.Invoke(layerName, canvas);
        }

        public void RemoveLayer(Canvas canvas)
        {
            RemoveLayer(canvas.gameObject.name);
        }

        public void RemoveLayer(string layerName)
        {
            if (!_canvasLayerDict.TryGetValue(layerName, out Canvas canvas))
            {
                Debug.LogWarning($"Canvas layer: {layerName} not found");
                return;
            }

            // 先移除该层级的所有面板
            HideAllPanel(layerName);

            // 清理字典
            _canvasLayerDict.Remove(layerName);
            _activePanelDict.Remove(layerName);

            // 触发层级移除事件
            OnLayerRemoved?.Invoke(layerName, canvas);

            // 销毁Canvas对象
            Object.Destroy(canvas.gameObject);
        }

        public Canvas GetLayer(string layerName)
        {
            if (_canvasLayerDict.TryGetValue(layerName, out Canvas canvas))
            {
                return canvas;
            }

            Debug.LogWarning($"未找到 {layerName} 层级，获取层级失败");
            return null;
        }

        public bool HasLayer(string layerName)
        {
            return _canvasLayerDict.ContainsKey(layerName);
        }

        #endregion

        #region 面板管理

        public T AddPanel<T>(GameObject prefab, string layer) where T : class, IUIPanel
        {
            return AddPanel<T>(prefab, layer, typeof(T).Name);
        }

        public T AddPanel<T>(GameObject prefab, string layer, string key) where T : class, IUIPanel
        {
            // 检查层级是否存在
            if (!HasLayer(layer))
            {
                Debug.LogError($"Layer: {layer} 不存在，无法添加面板");
                return null;
            }

            GameObject panelObj = Object.Instantiate(prefab);

            if (!panelObj.TryGetComponent(out T panel))
            {
                Debug.LogWarning($"{prefab.name} 对象不存在{typeof(T).Name}组件, 创建面板失败");
                Object.Destroy(panelObj);
                return null;
            }

            AddPanel(panel, layer, key);
            return panel;
        }

        public void AddPanel(IUIPanel panel, string layer)
        {
            AddPanel(panel, layer, string.Empty);
        }

        public void AddPanel(IUIPanel panel, string layer, string key)
        {
            // 检查层级是否存在
            if (!HasLayer(layer))
            {
                Debug.LogError($"Layer: {layer} 不存在，无法添加面板");
                return;
            }

            string keyName = string.IsNullOrEmpty(key) ? panel.GetType().Name : key;
            if (_panelDict.ContainsKey(keyName))
            {
                Debug.LogWarning($"已存在相同key的面板 {keyName}");
                return;
            }

            UIPanelInfo panelInfo = new UIPanelInfo(panel, layer, keyName);
            _panelDict.Add(keyName, panelInfo);
            Transform layerRoot = GetLayer(layer).transform;
            panel.Initialize(layerRoot);

            // 触发面板添加事件
            OnPanelAdded?.Invoke(keyName, panel);
        }

        public bool HasPanel<T>() where T : class, IUIPanel
        {
            return HasPanel(typeof(T).Name);
        }

        public bool HasPanel(IUIPanel panel)
        {
            return HasPanel(panel.GetType().Name);
        }

        public bool HasPanel(string key)
        {
            return _panelDict.ContainsKey(key);
        }

        public T GetPanel<T>(string key) where T : class, IUIPanel
        {
            return GetPanel(key) as T;
        }

        public T GetPanel<T>() where T : class, IUIPanel
        {
            return GetPanel<T>(typeof(T).Name);
        }

        public IUIPanel GetPanel(string key)
        {
            if (_panelDict.TryGetValue(key, out var panelInfo))
            {
                return panelInfo.panel;
            }

            Debug.LogWarning($"Panel: {key} not found");
            return null;
        }

        public IUIPanel[] GetActivePanels(string layer)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                return null;
            }

            if (layerPanelList.Count == 0)
            {
                return null;
            }

            return layerPanelList.Select(panel => panel.panel).ToArray();
        }

        public IUIPanel[] GetAllActivePanels()
        {
            List<IUIPanel> list = new List<IUIPanel>();
            foreach (var layer in _activePanelDict.Keys)
            {
                list.AddRange(_activePanelDict[layer].Select(panel => panel.panel));
            }

            return list.ToArray();
        }

        public IUIPanel[] GetAllPanels()
        {
            List<IUIPanel> list = new List<IUIPanel>();
            foreach (var panelDictValue in _panelDict.Values)
            {
                list.Add(panelDictValue.panel);
            }

            return list.ToArray();
        }

        public string[] GetActivePanelKeys(string layer)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                return null;
            }

            if (layerPanelList.Count == 0)
            {
                return null;
            }

            return layerPanelList.Select(panel => panel.key).ToArray();
        }

        public string[] GetAllPanelKeys()
        {
            return _panelDict.Keys.ToArray();
        }

        public int GetActivePanelCount(string layer)
        {
            return _activePanelDict.TryGetValue(layer, out var list) ? list.Count : 0;
        }

        public int GetAllPanelCount()
        {
            return _panelDict.Count;
        }

        public UIPanelInfo[] GetActivePanelInfos(string layer)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                return null;
            }

            if (layerPanelList.Count == 0)
            {
                return null;
            }

            return layerPanelList.ToArray();
        }

        public UIPanelInfo[] GetAllPanelInfos()
        {
            return _panelDict.Values.ToArray();
        }

        public void RemovePanel<T>() where T : class, IUIPanel
        {
            RemovePanel(typeof(T).Name);
        }

        public void RemovePanel(IUIPanel panel)
        {
            RemovePanel(panel.GetType().Name);
        }

        public void RemovePanel(string key)
        {
            if (!_panelDict.Remove(key, out var panelInfo))
            {
                Debug.LogWarning($"移除面板失败: 未找到面板 {key}");
                return;
            }

            // 检查状态转换是否合法
            if (!panelInfo.CanTransitionTo(UIPanelState.Destroyed))
            {
                Debug.LogWarning($"面板 {key} 无法从 {panelInfo.state} 状态转换到 Destroyed 状态");
                return;
            }

            // 更新状态
            var oldState = panelInfo.state;
            panelInfo.UpdateState(UIPanelState.Destroyed);

            // 从激活列表中移除
            _activePanelDict[panelInfo.layer].Remove(panelInfo);

            // 调用面板移除方法
            panelInfo.panel.Destroyed();

            // 触发事件
            OnPanelStateChanged?.Invoke(key, panelInfo.panel, oldState, UIPanelState.Destroyed);
            OnPanelRemoved?.Invoke(key, panelInfo.panel);
        }

        public void ClearPanels(string layer)
        {
            // 使用ToList()创建副本，避免并发修改异常
            foreach (var panelInfo in _panelDict.Values.ToList())
            {
                if (panelInfo.layer == layer)
                {
                    RemovePanel(panelInfo.key);
                }
            }
        }

        public void ClearAllPanels()
        {
            // 使用ToList()创建副本，避免并发修改异常
            foreach (var panelInfo in _panelDict.Values.ToList())
            {
                RemovePanel(panelInfo.key);
            }
        }

        public T ShowPanel<T>(string key, object arg = null) where T : class, IUIPanel
        {
            string keyName = string.IsNullOrEmpty(key) ? typeof(T).Name : key;
            return ShowPanel(keyName, arg) as T;
        }

        public T ShowPanel<T>(object arg = null) where T : class, IUIPanel
        {
            return ShowPanel(typeof(T).Name, arg) as T;
        }

        public IUIPanel ShowPanel(string key, object arg = null)
        {
            if (!_panelDict.TryGetValue(key, out var panelInfo))
            {
                Debug.LogWarning($"显示面板失败: 未找到面板 {key}");
                return null;
            }

            // 检查状态转换是否合法
            if (!panelInfo.CanTransitionTo(UIPanelState.Shown))
            {
                Debug.LogWarning($"面板 {key} 无法从 {panelInfo.state} 状态转换到 Shown 状态");
                return null;
            }

            var layerList = _activePanelDict[panelInfo.layer];

            // 如果面板已经在激活列表中，先移除它（避免重复）
            layerList.Remove(panelInfo);
            // 添加到列表末尾（最顶层显示）
            layerList.Add(panelInfo);

            // 更新状态
            var oldState = panelInfo.state;
            if (panelInfo.UpdateState(UIPanelState.Shown))
            {
                OnPanelStateChanged?.Invoke(key, panelInfo.panel, oldState, UIPanelState.Shown);
            }

            panelInfo.panel.Show(arg);

            // 触发面板显示事件
            OnPanelShown?.Invoke(key, panelInfo.panel);

            return panelInfo.panel;
        }

        public void HidePanel<T>(object arg = null) where T : class, IUIPanel
        {
            HidePanel(typeof(T).Name, arg);
        }

        public void HidePanel(IUIPanel panel, object arg = null)
        {
            HidePanel(panel.GetType().Name, arg);
        }

        public void HidePanel(string key, object arg = null)
        {
            if (!_panelDict.TryGetValue(key, out var panelInfo))
            {
                Debug.LogWarning($"隐藏面板失败: 未找到面板 {key}");
                return;
            }

            // 检查状态转换是否合法
            if (!panelInfo.CanTransitionTo(UIPanelState.Hidden))
            {
                Debug.LogWarning($"面板 {key} 无法从 {panelInfo.state} 状态转换到 Hidden 状态");
                return;
            }

            // 从激活列表中移除
            _activePanelDict[panelInfo.layer].Remove(panelInfo);

            // 更新状态
            var oldState = panelInfo.state;
            if (panelInfo.UpdateState(UIPanelState.Hidden))
            {
                OnPanelStateChanged?.Invoke(key, panelInfo.panel, oldState, UIPanelState.Hidden);
            }

            panelInfo.panel.Hide(arg);

            // 触发面板隐藏事件
            OnPanelHidden?.Invoke(key, panelInfo.panel);
        }

        public void HideTopPanel(string layer, object arg = null)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                Debug.LogError($"不存在Layer: {layer}, 隐藏顶部面板失败");
                return;
            }

            if (layerPanelList.Count == 0)
            {
                Debug.LogWarning($"{layer} 层中没有Panel");
                return;
            }

            var topPanel = layerPanelList[^1];
            layerPanelList.Remove(topPanel);

            // 更新状态
            var oldState = topPanel.state;
            if (topPanel.UpdateState(UIPanelState.Hidden))
            {
                OnPanelStateChanged?.Invoke(topPanel.key, topPanel.panel, oldState, UIPanelState.Hidden);
            }

            topPanel.panel.Hide(arg);

            // 触发面板隐藏事件
            OnPanelHidden?.Invoke(topPanel.key, topPanel.panel);
        }

        public void HideTopPanels(string layer, int count, object arg = null)
        {
            for (var i = 0; i < count; i++)
            {
                HideTopPanel(layer, arg);
            }
        }

        public void HideAllPanel(string layer, object arg = null)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                Debug.LogError($"不存在Layer: {layer}, 隐藏顶部面板失败");
                return;
            }

            if (layerPanelList.Count == 0)
            {
                Debug.LogWarning($"{layer} 层中没有Panel");
                return;
            }

            // 使用ToList()创建副本，避免并发修改异常
            var panelsToHide = layerPanelList.ToList();
            for (var i = panelsToHide.Count - 1; i >= 0; i--)
            {
                var topPanel = panelsToHide[i];
                layerPanelList.Remove(topPanel);

                // 更新状态
                var oldState = topPanel.state;
                if (topPanel.UpdateState(UIPanelState.Hidden))
                {
                    OnPanelStateChanged?.Invoke(topPanel.key, topPanel.panel, oldState, UIPanelState.Hidden);
                }

                topPanel.panel.Hide(arg);

                // 触发面板隐藏事件
                OnPanelHidden?.Invoke(topPanel.key, topPanel.panel);
            }
        }

        public void HideAllLayersPanels(object arg = null)
        {
            // 使用ToList()创建副本，避免并发修改异常
            var layersToPop = _activePanelDict.Keys.ToList();
            foreach (var layer in layersToPop)
            {
                HideAllPanel(layer, arg);
            }
        }

        #endregion

        #region 状态管理

        /// <summary>
        /// 获取面板当前状态
        /// </summary>
        /// <param name="key">面板Key</param>
        /// <returns>面板状态</returns>
        public UIPanelState GetPanelState(string key)
        {
            if (_panelDict.TryGetValue(key, out var panelInfo))
            {
                return panelInfo.state;
            }

            Debug.LogWarning($"Panel: {key} not found");
            return UIPanelState.Destroyed;
        }

        /// <summary>
        /// 获取面板当前状态
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <returns>面板状态</returns>
        public UIPanelState GetPanelState<T>() where T : class, IUIPanel
        {
            return GetPanelState(typeof(T).Name);
        }

        /// <summary>
        /// 检查面板是否可以转换到指定状态
        /// </summary>
        /// <param name="key">面板Key</param>
        /// <param name="targetState">目标状态</param>
        /// <returns>是否可以转换</returns>
        public bool CanPanelTransitionTo(string key, UIPanelState targetState)
        {
            if (_panelDict.TryGetValue(key, out var panelInfo))
            {
                return panelInfo.CanTransitionTo(targetState);
            }

            return false;
        }

        /// <summary>
        /// 获取指定状态的所有面板
        /// </summary>
        /// <param name="state">面板状态</param>
        /// <returns>面板数组</returns>
        public IUIPanel[] GetPanelsByState(UIPanelState state)
        {
            return _panelDict.Values
                .Where(info => info.state == state)
                .Select(info => info.panel)
                .ToArray();
        }

        /// <summary>
        /// 获取指定层级中指定状态的面板
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <param name="state">面板状态</param>
        /// <returns>面板数组</returns>
        public IUIPanel[] GetPanelsByState(string layer, UIPanelState state)
        {
            if (!_activePanelDict.TryGetValue(layer, out var layerPanelList))
            {
                return new IUIPanel[0];
            }

            return layerPanelList
                .Where(info => info.state == state)
                .Select(info => info.panel)
                .ToArray();
        }

        /// <summary>
        /// 获取各状态的面板统计信息
        /// </summary>
        /// <returns>状态统计字典</returns>
        public Dictionary<UIPanelState, int> GetPanelStateStatistics()
        {
            var statistics = new Dictionary<UIPanelState, int>();

            foreach (UIPanelState state in Enum.GetValues(typeof(UIPanelState)))
            {
                statistics[state] = 0;
            }

            foreach (var panelInfo in _panelDict.Values)
            {
                statistics[panelInfo.state]++;
            }

            return statistics;
            
        }

        #endregion
    }
}