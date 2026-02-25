using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace Framework.AdvancedLog
{
    /// <summary>
    /// 增强版日志工具类
    /// </summary>
    public static class Log
    {
        #region 配置选项

        /// <summary>是否启用日志</summary>
        public static bool IsEnableLog = true;

        /// <summary>是否启用Debug日志</summary>
        public static bool IsEnableDebug = true;

        /// <summary>是否启用Info日志</summary>
        public static bool IsEnableInfo = true;

        /// <summary>是否启用Warning日志</summary>
        public static bool IsEnableWarning = true;

        /// <summary>是否启用Error日志</summary>
        public static bool IsEnableError = true;

        /// <summary>是否显示时间戳</summary>
        public static bool ShowTimestamp = false;

        /// <summary>是否显示调用栈信息</summary>
        public static bool ShowStackTrace = false;

        /// <summary>是否输出到文件</summary>
        public static bool EnableFileOutput = false;

        /// <summary>最低日志级别</summary>
        public static LogLevel MinLogLevel = LogLevel.Debug;

        #endregion

        #region 私有字段

        private static readonly StringBuilder StringBuilderCache = new StringBuilder(512);

        private static readonly Dictionary<LogColor, string> ColorMap = new Dictionary<LogColor, string>
        {
            { LogColor.Default, "" },
            { LogColor.White, "#FFFFFF" },
            { LogColor.Black, "#000000" },
            { LogColor.Red, "#FF0000" },
            { LogColor.Orange, "#FFA500" },
            { LogColor.Yellow, "#FFFF00" },
            { LogColor.Green, "#00FF00" },
            { LogColor.Cyan, "#00FFFF" },
            { LogColor.Blue, "#4CB9FA" },
            { LogColor.Purple, "#FF00FF" }
        };

        #endregion

        #region Debug日志

        /// <summary>输出Debug日志（带颜色）</summary>
        public static void Debug(LogColor color, string title, string description, params string[] details)
        {
            if (!ShouldLog(LogLevel.Debug) || !IsEnableDebug) return;
            LogInternal(LogLevel.Debug, color, title, description, details);
        }

        /// <summary>输出Debug日志</summary>
        public static void Debug(string title, string description, params string[] details)
        {
            Debug(LogColor.Default, title, description, details);
        }

        /// <summary>简化的Debug日志</summary>
        public static void Debug(string message)
        {
            Debug(LogColor.Default, "Debug", message);
        }

        #endregion

        #region Info日志

        /// <summary>输出Info日志（带颜色）</summary>
        public static void Info(LogColor color, string title, string description, params string[] details)
        {
            if (!ShouldLog(LogLevel.Info) || !IsEnableInfo) return;
            LogInternal(LogLevel.Info, color, title, description, details);
        }

        /// <summary>输出Info日志</summary>
        public static void Info(string title, string description, params string[] details)
        {
            Info(LogColor.Green, title, description, details);
        }

        /// <summary>简化的Info日志</summary>
        public static void Info(string message)
        {
            Info(LogColor.Green, "Info", message);
        }

        public static void Info(LogColor color, string description)
        {
            Info(color, "" ,description);
        }
        

        #endregion

        #region Warning日志

        /// <summary>输出Warning日志（带颜色）</summary>
        public static void Warning(LogColor color, string title, string description, params string[] details)
        {
            if (!ShouldLog(LogLevel.Warning) || !IsEnableWarning) return;
            LogInternal(LogLevel.Warning, color, title, description, details);
        }

        /// <summary>输出Warning日志</summary>
        public static void Warning(string title, string description, params string[] details)
        {
            Warning(LogColor.Yellow, title, description, details);
        }

        /// <summary>简化的Warning日志</summary>
        public static void Warning(string message)
        {
            Warning(LogColor.Yellow, "Warning", message);
        }

        #endregion

        #region Error日志

        /// <summary>输出Error日志（带颜色）</summary>
        public static void Error(LogColor color, string title, string description, params string[] details)
        {
            if (!ShouldLog(LogLevel.Error) || !IsEnableError) return;
            LogInternal(LogLevel.Error, color, title, description, details);
        }

        /// <summary>输出Error日志</summary>
        public static void Error(string title, string description, params string[] details)
        {
            Error(LogColor.Red, title, description, details);
        }

        /// <summary>简化的Error日志</summary>
        public static void Error(string message)
        {
            Error(LogColor.Red, "Error", message);
        }

        /// <summary>输出异常日志</summary>
        public static void Error(Exception exception, string title = "Exception")
        {
            Error(LogColor.Red, title, exception.Message, exception.StackTrace);
        }

        #endregion

        #region 核心日志方法

        /// <summary>
        /// 核心日志输出方法
        /// </summary>
        private static void LogInternal(LogLevel level, LogColor color, string title, string description, params string[] details)
        {
            if (!IsEnableLog) return;

            try
            {
                StringBuilderCache.Clear();

                // 应用颜色
                string colorCode = "";
                bool hasColor = color != LogColor.Default && ColorMap.TryGetValue(color, out colorCode) && !string.IsNullOrEmpty(colorCode);

                // 标题颜色开始
                if (hasColor)
                {
                    StringBuilderCache.Append($"<color={colorCode}>");
                }

                // 添加时间戳
                if (ShowTimestamp)
                {
                    StringBuilderCache.Append($"[{DateTime.Now:HH:mm:ss.fff}] ");
                }

                // 添加日志级别
                StringBuilderCache.Append($"[{level}] ");


                // 添加标题
                StringBuilderCache.AppendLine($"[{title}]");

                // 添加描述
                if (!string.IsNullOrEmpty(description))
                {
                    if (hasColor)
                    {
                        StringBuilderCache.Append(description);
                        StringBuilderCache.AppendLine("</color>");
                    }
                    else
                    {
                        StringBuilderCache.AppendLine(description);
                    }
                }

                // 详细信息颜色开始
                if (hasColor)
                {
                    StringBuilderCache.Append($"<color={colorCode}>");
                }

                // 添加详细信息
                if (details != null && details.Length > 0)
                {
                    foreach (var detail in details)
                    {
                        if (!string.IsNullOrEmpty(detail))
                        {
                            StringBuilderCache.AppendLine(detail);
                        }
                    }
                }

                // 添加调用栈信息
                if (ShowStackTrace)
                {
                    var stackTrace = new StackTrace(2, true);
                    StringBuilderCache.AppendLine($"调用栈: {stackTrace.GetFrame(0)?.GetMethod()?.Name}");
                }

                // 详细信息颜色结束
                if (hasColor)
                {
                    StringBuilderCache.AppendLine("</color>");
                }

                string logMessage = StringBuilderCache.ToString();

                // 输出到Unity控制台
                OutputToConsole(level, logMessage);

                // 输出到文件（如果启用）
                if (EnableFileOutput)
                {
                    OutputToFile(level, logMessage);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"日志系统内部错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 输出到Unity控制台
        /// </summary>
        private static void OutputToConsole(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
            }
        }

        /// <summary>
        /// 输出到文件
        /// </summary>
        private static void OutputToFile(LogLevel level, string message)
        {
            try
            {
                string logPath = Path.Combine(UnityEngine.Application.persistentDataPath, "Logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                string fileName = $"Log_{DateTime.Now:yyyy-MM-dd}.txt";
                string filePath = Path.Combine(logPath, fileName);

                File.AppendAllText(filePath, $"{message}\n");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"写入日志文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否应该输出日志
        /// </summary>
        private static bool ShouldLog(LogLevel level)
        {
            return IsEnableLog && level >= MinLogLevel;
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 设置日志级别
        /// </summary>
        public static void SetLogLevel(LogLevel minLevel)
        {
            MinLogLevel = minLevel;
        }

        /// <summary>
        /// 清理日志文件（删除过期日志）
        /// </summary>
        public static void CleanupLogFiles(int keepDays = 7)
        {
            try
            {
                string logPath = Path.Combine(UnityEngine.Application.persistentDataPath, "Logs");
                if (!Directory.Exists(logPath)) return;

                var files = Directory.GetFiles(logPath, "Log_*.txt");
                var cutoffDate = DateTime.Now.AddDays(-keepDays);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"清理日志文件失败: {ex.Message}");
            }
        }

        #endregion
    }
}