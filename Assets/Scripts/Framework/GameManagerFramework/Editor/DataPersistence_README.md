# GameManager 数据持久化说明

## 问题
之前窗口关闭后，所有创建的 World 和 Manager 数据都会丢失。

## 解决方案
使用 ScriptableObject 实现数据持久化存储。

## 新增文件

### 1. GameManagerData.cs
ScriptableObject 数据资产类，用于持久化存储：
- World 列表
- Logic Manager 列表（名称 + 所属世界）
- Data Manager 列表（名称 + 所属世界）
- Message Manager 列表（名称 + 所属世界）

### 2. GameManagerDataInitializer.cs
提供菜单工具用于初始化和管理数据资产：
- `Framework/GameManager/Initialize Data Asset` - 创建/定位数据资产
- `Framework/GameManager/Clear All Data` - 清空所有数据

## 使用方法

### 首次使用
1. 在 Unity 菜单栏选择 `Framework > GameManager > Initialize Data Asset`
2. 系统会自动创建 `GameManagerData.asset` 文件（如果不存在）
3. 文件位置：`Assets/Scripts/Framework/GameManagerFramework/Editor/GameManagerData.asset`

### 数据自动保存
- 每次创建 World、Logic、Data 或 Message Manager 时，数据会自动保存到资产文件
- 关闭窗口或重启 Unity 后，数据会自动加载

### 清空数据
- 如果需要清空所有数据，选择 `Framework > GameManager > Clear All Data`

## 技术细节

### GameFrameworkDataManager 修改
- 不再使用静态列表直接存储数据
- 通过 `DataAsset` 属性访问持久化的 ScriptableObject
- 提供 `SaveData()` 方法用于保存数据

### CreateModelConfig 修改
- 每次创建新项时，通过 `AssetDatabase.LoadAssetAtPath` 获取数据资产
- 将数据添加到资产的对应列表中
- 调用 `GameFrameworkDataManager.SaveData()` 保存

## 注意事项
- 数据资产文件会被 Git 跟踪，建议团队成员同步此文件
- 如果移动了数据资产文件位置，需要更新代码中的路径常量
- 数据资产可以在 Inspector 中查看和手动编辑

