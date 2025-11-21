using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework.Core
{
    /// <summary>
    /// UI管理器接口
    /// 负责管理UI面板的层级、显示、隐藏、状态等功能
    /// 提供完整的UI系统管理能力
    /// </summary>
    public interface IUIManager
    {
        #region 主要属性
        /// <summary>
        /// 获取UI系统的根Canvas对象
        /// </summary>
        /// <remarks>
        /// 如果尚未初始化，会自动创建默认UIRoot对象
        /// 默认配置：ScreenSpace - Overlay - 0
        /// </remarks>
        Canvas RootCanvas { get; }

        /// <summary>
        /// 获取UI系统是否已经初始化完成
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 获取当前UI层级的数量
        /// </summary>
        int LayerCount { get; }
        #endregion

        #region 事件回调
        /// <summary>
        /// 面板添加到UI系统时触发的事件
        /// </summary>
        event Action<string, IUIPanel> OnPanelAdded;

        /// <summary>
        /// 面板从UI系统移除时触发的事件
        /// </summary>
        event Action<string, IUIPanel> OnPanelRemoved;

        /// <summary>
        /// 面板显示时触发的事件
        /// </summary>
        event Action<string, IUIPanel> OnPanelShown;

        /// <summary>
        /// 面板隐藏时触发的事件
        /// </summary>
        event Action<string, IUIPanel> OnPanelHidden;

        /// <summary>
        /// 面板状态改变时触发的事件
        /// 参数依次为 Guid 面板实例 旧状态 新状态
        /// </summary>
        event Action<string, IUIPanel, UIPanelState, UIPanelState> OnPanelStateChanged;

        /// <summary>
        /// UI层级添加时触发的事件
        /// 参数依次为 层级名称 层级Canvas对象
        /// </summary>
        event Action<string, Canvas> OnLayerAdded;

        /// <summary>
        /// UI层级移除时触发的事件
        /// </summary>
        event Action<string, Canvas> OnLayerRemoved;
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化UI管理系统
        /// </summary>
        /// <param name="rootCanvas">根Canvas对象，不能为空</param>
        /// <returns>初始化是否成功</returns>
        /// <exception cref="NullReferenceException">当rootCanvas为空时抛出</exception>
        bool Initialize(Canvas rootCanvas);
        #endregion

        #region 层级管理
        /// <summary>
        /// 创建新的UI层级
        /// </summary>
        /// <param name="layerName">层级名称（必须唯一）</param>
        /// <param name="sortingOrder">渲染排序值（数值越大显示在越上层）</param>
        /// <remarks>
        /// 会自动创建GameObject并添加Canvas和GraphicRaycaster组件
        /// 设置为根Canvas的子对象，配置为覆盖排序模式
        /// </remarks>
        void AddLayer(string layerName, int sortingOrder);

        /// <summary>
        /// 添加已存在的Canvas作为UI层级
        /// </summary>
        /// <param name="canvas">要添加的Canvas对象</param>
        /// <remarks>
        /// 使用Canvas的游戏对象名称作为层级名称
        /// Canvas会被设置为根Canvas的子对象
        /// </remarks>
        void AddLayer(Canvas canvas);

        /// <summary>
        /// 添加已存在的Canvas作为UI层级
        /// </summary>
        /// <param name="canvas">要添加的Canvas对象</param>
        /// <param name="layerName">自定义层级名称</param>
        /// <remarks>
        /// Canvas会被设置为根Canvas的子对象
        /// 如果层级名称已存在，会输出警告并忽略操作
        /// </remarks>
        void AddLayer(Canvas canvas, string layerName);

        /// <summary>
        /// 移除指定Canvas对应的UI层级
        /// </summary>
        /// <param name="canvas">要移除的Canvas对象</param>
        /// <remarks>
        /// 使用Canvas的游戏对象名称查找对应层级
        /// </remarks>
        void RemoveLayer(Canvas canvas);

        /// <summary>
        /// 移除指定名称的UI层级
        /// </summary>
        /// <param name="layerName">要移除的层级名称</param>
        /// <remarks>
        /// 会先弹出该层级下的所有面板，然后销毁Canvas GameObject
        /// 触发OnLayerRemoved事件
        /// </remarks>
        void RemoveLayer(string layerName);

        /// <summary>
        /// 获取指定名称的UI层级Canvas
        /// </summary>
        /// <param name="layerName">要获取的层级名称</param>
        /// <returns>找到的Canvas对象，未找到返回null</returns>
        Canvas GetLayer(string layerName);

        /// <summary>
        /// 检查是否存在指定名称的UI层级
        /// </summary>
        /// <param name="layerName">要检查的层级名称</param>
        /// <returns>存在返回true，否则false</returns>
        bool HasLayer(string layerName);
        #endregion

        #region 面板管理
        /// <summary>
        /// 通过预制体实例化并添加UI面板到指定层级
        /// </summary>
        /// <typeparam name="T">面板组件类型，必须实现IUIPanel接口</typeparam>
        /// <param name="prefab">面板预制体</param>
        /// <param name="layer">目标层级名称</param>
        /// <returns>成功返回面板实例，失败返回null</returns>
        /// <remarks>
        /// 使用类型名作为面板的唯一标识
        /// 如果预制体缺少T类型组件，会销毁实例并返回null
        /// </remarks>
        T AddPanel<T>(GameObject prefab, string layer) where T : class, IUIPanel;

        /// <summary>
        /// 通过预制体实例化并添加UI面板到指定层级（带唯一标识）
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="prefab">面板预制体</param>
        /// <param name="layer">目标层级名称</param>
        /// <param name="key">面板唯一标识</param>
        /// <returns>成功返回面板实例，失败返回null</returns>
        /// <remarks>
        /// 如果key为空，使用类型名作为标识
        /// 相同key的面板会被覆盖
        /// </remarks>
        T AddPanel<T>(GameObject prefab, string layer, string key) where T : class, IUIPanel;

        /// <summary>
        /// 直接添加UI面板实例到指定层级
        /// </summary>
        /// <param name="panel">面板实例</param>
        /// <param name="layer">目标层级名称</param>
        /// <remarks>
        /// 使用面板类型名作为唯一标识
        /// 会调用面板的Initialize方法进行初始化
        /// </remarks>
        void AddPanel(IUIPanel panel, string layer);

        /// <summary>
        /// 直接添加UI面板实例到指定层级（带唯一标识）
        /// </summary>
        /// <param name="panel">面板实例</param>
        /// <param name="layer">目标层级名称</param>
        /// <param name="key">面板唯一标识</param>
        /// <remarks>
        /// 如果key为空，使用面板类型名作为标识
        /// 相同key的面板会输出警告并忽略操作
        /// </remarks>
        void AddPanel(IUIPanel panel, string layer, string key);

        /// <summary>
        /// 检查是否存在指定类型的面板
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <returns>存在返回true，否则false</returns>
        bool HasPanel<T>() where T : class, IUIPanel;

        /// <summary>
        /// 检查是否存在指定面板实例
        /// </summary>
        /// <param name="panel">面板实例</param>
        /// <returns>存在返回true，否则false</returns>
        bool HasPanel(IUIPanel panel);

        /// <summary>
        /// 检查是否存在指定标识的面板
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <returns>存在返回true，否则false</returns>
        bool HasPanel(string key);

        /// <summary>
        /// 获取指定类型的面板实例
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <returns>找到返回实例，否则返回null</returns>
        T GetPanel<T>() where T : class, IUIPanel;

        /// <summary>
        /// 获取指定标识的面板实例（泛型版本）
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="key">面板唯一标识</param>
        /// <returns>找到返回实例，否则返回null</returns>
        T GetPanel<T>(string key) where T : class, IUIPanel;

        /// <summary>
        /// 获取指定标识的面板实例
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <returns>找到返回实例，否则返回null</returns>
        IUIPanel GetPanel(string key);

        /// <summary>
        /// 获取指定层级中所有活跃的面板
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <returns>面板数组，如果层级不存在或无面板返回null</returns>
        IUIPanel[] GetActivePanels(string layer);

        /// <summary>
        /// 获取所有层级中的活跃面板
        /// </summary>
        /// <returns>面板数组</returns>
        IUIPanel[] GetAllActivePanels();

        /// <summary>
        /// 获取UI系统中的所有面板（包括隐藏的）
        /// </summary>
        /// <returns>面板数组</returns>
        IUIPanel[] GetAllPanels();

        /// <summary>
        /// 获取指定层级中所有活跃面板的标识列表
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <returns>标识数组，如果层级不存在或无面板返回null</returns>
        string[] GetActivePanelKeys(string layer);

        /// <summary>
        /// 获取UI系统中所有面板的标识列表
        /// </summary>
        /// <returns>标识数组</returns>
        string[] GetAllPanelKeys();

        /// <summary>
        /// 获取指定层级中活跃面板的数量
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <returns>面板数量，层级不存在返回0</returns>
        int GetActivePanelCount(string layer);

        /// <summary>
        /// 获取UI系统中所有面板的数量
        /// </summary>
        /// <returns>面板总数量</returns>
        int GetAllPanelCount();

        /// <summary>
        /// 获取指定层级中所有活跃面板的详细信息
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <returns>面板信息数组，如果层级不存在或无面板返回null</returns>
        UIPanelInfo[] GetActivePanelInfos(string layer);

        /// <summary>
        /// 获取UI系统中所有面板的详细信息
        /// </summary>
        /// <returns>面板信息数组</returns>
        UIPanelInfo[] GetAllPanelInfos();

        /// <summary>
        /// 移除指定类型的面板
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <remarks>
        /// 会调用面板的OnDestroy方法并从UI系统中移除
        /// 触发OnPanelRemoved和OnPanelStateChanged事件
        /// </remarks>
        void RemovePanel<T>() where T : class, IUIPanel;

        /// <summary>
        /// 移除指定面板实例
        /// </summary>
        /// <param name="panel">面板实例</param>
        void RemovePanel(IUIPanel panel);

        /// <summary>
        /// 移除指定标识的面板
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <remarks>
        /// 会检查状态转换是否合法，不合法时输出警告
        /// </remarks>
        void RemovePanel(string key);

        /// <summary>
        /// 清理指定层级中的所有面板
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <remarks>
        /// 会移除并销毁该层级下的所有面板
        /// 对每个面板触发相应的事件
        /// </remarks>
        void ClearPanels(string layer);

        /// <summary>
        /// 清理UI系统中的所有面板
        /// </summary>
        /// <remarks>
        /// 会清理所有层级中的所有面板
        /// </remarks>
        void ClearAllPanels();
        #endregion

        #region Panel Display Control
        /// <summary>
        /// 显示指定类型的面板
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="arg">显示面板时传入的参数</param>
        /// <returns>成功返回面板实例，失败返回null</returns>
        /// <remarks>
        /// 面板会被添加到所属层级的顶部显示
        /// 触发OnPanelShown和OnPanelStateChanged事件
        /// </remarks>
        T ShowPanel<T>(object arg = null) where T : class, IUIPanel;

        /// <summary>
        /// 显示指定标识的面板（泛型版本）
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="key">面板唯一标识</param>
        /// <param name="arg">显示面板时传入的参数</param>
        /// <returns>成功返回面板实例，失败返回null</returns>
        T ShowPanel<T>(string key, object arg = null) where T : class, IUIPanel;

        /// <summary>
        /// 显示指定标识的面板
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <param name="arg">显示面板时传入的参数</param>
        /// <returns>成功返回面板实例，失败返回null</returns>
        /// <remarks>
        /// 会检查状态转换是否合法，不合法时输出警告并返回null
        /// </remarks>
        IUIPanel ShowPanel(string key, object arg = null);

        /// <summary>
        /// 隐藏指定类型的面板
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="arg">隐藏面板时传入的参数</param>
        /// <remarks>
        /// 面板会从活跃列表中移除但不会销毁
        /// 触发OnPanelHidden和OnPanelStateChanged事件
        /// </remarks>
        void HidePanel<T>(object arg = null) where T : class, IUIPanel;

        /// <summary>
        /// 隐藏指定面板实例
        /// </summary>
        /// <param name="panel">面板实例</param>
        /// <param name="arg">隐藏面板时传入的参数</param>
        void HidePanel(IUIPanel panel, object arg = null);

        /// <summary>
        /// 隐藏指定标识的面板
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <param name="arg">隐藏面板时传入的参数</param>
        void HidePanel(string key, object arg = null);

        /// <summary>
        /// 隐藏指定层级最顶部的面板
        /// </summary>
        /// <param name="layer">目标层级名称</param>
        /// <param name="arg">隐藏面板时传入的参数</param>
        /// <remarks>
        /// 类似栈操作，隐藏最后显示的面板
        /// 如果层级不存在或无面板会输出相应提示
        /// </remarks>
        void HideTopPanel(string layer, object arg = null);

        /// <summary>
        /// 隐藏指定层级的多个顶部面板
        /// </summary>
        /// <param name="layer">目标层级名称</param>
        /// <param name="count">要隐藏的面板数量</param>
        /// <param name="arg">隐藏面板时传入的参数</param>
        void HideTopPanels(string layer, int count, object arg = null);

        /// <summary>
        /// 隐藏指定层级的所有面板
        /// </summary>
        /// <param name="layer">目标层级名称</param>
        /// <param name="arg">隐藏面板时传入的参数</param>
        /// <remarks>
        /// 会隐藏该层级中的所有活跃面板
        /// 按照从顶部到底部的顺序依次隐藏
        /// </remarks>
        void HideAllPanel(string layer, object arg = null);

        /// <summary>
        /// 隐藏所有层级的所有面板
        /// </summary>
        /// <param name="arg">隐藏面板时传入的参数</param>
        /// <remarks>
        /// 会隐藏UI系统中的所有活跃面板
        /// </remarks>
        void HideAllLayersPanels(object arg = null);
        #endregion

        #region State Management
        /// <summary>
        /// 获取指定标识面板的当前状态
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <returns>面板状态，面板不存在返回Destroyed</returns>
        UIPanelState GetPanelState(string key);

        /// <summary>
        /// 获取指定类型面板的当前状态
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <returns>面板状态</returns>
        UIPanelState GetPanelState<T>() where T : class, IUIPanel;

        /// <summary>
        /// 检查指定面板是否可以转换到目标状态
        /// </summary>
        /// <param name="key">面板唯一标识</param>
        /// <param name="targetState">目标状态</param>
        /// <returns>是否可以转换，面板不存在返回false</returns>
        bool CanPanelTransitionTo(string key, UIPanelState targetState);

        /// <summary>
        /// 获取指定状态的所有面板
        /// </summary>
        /// <param name="state">面板状态</param>
        /// <returns>符合条件的面板数组</returns>
        IUIPanel[] GetPanelsByState(UIPanelState state);

        /// <summary>
        /// 获取指定层级中指定状态的面板
        /// </summary>
        /// <param name="layer">层级名称</param>
        /// <param name="state">面板状态</param>
        /// <returns>符合条件的面板数组，层级不存在返回空数组</returns>
        IUIPanel[] GetPanelsByState(string layer, UIPanelState state);

        /// <summary>
        /// 获取各状态面板的统计信息
        /// </summary>
        /// <returns>状态统计字典，键为状态，值为该状态的面板数量</returns>
        Dictionary<UIPanelState, int> GetPanelStateStatistics();
        #endregion
    }
}