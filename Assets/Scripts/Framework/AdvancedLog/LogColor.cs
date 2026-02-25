using System;

namespace Framework.AdvancedLog
{
    /// <summary>
    /// 日志颜色枚举
    /// </summary>
    [Flags]
    public enum LogColor
    {
        /// <summary>默认颜色</summary>
        Default = 1,

        /// <summary>白色</summary>
        White = 2,

        /// <summary>黑色</summary>
        Black = 4,

        /// <summary>红色</summary>
        Red = 8,

        /// <summary>橙色</summary>
        Orange = 16,

        /// <summary>黄色</summary>
        Yellow = 32,

        /// <summary>绿色</summary>
        Green = 64,

        /// <summary>青色</summary>
        Cyan = 128,

        /// <summary>蓝色</summary>
        Blue = 256,

        /// <summary>紫色</summary>
        Purple = 512,
    }
}