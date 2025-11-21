using UnityEngine;

namespace UIFramework.Core
{
    /// <summary>
    /// UI面板接口
    /// </summary>
    public interface IUIPanel
    {
        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        bool IsInitialized { get; }
        
        /// <summary>
        /// 是否可见
        /// </summary>
        bool IsVisible { get; }
        
        /// <summary>
        /// 初始化面板
        /// 在面板添加到UIManager之后会被调用，用于设置层级关系和初始化面板
        /// </summary>
        /// <param name="layerRoot">所属层级的根节点</param>
        void Initialize(Transform layerRoot);
        
        /// <summary>
        /// 面板显示
        /// </summary>
        void Show(object arg);
        
        /// <summary>
        /// 面板隐藏
        /// </summary>
        void Hide(object arg);
        
        /// <summary>
        /// 面板销毁
        /// </summary>
        void Destroyed();
    }
}