using UnityEngine;

namespace UIFramework.Core
{
    public static class UIManagerExtensions
    {
         /// <summary>
        /// 通过枚举值移除对应的UI层级
        /// </summary>
        /// <param name="self">UI管理器实例</param>
        /// <param name="layer">要移除的层级枚举值</param>
        /// <remarks>
        /// 实际调用RemoveLayer(string)方法，转换枚举值为字符串
        /// 如果对应层级不存在会触发警告
        /// </remarks>
        public static void RemoveLayer(this IUIManager self, UILayer layer)
        {
            self.RemoveLayer(layer.ToString());
        }

        /// <summary>
        /// 获取指定枚举值对应的UI层级Canvas
        /// </summary>
        /// <param name="self">UI管理器实例</param>
        /// <param name="layer">要获取的层级枚举值</param>
        /// <returns>找到返回对应Canvas对象，否则返回null</returns>
        /// <remarks>
        /// 当层级不存在时会触发警告日志
        /// </remarks>
        public static Canvas GetLayer(this IUIManager self, UILayer layer)
        {
            return self.GetLayer(layer.ToString());
        }

        /// <summary>
        /// 检查是否存在指定枚举值的UI层级
        /// </summary>
        /// <param name="self">UI管理器实例</param>
        /// <param name="layer">要检查的层级枚举值</param>
        /// <returns>存在返回true，否则false</returns>
        public static bool HasLayer(this IUIManager self, UILayer layer)
        {
            return self.HasLayer(layer.ToString());
        }

        /// <summary>
        /// 添加面板到指定枚举层级（无标识）
        /// </summary>
        /// <typeparam name="T">面板组件类型，必须实现IUIPanel接口</typeparam>
        /// <param name="self">UI管理器实例</param>
        /// <param name="panel">要添加的面板实例</param>
        /// <param name="layer">目标层级枚举值</param>
        public static void AddPanel<T>(this IUIManager self, T panel, UILayer layer) where T : class, IUIPanel
        {
            self.AddPanel(panel, layer.ToString(), string.Empty);
        }

        /// <summary>
        /// 添加面板到指定枚举层级（带标识）
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="self">UI管理器实例</param>
        /// <param name="panel">要添加的面板实例</param>
        /// <param name="layer">目标层级枚举值</param>
        /// <param name="key">面板唯一标识字符串</param>
        /// <remarks>
        /// 标识规则与基础方法一致，格式为"TypeName_key"
        /// 会覆盖相同key的面板
        /// </remarks>
        public static void AddPanel<T>(this IUIManager self, T panel, UILayer layer, string key) where T : class, IUIPanel
        {
            self.AddPanel(panel, layer.ToString(), key);
        }

        /// <summary>
        /// 实例化并添加新面板到指定枚举层级
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="self">UI管理器实例</param>
        /// <param name="prefab">面板预制体</param>
        /// <param name="layer">目标层级枚举值</param>
        /// <returns>成功返回实例化的面板，失败返回null</returns>
        /// <exception cref="MissingComponentException">
        /// 当预制体缺少对应组件时触发警告
        /// </exception>
        public static T AddPanel<T>(this IUIManager self, GameObject prefab, UILayer layer) where T : class, IUIPanel
        {
            return self.AddPanel<T>(prefab, layer.ToString(), string.Empty);
        }

        /// <summary>
        /// 实例化并添加新面板到指定枚举层级（带标识）
        /// </summary>
        /// <typeparam name="T">面板组件类型</typeparam>
        /// <param name="self">UI管理器实例</param>
        /// <param name="prefab">面板预制体</param>
        /// <param name="layer">目标层级枚举值</param>
        /// <param name="key">面板唯一标识字符串</param>
        /// <returns>成功返回实例化的面板，失败返回null</returns>
        /// <remarks>
        /// 自动将预制体实例化到对应层级的Canvas下
        /// </remarks>
        public static T AddPanel<T>(this IUIManager self, GameObject prefab, UILayer layer, string key) where T : class, IUIPanel
        {
            return self.AddPanel<T>(prefab, layer.ToString(), key);
        }

        /// <summary>
        /// 关闭指定枚举层级的最顶部面板
        /// </summary>
        /// <param name="self">UI管理器实例</param>
        /// <param name="layer">目标层级枚举值</param>
        /// <remarks>
        /// 如果目标层级没有活动面板会触发警告
        /// 实际调用HideTopPanel(string)方法
        /// </remarks>
        public static void HideTopPanel(this IUIManager self, UILayer layer)
        {
            self.HideTopPanel(layer.ToString());
        }

        public static void ClearPanels(this IUIManager self, UILayer layer)
        {
            self.ClearPanels(layer.ToString());
        }
    }
}