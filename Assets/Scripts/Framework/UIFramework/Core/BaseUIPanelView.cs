using System;
using UIFramework.View;
using UnityEngine;
using UnityEngine.Events;

namespace UIFramework.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUIPanelView : BaseView , IUIPanel
    {

        [Header("初始状态设置")][SerializeField] protected bool setActiveOnShow = true;
        [SerializeField] protected bool setInactiveOnHide = true;

        #region 事件回调

        /// <summary>
        /// 面板初始化完成事件
        /// </summary>
        public UnityAction OnInitializedEvent { get; set; }

        /// <summary>
        /// 面板显示前事件
        /// </summary>
        public UnityAction<object> OnShowBefore { get; set; }

        /// <summary>
        /// 面板显示后事件
        /// </summary>
        public UnityAction<object> OnShowAfter { get; set; }

        /// <summary>
        /// 面板隐藏前事件
        /// </summary>
        public UnityAction<object> OnHideBefore { get; set; }

        /// <summary>
        /// 面板隐藏后事件
        /// </summary>
        public UnityAction<object> OnHideAfter { get; set; }

        /// <summary>
        /// 面板销毁前事件
        /// </summary>
        public UnityAction OnDestroyEvent { get; set; }

        #endregion

        #region 面板成员属性

        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private bool _isInitialized;
        private bool _isVisible;
        
        
        
        /// <summary>
        /// CanvasGroup组件
        /// </summary>
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                    if (_canvasGroup == null)
                    {
                        Debug.LogError($"面板 {name} 缺少 CanvasGroup 组件");
                    }
                }

                return _canvasGroup;
            }
        }
        
        /// <summary>
        /// RectTransform组件
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        
        /// <summary>
        /// 面板是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 面板是否可见
        /// </summary>
        public bool IsVisible => _isVisible;
        

        #endregion

        #region 接口实现

        void IUIPanel.Initialize(Transform layerRoot) => InitializePanel(layerRoot);

        void IUIPanel.Show(object arg) => ShowPanel(arg);

        void IUIPanel.Hide(object arg) => HidePanel(arg);

        void IUIPanel.Destroyed() => DestroyPanel();

        #endregion

        #region 核心方法
        
        
        /// <summary>
        /// 初始化面板
        /// </summary>
        /// <param name="layerRoot">层级根节点</param>
        protected virtual void InitializePanel(Transform layerRoot)
        {
            if (_isInitialized)
            {
                Debug.LogWarning($"面板 {name} 已经初始化过了");
                return;
            }
            
            if (layerRoot == null)
            {
                Debug.LogError($"面板 {name} 初始化失败：layerRoot 为空");
                return;
            }

            try
            {
                // 设置层级关系
                transform.SetParent(layerRoot, false);
                
                // 初始化RectTransform
                InitializeRectTransform();

                // 设置初始状态
                SetInitialState();

                // 标记为已初始化
                _isInitialized = true;

                // 触发初始化事件
                OnInitializedEvent?.Invoke();

                // 子类自定义初始化
                OnPanelInitialized();

                Debug.Log($"[{name}] 面板初始化完成");
                
                
            }
            catch (Exception e)
            {
                Debug.LogError($"面板 {name} 初始化失败：{e.Message}");
            }
        }
        
        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="arg">显示参数</param>
        protected virtual void ShowPanel(object arg)
        {
            if (!_isInitialized)
            {
                Debug.LogError($"面板 {name} 未初始化，无法显示");
                return;
            }

            OnShowBefore?.Invoke(arg);

            // 子类显示前处理
            OnPanelShowBefore(arg);

            if (setActiveOnShow && !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            _isVisible = true;
            transform.SetAsLastSibling();

            // 执行显示逻辑
            PerformShow(() =>
            {
                OnPanelShowAfter(arg);
                OnShowAfter?.Invoke(arg);
            });


        }
        
        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <param name="arg">隐藏参数</param>
        protected virtual void HidePanel(object arg)
        {
            if (!_isInitialized)
            {
                Debug.LogError($"面板 {name} 未初始化，无法隐藏");
                return;
            }

            try
            {
                OnHideBefore?.Invoke(arg);

                // 子类隐藏前处理
                OnPanelHideBefore(arg);

                _isVisible = false;

                // 执行隐藏逻辑
                PerformHide(() =>
                {
                    if (setInactiveOnHide)
                    {
                        gameObject.SetActive(false);
                    }

                    OnPanelHideAfter(arg);
                    OnHideAfter?.Invoke(arg);
                });
            }
            catch (Exception ex)
            {
                Debug.LogError($"面板 {name} 隐藏时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        protected virtual void DestroyPanel()
        {
            try
            {
                OnDestroyEvent?.Invoke();

                // 停止所有协程
                StopAllCoroutines();

                // 清理事件订阅
                ClearEventSubscriptions();

                // 子类自定义销毁处理
                OnPanelDestroy();

                Debug.Log($"[{name}] 面板销毁处理完成");
            }
            catch (Exception ex)
            {
                Debug.LogError($"面板 {name} 销毁时发生异常: {ex.Message}");
            }
        }
        
        
        

        #endregion


        #region 执行处理

        /// <summary>
        /// 执行显示逻辑（子类可重写以添加动画等效果）
        /// </summary>
        /// <param name="onComplete">完成回调</param>
        protected virtual void PerformShow(Action onComplete = null)
        {
            ShowInstantly();
            onComplete?.Invoke();
        }

        /// <summary>
        /// 执行隐藏逻辑（子类可重写以添加动画等效果）
        /// </summary>
        /// <param name="onComplete">完成回调</param>
        protected virtual void PerformHide(Action onComplete = null)
        {
            HideInstantly();
            onComplete?.Invoke();
        }

        /// <summary>
        /// 立即显示
        /// </summary>
        protected virtual void ShowInstantly()
        {
            if (CanvasGroup == null) return;

            CanvasGroup.alpha = 1f;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        /// <summary>
        /// 立即隐藏
        /// </summary>
        protected virtual void HideInstantly()
        {
            if (CanvasGroup == null) return;

            CanvasGroup.alpha = 0f;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
        }        

        #endregion
        
        

        #region 核心方法处理

        /// <summary>
        /// 设置初始状态
        /// </summary>
        protected virtual void SetInitialState()
        {
            if (CanvasGroup != null)
            {
                CanvasGroup.alpha = 0f;
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.interactable = false;
            }

            gameObject.SetActive(false);
            _isVisible = false;
        }
        
        /// <summary>
        /// 初始化RectTransform
        /// </summary>
        protected virtual void InitializeRectTransform()
        {
            if (RectTransform == null) return;

            RectTransform.anchoredPosition = Vector2.zero;
            RectTransform.localPosition = Vector3.zero;
            RectTransform.localScale = Vector3.one;
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;
        }
        
        /// <summary>
        /// 清理事件订阅
        /// </summary>
        protected virtual void ClearEventSubscriptions()
        {
            OnInitializedEvent = null;
            OnShowBefore = null;
            OnShowAfter = null;
            OnHideBefore = null;
            OnHideAfter = null;
            OnDestroyEvent = null;
        }

        #endregion

        #region 供子类重写的虚方法，用于自定义

        /// <summary>
        /// 面板初始化完成后调用（子类重写）
        /// </summary>
        protected virtual void OnPanelInitialized()
        {
        }

        /// <summary>
        /// 面板显示前调用（子类重写）
        /// </summary>
        /// <param name="arg">显示参数</param>
        protected virtual void OnPanelShowBefore(object arg)
        {
        }

        /// <summary>
        /// 面板显示后调用（子类重写）
        /// </summary>
        /// <param name="arg">显示参数</param>
        protected virtual void OnPanelShowAfter(object arg)
        {
        }

        /// <summary>
        /// 面板隐藏前调用（子类重写）
        /// </summary>
        /// <param name="arg">隐藏参数</param>
        protected virtual void OnPanelHideBefore(object arg)
        {
        }

        /// <summary>
        /// 面板隐藏后调用（子类重写）
        /// </summary>
        /// <param name="arg">隐藏参数</param>
        protected virtual void OnPanelHideAfter(object arg)
        {
        }

        /// <summary>
        /// 面板销毁时调用（子类重写）
        /// </summary>
        protected virtual void OnPanelDestroy()
        {
            Destroy(gameObject);
        }


        #endregion
        
        protected virtual void OnDestroy()
        {
            ClearEventSubscriptions();
        }

        
    }
}