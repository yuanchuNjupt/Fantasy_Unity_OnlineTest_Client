# 日志工具类使用文档

## 概述

这是一个为Unity项目设计的增强版日志工具类，提供了丰富的日志功能，包括颜色支持、级别控制、时间戳、文件输出等特性。

## 功能特性

- ✅ **多级别日志**：Debug、Info、Warning、Error
- ✅ **颜色支持**：9种预定义颜色，支持Unity富文本
- ✅ **灵活控制**：可单独开关各级别日志
- ✅ **时间戳**：精确到毫秒的时间记录
- ✅ **文件输出**：自动按日期分割日志文件
- ✅ **调用栈**：可选显示方法调用信息
- ✅ **异常处理**：专门的异常日志方法
- ✅ **性能优化**：使用缓存减少GC压力

## 快速开始

### 基础用法

```csharp

// 简单日志
Log.Debug("这是调试信息");
Log.Info("这是普通信息");
Log.Warning("这是警告信息");
Log.Error("这是错误信息");

// 带标题和描述的日志
Log.Debug("系统初始化", "正在加载配置文件");
Log.Info("用户操作", "玩家登录成功", "用户ID: 12345", "登录时间: 14:30:25");
Log.Warning("网络状态", "连接不稳定", "延迟: 200ms", "丢包率: 5%");
Log.Error("数据库", "查询失败", "SQL: SELECT * FROM users", "错误码: 1001");
```

### 彩色日志

```csharp
// 使用预定义颜色
Log.Debug(LogColor.Blue, "战斗系统", "技能释放", "技能ID: 1001", "伤害: 150");
Log.Info(LogColor.Green, "任务系统", "任务完成", "任务ID: 2001", "奖励: 100金币");
Log.Warning(LogColor.Yellow, "资源管理", "内存使用过高", "当前: 80%", "阈值: 75%");
Log.Error(LogColor.Red, "严重错误", "游戏崩溃", "原因: 空指针异常");
```

### 异常日志

```csharp
try 
{
    // 可能出错的代码
    int result = 10 / 0;
}
catch (Exception ex)
{
    Log.Error(ex, "计算异常");
    // 或者
    Log.Error(ex, "业务处理失败");
}
```

## 配置选项

### 基础开关

```csharp
// 全局日志开关
Log.IsEnableLog = true;          // 是否启用日志系统

// 分级别开关
Log.IsEnableDebug = true;        // 是否启用Debug日志
Log.IsEnableInfo = true;         // 是否启用Info日志
Log.IsEnableWarning = true;      // 是否启用Warning日志
Log.IsEnableError = true;        // 是否启用Error日志
```

### 显示选项

```csharp
// 时间戳
Log.ShowTimestamp = true;        // 显示时间戳 [14:30:25.123]

// 调用栈信息（调试用）
Log.ShowStackTrace = false;      // 显示方法调用信息
```

### 级别控制

```csharp
// 设置最低日志级别
Log.SetLogLevel(LogLevel.Warning);  // 只显示Warning和Error

// 或直接设置
Log.MinLogLevel = LogLevel.Info;     // 显示Info、Warning、Error
```

### 文件输出

```csharp
// 启用文件输出
Log.EnableFileOutput = true;

// 清理过期日志（保留7天）
Log.CleanupLogFiles(7);
```

## 颜色系统

### 可用颜色

| 颜色 | 枚举值 | 十六进制 | 效果 |
|------|--------|----------|------|
| 默认 | `LogColor.Default` | - | 无颜色 |
| 白色 | `LogColor.White` | #FFFFFF | 白色文本 |
| 黑色 | `LogColor.Black` | #000000 | 黑色文本 |
| 红色 | `LogColor.Red` | #FF0000 | 红色文本 |
| 橙色 | `LogColor.Orange` | #FFA500 | 橙色文本 |
| 黄色 | `LogColor.Yellow` | #FFFF00 | 黄色文本 |
| 绿色 | `LogColor.Green` | #00FF00 | 绿色文本 |
| 青色 | `LogColor.Cyan` | #00FFFF | 青色文本 |
| 蓝色 | `LogColor.Blue` | #4CB9FA | 蓝色文本 |
| 紫色 | `LogColor.Purple` | #FF00FF | 紫色文本 |

### 默认颜色方案

- **Debug**: 默认颜色
- **Info**: 绿色
- **Warning**: 黄色  
- **Error**: 红色

## 日志级别

### 级别说明

```csharp
public enum LogLevel
{
    Debug = 0,      // 调试信息，开发时使用
    Info = 1,       // 一般信息，正常业务流程
    Warning = 2,    // 警告信息，需要注意但不影响运行
    Error = 3       // 错误信息，严重问题需要处理
}
```

### 使用建议

- **Debug**: 详细的调试信息，发布版本可关闭
- **Info**: 重要的业务节点，如用户操作、系统状态变化
- **Warning**: 异常但可恢复的情况，如网络重连、资源不足
- **Error**: 严重错误，如崩溃、数据损坏、关键功能失效

## 文件输出

### 文件位置

日志文件保存在：`Application.persistentDataPath/Logs/`

不同平台的具体路径：
- **Windows**: `%userprofile%\AppData\LocalLow\<公司名>\<产品名>\Logs\`
- **Mac**: `~/Library/Application Support/<公司名>/<产品名>/Logs/`
- **Android**: `/storage/emulated/0/Android/data/<包名>/files/Logs/`

### 文件命名

- 格式：`Log_yyyy-MM-dd.txt`
- 示例：`Log_2024-01-15.txt`
- 每天一个文件，自动按日期分割

### 日志清理

```csharp
// 清理7天前的日志文件
Log.CleanupLogFiles(7);

// 清理30天前的日志文件
Log.CleanupLogFiles(30);
```

## 性能优化

### 内置优化

1. **StringBuilder缓存**: 重用StringBuilder对象，减少GC
2. **条件检查**: 在日志级别不匹配时直接返回，避免字符串构建
3. **延迟格式化**: 只有在需要输出时才进行字符串格式化

### 使用建议

```csharp
// ❌ 不推荐：即使Debug关闭也会执行字符串拼接
Log.Debug("玩家信息", $"玩家{player.Name}的血量是{player.Health}");

// ✅ 推荐：利用params参数，避免不必要的字符串拼接
Log.Debug("玩家信息", "玩家血量", $"玩家: {player.Name}", $"血量: {player.Health}");

// ✅ 更推荐：在调用前检查
if (Log.IsEnableDebug)
{
    Log.Debug("玩家信息", "玩家血量", $"玩家: {player.Name}", $"血量: {player.Health}");
}
```

## 实际应用示例

### 战斗系统日志

```csharp
public class BattleSystem
{
    public void AttackTarget(Unit attacker, Unit target, int damage)
    {
        Log.Debug(LogColor.Blue, "战斗系统", "攻击计算开始",
            $"攻击者: {attacker.Name}",
            $"目标: {target.Name}",
            $"基础伤害: {damage}");

        if (target.Health <= 0)
        {
            Log.Info(LogColor.Red, "战斗结果", "单位死亡",
                $"死亡单位: {target.Name}",
                $"击杀者: {attacker.Name}");
        }
    }
}
```

### 网络系统日志

```csharp
public class NetworkManager
{
    public void OnConnectionLost()
    {
        Log.Warning(LogColor.Orange, "网络连接", "连接丢失",
            "正在尝试重连...",
            $"重连次数: {retryCount}");
    }

    public void OnReconnected()
    {
        Log.Info(LogColor.Green, "网络连接", "重连成功",
            $"耗时: {reconnectTime}ms");
    }
}
```

### 资源管理日志

```csharp
public class ResourceManager
{
    public void LoadAsset(string assetPath)
    {
        try
        {
            // 加载资源
            var asset = Resources.Load(assetPath);
            Log.Info("资源管理", "资源加载成功", $"路径: {assetPath}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "资源加载失败");
        }
    }
}
```

## 发布版本配置

### 推荐的发布配置

```csharp
public static class LogConfig
{
    public static void SetupForRelease()
    {
        Log.IsEnableDebug = false;       // 关闭Debug日志
        Log.IsEnableInfo = true;         // 保留Info日志
        Log.IsEnableWarning = true;      // 保留Warning日志
        Log.IsEnableError = true;        // 保留Error日志
        Log.ShowTimestamp = true;        // 保留时间戳
        Log.ShowStackTrace = false;      // 关闭调用栈
        Log.EnableFileOutput = true;     // 启用文件输出
        Log.SetLogLevel(LogLevel.Info);  // 设置最低级别
    }

    public static void SetupForDebug()
    {
        Log.IsEnableDebug = true;        // 启用所有日志
        Log.IsEnableInfo = true;
        Log.IsEnableWarning = true;
        Log.IsEnableError = true;
        Log.ShowTimestamp = true;
        Log.ShowStackTrace = true;       // 启用调用栈
        Log.EnableFileOutput = false;    // 开发时可选
        Log.SetLogLevel(LogLevel.Debug);
    }
}
```

### 条件编译

```csharp
public static class GameInitializer
{
    public static void Initialize()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        LogConfig.SetupForDebug();
#else
        LogConfig.SetupForRelease();
#endif
    }
}
```

## 注意事项

1. **性能影响**: 大量的日志输出会影响性能，特别是文件输出
2. **存储空间**: 启用文件输出时注意清理过期日志
3. **敏感信息**: 避免在日志中记录密码、密钥等敏感信息
4. **线程安全**: 当前实现不是线程安全的，多线程使用需要额外同步
5. **Unity版本**: 需要Unity 2019.4或更高版本

## 故障排除

### 常见问题

**Q: 日志不显示颜色？**
A: 确保在Unity Console窗口中启用了"Rich Text"选项。

**Q: 文件输出不工作？**
A: 检查`Log.EnableFileOutput`是否为true，以及是否有文件写入权限。

**Q: 日志文件过大？**
A: 定期调用`Log.CleanupLogFiles()`清理过期日志。

**Q: 性能问题？**
A: 在发布版本中关闭Debug日志，或提高最低日志级别。
