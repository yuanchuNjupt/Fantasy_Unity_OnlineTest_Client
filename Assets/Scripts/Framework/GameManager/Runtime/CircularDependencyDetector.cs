using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.GameManagerFramework.Base;
using Framework.GameManager.Base;
using UnityEngine;

namespace Framework.GameManagerFramework.Runtime
{
    /// <summary>
    /// 循环依赖检测工具（可选功能）
    /// 使用方法：在 TypeManager.InitializeWorldAssemblies 开始时调用
    /// CircularDependencyDetector.AnalyzeDependencies(logicBehaviourList, dataBehaviourList, messageBehaviourList);
    /// </summary>
    public static class CircularDependencyDetector
    {
        /// <summary>
        /// 分析并检测所有Manager的依赖关系
        /// </summary>
        public static void AnalyzeDependencies(
            List<Type> logicTypes, 
            List<Type> dataTypes, 
            List<Type> messageTypes)
        {
            Debug.Log("=== 开始分析Manager依赖关系 ===");
            
            var allTypes = new List<Type>();
            allTypes.AddRange(dataTypes);
            allTypes.AddRange(messageTypes);
            allTypes.AddRange(logicTypes);
            
            // 构建依赖图
            var dependencyGraph = BuildDependencyGraph(allTypes);
            
            // 检测循环依赖
            var circularDependencies = DetectCircularDependencies(dependencyGraph);
            
            if (circularDependencies.Count > 0)
            {
                Debug.LogWarning($"⚠️ 检测到 {circularDependencies.Count} 个循环依赖！");
                foreach (var cycle in circularDependencies)
                {
                    Debug.LogWarning($"🔄 循环依赖: {string.Join(" → ", cycle)} → {cycle[0]}");
                }
            }
            else
            {
                Debug.Log("✅ 未检测到循环依赖");
            }
            
            // 检测跨层级依赖
            CheckCrossLayerDependencies(logicTypes, dataTypes, messageTypes);
            
            Debug.Log("=== 依赖关系分析完成 ===");
        }
        
        /// <summary>
        /// 构建依赖关系图
        /// </summary>
        private static Dictionary<Type, List<Type>> BuildDependencyGraph(List<Type> types)
        {
            var graph = new Dictionary<Type, List<Type>>();
            
            foreach (var type in types)
            {
                var dependencies = new List<Type>();
                
                // 扫描字段
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.IsDefined(typeof(InjectAttribute), true))
                    {
                        dependencies.Add(field.FieldType);
                    }
                }
                
                // 扫描属性
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    if (property.IsDefined(typeof(InjectAttribute), true))
                    {
                        dependencies.Add(property.PropertyType);
                    }
                }
                
                graph[type] = dependencies;
            }
            
            return graph;
        }
        
        /// <summary>
        /// 检测循环依赖（使用DFS算法）
        /// </summary>
        private static List<List<string>> DetectCircularDependencies(Dictionary<Type, List<Type>> graph)
        {
            var cycles = new List<List<string>>();
            var visited = new HashSet<Type>();
            var recursionStack = new HashSet<Type>();
            var path = new List<Type>();
            
            foreach (var node in graph.Keys)
            {
                if (!visited.Contains(node))
                {
                    DFS(node, graph, visited, recursionStack, path, cycles);
                }
            }
            
            return cycles;
        }
        
        private static bool DFS(
            Type node, 
            Dictionary<Type, List<Type>> graph,
            HashSet<Type> visited,
            HashSet<Type> recursionStack,
            List<Type> path,
            List<List<string>> cycles)
        {
            visited.Add(node);
            recursionStack.Add(node);
            path.Add(node);
            
            if (graph.ContainsKey(node))
            {
                foreach (var neighbor in graph[node])
                {
                    if (!graph.ContainsKey(neighbor))
                        continue;
                        
                    if (!visited.Contains(neighbor))
                    {
                        if (DFS(neighbor, graph, visited, recursionStack, path, cycles))
                            return true;
                    }
                    else if (recursionStack.Contains(neighbor))
                    {
                        // 找到循环依赖
                        var cycleIndex = path.IndexOf(neighbor);
                        var cycle = path.Skip(cycleIndex).Select(t => t.Name).ToList();
                        cycles.Add(cycle);
                    }
                }
            }
            
            path.Remove(node);
            recursionStack.Remove(node);
            return false;
        }
        
        /// <summary>
        /// 检查跨层级依赖（不符合架构原则）
        /// </summary>
        private static void CheckCrossLayerDependencies(
            List<Type> logicTypes, 
            List<Type> dataTypes, 
            List<Type> messageTypes)
        {
            Debug.Log("--- 检查跨层级依赖 ---");
            
            var hasIssue = false;
            
            // 检查 Data 层是否依赖 Logic 或 Message
            foreach (var dataType in dataTypes)
            {
                var dependencies = GetDependencies(dataType);
                
                foreach (var dep in dependencies)
                {
                    if (logicTypes.Contains(dep))
                    {
                        Debug.LogError($"❌ 架构违规: DataBehaviour [{dataType.Name}] 不应该依赖 LogicBehaviour [{dep.Name}]");
                        hasIssue = true;
                    }
                    if (messageTypes.Contains(dep))
                    {
                        Debug.LogError($"❌ 架构违规: DataBehaviour [{dataType.Name}] 不应该依赖 MessageBehaviour [{dep.Name}]");
                        hasIssue = true;
                    }
                }
            }
            
            // 检查 Message 层是否依赖 Logic
            foreach (var messageType in messageTypes)
            {
                var dependencies = GetDependencies(messageType);
                
                foreach (var dep in dependencies)
                {
                    if (logicTypes.Contains(dep))
                    {
                        Debug.LogError($"❌ 架构违规: MessageBehaviour [{messageType.Name}] 不应该依赖 LogicBehaviour [{dep.Name}]");
                        hasIssue = true;
                    }
                }
            }
            
            if (!hasIssue)
            {
                Debug.Log("✅ 未检测到跨层级依赖违规");
            }
        }
        
        private static List<Type> GetDependencies(Type type)
        {
            var dependencies = new List<Type>();
            
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(InjectAttribute), true))
                {
                    dependencies.Add(field.FieldType);
                }
            }
            
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(InjectAttribute), true))
                {
                    dependencies.Add(property.PropertyType);
                }
            }
            
            return dependencies;
        }
        
        /// <summary>
        /// 可视化输出依赖关系图（可选）
        /// </summary>
        public static void PrintDependencyTree(List<Type> types)
        {
            Debug.Log("=== Manager依赖关系树 ===");
            
            foreach (var type in types)
            {
                var dependencies = GetDependencies(type);
                
                if (dependencies.Count > 0)
                {
                    Debug.Log($"📦 {type.Name}");
                    foreach (var dep in dependencies)
                    {
                        Debug.Log($"  └─ depends on → {dep.Name}");
                    }
                }
                else
                {
                    Debug.Log($"📦 {type.Name} (无依赖)");
                }
            }
            
            Debug.Log("=== 依赖关系树结束 ===");
        }
    }
}

