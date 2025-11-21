namespace UIFramework.Core
{
    /// <summary>
    /// 面板实例的详细信息类
    /// </summary>
    public class UIPanelInfo
    {
        /// <summary>
        /// 面板实例
        /// </summary>
        public IUIPanel panel;
        
        /// <summary>
        /// 面板所在层级
        /// </summary>
        public string layer;
        
        /// <summary>
        /// 面板的唯一标识
        /// </summary>
        public string key;
        
        /// <summary>
        /// 面板当前状态
        /// </summary>
        public UIPanelState state;

        public UIPanelInfo(IUIPanel panel, string layer)
        {
            this.panel = panel;
            this.layer = layer;
            this.state = UIPanelState.Hidden;
        }

        public UIPanelInfo(IUIPanel panel, string layer, string key) : this(panel, layer)
        {
            this.key = key;
        }

        /// <summary>
        /// 更新面板状态
        /// </summary>
        /// <param name="newState">新状态</param>
        /// <returns>状态是否发生改变</returns>
        public bool UpdateState(UIPanelState newState)
        {
            if (state == newState)
                return false;

            state = newState;
            return true;
        }

        /// <summary>
        /// 检查状态转换是否合法
        /// </summary>
        /// <param name="targetState">目标状态</param>
        /// <returns>转换是否合法</returns>
        public bool CanTransitionTo(UIPanelState targetState)
        {
            return targetState switch
            {
                UIPanelState.Hidden => state != UIPanelState.Destroyed,
                UIPanelState.Shown => state != UIPanelState.Destroyed,
                UIPanelState.Destroyed => true,
                _ => false
            };
        }
    }
}