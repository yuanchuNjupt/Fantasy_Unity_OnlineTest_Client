using System;
using System.Collections;

using UnityEngine;

namespace Framework.AdvancedLog
{
    /// <summary>
    /// 日志工具类使用示例
    /// 展示了在实际项目中如何使用Log系统
    /// </summary>
    public class LogExample : MonoBehaviour
    {
        [Header("示例配置")]
        [SerializeField] private bool enableFileOutput = false;

        [SerializeField] private bool showTimestamp = true;
        [SerializeField] private bool showStackTrace = false;
        [SerializeField] private LogLevel minLogLevel = LogLevel.Debug;

        private void Start()
        {
            // Debug.Log("<color=red>颜色测试日志1</color>");
            // Debug.Log("<color=red>颜色</color>测试日志2");
            // Debug.Log("<color=#FFA500>颜色</color>测试日志3");
            // Debug.Log("<color=red>颜色\n测试\n日志4</color>");

            // 配置日志系统
            SetupLogSystem();

            // 演示各种日志用法
            StartCoroutine(RunLogExamples());
        }

        /// <summary>
        /// 配置日志系统
        /// </summary>
        private void SetupLogSystem()
        {
            Log.Info(LogColor.Cyan, "系统初始化", "正在配置日志系统...");

            // 基础配置
            Log.EnableFileOutput = enableFileOutput;
            Log.ShowTimestamp = showTimestamp;
            Log.ShowStackTrace = showStackTrace;
            Log.SetLogLevel(minLogLevel);

            Log.Info(LogColor.Green, "系统初始化", "日志系统配置完成",
                $"文件输出: {(enableFileOutput ? "启用" : "禁用")}",
                $"时间戳: {(showTimestamp ? "显示" : "隐藏")}",
                $"最低级别: {minLogLevel}");
        }

        /// <summary>
        /// 运行各种日志示例
        /// </summary>
        private IEnumerator RunLogExamples()
        {
            yield return new WaitForSeconds(1f);

            // 1. 基础日志示例
            BasicLogExamples();
            yield return new WaitForSeconds(2f);

            // 2. 游戏系统日志示例
            GameSystemLogExamples();
            yield return new WaitForSeconds(2f);

            // 3. 异常处理示例
            ExceptionHandlingExamples();
            yield return new WaitForSeconds(2f);

            // 4. 性能监控示例
            PerformanceMonitoringExamples();
            yield return new WaitForSeconds(2f);

            // 5. 条件日志示例
            ConditionalLoggingExamples();
            yield return new WaitForSeconds(2f);

            // 6. 文件管理示例
            FileManagementExamples();
        }

        #region 基础日志示例

        /// <summary>
        /// 基础日志使用示例
        /// </summary>
        private void BasicLogExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 基础日志示例 ===");

            // 简单日志
            Log.Debug("这是一条调试信息");
            Log.Info("这是一条普通信息");
            Log.Warning("这是一条警告信息");
            Log.Error("这是一条错误信息");

            // 带标题的日志
            Log.Debug("调试模块", "变量值检查完成");
            Log.Info("用户操作", "玩家点击了开始按钮");
            Log.Warning("系统状态", "内存使用率较高");
            Log.Error("网络错误", "连接服务器失败");

            // 带详细信息的日志
            Log.Debug("数据加载", "正在加载玩家数据",
                "玩家ID: 12345",
                "存档版本: 1.2.0",
                "加载时间: 150ms");

            // 彩色日志
            Log.Debug(LogColor.Cyan, "系统", "调试信息", "这是青色的调试日志");
            Log.Info(LogColor.Green, "成功", "操作完成", "这是绿色的成功日志");
            Log.Warning(LogColor.Yellow, "警告", "注意事项", "这是黄色的警告日志");
            Log.Error(LogColor.Red, "错误", "严重问题", "这是红色的错误日志");
        }

        #endregion

        #region 游戏系统日志示例

        /// <summary>
        /// 游戏系统日志示例
        /// </summary>
        private void GameSystemLogExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 游戏系统日志示例 ===");

            // 战斗系统日志
            BattleSystemLog();

            // UI系统日志
            UISystemLog();

            // 资源管理日志
            ResourceManagementLog();

            // 存档系统日志
            SaveSystemLog();
        }

        /// <summary>
        /// 战斗系统日志示例
        /// </summary>
        private void BattleSystemLog()
        {
            Log.Debug(LogColor.Purple, "战斗系统", "战斗开始",
                "玩家血量: 100",
                "敌人血量: 80",
                "战斗场景: 森林");

            Log.Info(LogColor.Orange, "战斗系统", "技能释放",
                "技能名称: 火球术",
                "消耗MP: 20",
                "造成伤害: 35");

            Log.Warning(LogColor.Yellow, "战斗系统", "血量不足",
                "当前血量: 15",
                "建议: 使用治疗药水");

            Log.Info(LogColor.Green, "战斗系统", "战斗胜利",
                "获得经验: 150",
                "获得金币: 50",
                "战斗时长: 45秒");
        }

        /// <summary>
        /// UI系统日志示例
        /// </summary>
        private void UISystemLog()
        {
            Log.Debug("UI系统", "面板打开", "背包面板");
            Log.Info("UI系统", "按钮点击", "设置按钮");
            Log.Warning("UI系统", "界面卡顿", "帧率低于30FPS");
        }

        /// <summary>
        /// 资源管理日志示例
        /// </summary>
        private void ResourceManagementLog()
        {
            Log.Debug(LogColor.Cyan, "资源管理", "开始加载资源",
                "资源类型: 纹理",
                "资源路径: Textures/Characters/Hero");

            Log.Info(LogColor.Green, "资源管理", "资源加载成功",
                "资源名称: hero_texture.png",
                "文件大小: 2.5MB",
                "加载时间: 120ms");

            Log.Warning(LogColor.Orange, "资源管理", "内存使用警告",
                "当前使用: 512MB",
                "可用内存: 256MB",
                "建议: 释放未使用资源");
        }

        /// <summary>
        /// 存档系统日志示例
        /// </summary>
        private void SaveSystemLog()
        {
            Log.Debug("存档系统", "开始保存游戏");
            Log.Info("存档系统", "保存完成", "存档位置: slot_1.save");
            Log.Debug("存档系统", "开始加载游戏");
            Log.Info("存档系统", "加载完成", "加载时间: 200ms");
        }

        #endregion

        #region 异常处理示例

        /// <summary>
        /// 异常处理日志示例
        /// </summary>
        private void ExceptionHandlingExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 异常处理示例 ===");

            // 模拟空引用异常
            try
            {
                string nullString = null;
                int length = nullString.Length; // 这会抛出异常
            }
            catch (NullReferenceException ex)
            {
                Log.Error(ex, "空引用异常");
            }

            // 模拟数组越界异常
            try
            {
                int[] array = new int[5];
                int value = array[10]; // 这会抛出异常
            }
            catch (IndexOutOfRangeException ex)
            {
                Log.Error(ex, "数组越界异常");
            }

            // 模拟自定义异常
            try
            {
                ThrowCustomException();
            }
            catch (Exception ex)
            {
                Log.Error(LogColor.Red, "自定义异常", "业务逻辑错误",
                    $"异常类型: {ex.GetType().Name}",
                    $"错误信息: {ex.Message}");
            }
        }

        /// <summary>
        /// 抛出自定义异常用于演示
        /// </summary>
        private void ThrowCustomException()
        {
            throw new InvalidOperationException("这是一个演示用的自定义异常");
        }

        #endregion

        #region 性能监控示例

        /// <summary>
        /// 性能监控日志示例
        /// </summary>
        private void PerformanceMonitoringExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 性能监控示例 ===");

            // 模拟性能监控
            var startTime = DateTime.Now;

            // 模拟一些耗时操作
            System.Threading.Thread.Sleep(100);

            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;

            Log.Info(LogColor.Green, "性能监控", "操作完成",
                $"开始时间: {startTime:HH:mm:ss.fff}",
                $"结束时间: {endTime:HH:mm:ss.fff}",
                $"耗时: {duration:F1}ms");

            // 内存使用情况
            long memoryBefore = GC.GetTotalMemory(false);

            // 创建一些对象
            for (int i = 0; i < 1000; i++)
            {
                var obj = new object();
            }

            long memoryAfter = GC.GetTotalMemory(false);

            Log.Debug(LogColor.Orange, "内存监控", "内存使用变化",
                $"操作前: {memoryBefore / 1024}KB",
                $"操作后: {memoryAfter / 1024}KB",
                $"增长: {(memoryAfter - memoryBefore) / 1024}KB");

            // FPS监控示例
            float fps = 1.0f / Time.deltaTime;
            if (fps < 30)
            {
                Log.Warning(LogColor.Red, "性能警告", "帧率过低",
                    $"当前FPS: {fps:F1}",
                    "建议: 降低画质设置");
            }
            else
            {
                Log.Debug("性能监控", $"当前FPS: {fps:F1}");
            }
        }

        #endregion

        #region 条件日志示例

        /// <summary>
        /// 条件日志示例
        /// </summary>
        private void ConditionalLoggingExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 条件日志示例 ===");

            // 根据条件决定是否输出日志
            bool isDebugMode = UnityEngine.Application.isEditor;

            if (isDebugMode)
            {
                Log.Debug("开发模式", "详细调试信息",
                    "当前场景: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                    "Unity版本: " + UnityEngine.Application.unityVersion,
                    "平台: " + UnityEngine.Application.platform);
            }

            // 性能敏感的日志
            if (Log.IsEnableDebug && Log.MinLogLevel <= LogLevel.Debug)
            {
                // 只有在Debug日志启用时才进行复杂的字符串构建
                string complexInfo = BuildComplexDebugInfo();
                Log.Debug("复杂调试", complexInfo);
            }

            // 根据日志级别输出不同详细程度的信息
            LogPlayerInfo("玩家123", 100, 50, new Vector3(10, 0, 5));
        }

        /// <summary>
        /// 构建复杂的调试信息（模拟耗时操作）
        /// </summary>
        private string BuildComplexDebugInfo()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("=== 系统状态详情 ===");
            sb.AppendLine($"内存使用: {GC.GetTotalMemory(false) / 1024 / 1024}MB");
            sb.AppendLine($"活跃GameObject数量: {FindObjectsOfType<GameObject>().Length}");
            sb.AppendLine($"系统时间: {DateTime.Now}");
            return sb.ToString();
        }

        /// <summary>
        /// 根据日志级别输出玩家信息
        /// </summary>
        private void LogPlayerInfo(string playerId, int health, int mana, Vector3 position)
        {
            if (Log.MinLogLevel <= LogLevel.Debug)
            {
                // Debug级别：输出详细信息
                Log.Debug(LogColor.Cyan, "玩家状态", "详细信息",
                    $"玩家ID: {playerId}",
                    $"血量: {health}/100",
                    $"魔法值: {mana}/100",
                    $"位置: {position}",
                    $"时间戳: {DateTime.Now:HH:mm:ss.fff}");
            }
            else if (Log.MinLogLevel <= LogLevel.Info)
            {
                // Info级别：输出基本信息
                Log.Info("玩家状态", "基本信息",
                    $"玩家ID: {playerId}",
                    $"血量: {health}",
                    $"魔法值: {mana}");
            }
            else if (Log.MinLogLevel <= LogLevel.Warning && health < 20)
            {
                // Warning级别：只在血量低时输出
                Log.Warning("玩家状态", "血量危险",
                    $"玩家ID: {playerId}",
                    $"血量: {health}");
            }
        }

        #endregion

        #region 文件管理示例

        /// <summary>
        /// 文件管理示例
        /// </summary>
        private void FileManagementExamples()
        {
            Log.Info(LogColor.Blue, "示例演示", "=== 文件管理示例 ===");

            if (Log.EnableFileOutput)
            {
                Log.Info(LogColor.Green, "文件输出", "日志文件输出已启用",
                    $"日志路径: {UnityEngine.Application.persistentDataPath}/Logs/",
                    $"文件命名: Log_yyyy-MM-dd.txt");

                // 演示清理日志文件
                Log.Info("文件管理", "准备清理过期日志文件...");
                Log.CleanupLogFiles(7); // 清理7天前的日志
                Log.Info("文件管理", "日志文件清理完成");
            }
            else
            {
                Log.Warning(LogColor.Orange, "文件输出", "日志文件输出未启用",
                    "如需启用，请设置 Log.EnableFileOutput = true");
            }
        }

        #endregion

        #region UI控制方法

        /// <summary>
        /// 通过UI按钮测试不同的日志功能
        /// </summary>
        [ContextMenu("测试所有日志级别")]
        public void TestAllLogLevels()
        {
            Log.Debug(LogColor.Cyan, "测试", "这是Debug级别日志");
            Log.Info(LogColor.Green, "测试", "这是Info级别日志");
            Log.Warning(LogColor.Yellow, "测试", "这是Warning级别日志");
            Log.Error(LogColor.Red, "测试", "这是Error级别日志");
        }

        [ContextMenu("测试异常日志")]
        public void TestExceptionLog()
        {
            try
            {
                throw new System.Exception("这是一个测试异常");
            }
            catch (System.Exception ex)
            {
                Log.Error(ex, "异常测试");
            }
        }

        [ContextMenu("切换文件输出")]
        public void ToggleFileOutput()
        {
            Log.EnableFileOutput = !Log.EnableFileOutput;
            Log.Info("设置", $"文件输出已{(Log.EnableFileOutput ? "启用" : "禁用")}");
        }

        [ContextMenu("切换时间戳")]
        public void ToggleTimestamp()
        {
            Log.ShowTimestamp = !Log.ShowTimestamp;
            Log.Info("设置", $"时间戳显示已{(Log.ShowTimestamp ? "启用" : "禁用")}");
        }

        [ContextMenu("清理日志文件")]
        public void CleanupLogs()
        {
            Log.CleanupLogFiles(1); // 清理1天前的日志
            Log.Info("维护", "日志文件清理完成");
        }

        #endregion

        #region 生命周期方法

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Log.Info("应用生命周期", "应用已暂停");
            }
            else
            {
                Log.Info("应用生命周期", "应用已恢复");
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Log.Debug("应用生命周期", "应用获得焦点");
            }
            else
            {
                Log.Debug("应用生命周期", "应用失去焦点");
            }
        }

        private void OnDestroy()
        {
            Log.Info(LogColor.Orange, "示例演示", "LogExample组件即将销毁");
        }

        #endregion
    }
}