using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.GameManagerFramework.Base;
using Framework.GameManager.Base;
using Framework.GameManager.Core;

namespace Framework.GameManagerFramework.Runtime
{
    public class TypeManager
    {
        
        
        private static IBehaviourExecution _behaviourExecution;
        
        public static void InitializeWorldAssemblies(GameManager.Core.World world , IBehaviourExecution behaviourExecution)
        {
            _behaviourExecution = behaviourExecution;
            
            //获取Unity和我们创建的脚本所在的程序集
            Assembly worldAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");

            if (worldAssembly == null)
            {
                UnityEngine.Debug.LogError("未找到 Assembly-CSharp 程序集！");
                return;
            }

            //获取世界类型
            Type worldType = world.GetType();
            
            //获取命名空间下的所有类型
            //判断脚本是否继承Behavior 
            Type logicType = typeof(ILogicBehaviour);
            Type dataType = typeof(IDataBehaviour);
            Type messageType = typeof(IMessageBehaviour);

            Type[] types = worldAssembly.GetTypes();

            List<TypeOrder> logicBehaviourList = new List<TypeOrder>();
            List<TypeOrder> dataBehaviourList = new List<TypeOrder>();
            List<TypeOrder> messageBehaviourList = new List<TypeOrder>();
            
            
            foreach (var type in types)
            {
                if (type.IsDefined(typeof(WorldSourceAttribute), false))
                {
                    //获取特性
                    var SourceWorld = type.GetCustomAttribute<WorldSourceAttribute>();
                    if (SourceWorld.WorldType == worldType)
                    {
                        //确定了是需要处理的类型
                        if(type.IsAbstract)
                            continue;
                        if (logicType.IsAssignableFrom(type))
                        {
                            TypeOrder order = new TypeOrder(GetLogicBehaviourOrderIndex(type), type);
                            logicBehaviourList.Add(order);
                        }
                        else if (messageType.IsAssignableFrom(type))
                        {
                            TypeOrder order = new TypeOrder(GetMessageBehaviourOrderIndex(type), type);
                            messageBehaviourList.Add(order);
                        }
                        else if(dataType.IsAssignableFrom(type))
                        {
                            TypeOrder order = new TypeOrder(GetDataBehaviourOrderIndex(type), type);
                            dataBehaviourList.Add(order);
                        }
                    }
                }
            }
            
            
            //排序
            logicBehaviourList.Sort((a,b)=> a.order.CompareTo(b.order));
            dataBehaviourList.Sort((a,b)=> a.order.CompareTo(b.order));
            messageBehaviourList.Sort((a,b)=> a.order.CompareTo(b.order));
            
            //初始化层级
            //数据层 > 消息层 > 逻辑层
            // 第一步：创建所有实例并添加到World字典中
            List<IDataBehaviour> dataInstances = new List<IDataBehaviour>();
            for (int i = 0; i < dataBehaviourList.Count; i++)
            {
                IDataBehaviour data = Activator.CreateInstance(dataBehaviourList[i].type) as IDataBehaviour;
                if (world.AddDataManager(data))
                {
                    dataInstances.Add(data);
                }
            }
            
            List<IMessageBehaviour> messageInstances = new List<IMessageBehaviour>();
            for (int i = 0; i < messageBehaviourList.Count; i++)
            {
                IMessageBehaviour message = Activator.CreateInstance(messageBehaviourList[i].type) as IMessageBehaviour;
                if (world.AddMessageManager(message))
                {
                    messageInstances.Add(message);
                }
            }

            List<ILogicBehaviour> logicInstances = new List<ILogicBehaviour>();
            for (int i = 0; i < logicBehaviourList.Count; i++)
            {
                ILogicBehaviour logic = Activator.CreateInstance(logicBehaviourList[i].type) as ILogicBehaviour;
                if (world.AddLogicManager(logic))
                {
                    logicInstances.Add(logic);
                }
            }
            
            // 第二步：依赖注入
            foreach (var data in dataInstances)
            {
                InjectDependencies(data);
            }
            foreach (var message in messageInstances)
            {
                InjectDependencies(message);
            }
            foreach (var logic in logicInstances)
            {
                InjectDependencies(logic);
            }
            
            // 第三步：调用OnCreate生命周期
            foreach (var data in dataInstances)
            {
                data.OnCreate();
            }
            foreach (var message in messageInstances)
            {
                message.OnCreate();
            }
            foreach (var logic in logicInstances)
            {
                logic.OnCreate();
            }
            
            logicBehaviourList.Clear();
            dataBehaviourList.Clear();
            messageBehaviourList.Clear();
            _behaviourExecution = null;


        }


        private static int GetLogicBehaviourOrderIndex(Type type)
        {
            if (_behaviourExecution == null)
            {
                return 999;
            }
            Type[] logicTypes = _behaviourExecution.GetLogicBehaviourExecution();
            for (int i = 0; i < logicTypes.Length; i++)
            {
                if (logicTypes[i] == type)
                {
                    return i;
                }
            }
            return 999;
        }
        
        private static int GetDataBehaviourOrderIndex(Type type)
        {
            if (_behaviourExecution == null)
            {
                return 999;
            }
            Type[] dataTypes = _behaviourExecution.GetDataBehaviourExecution();
            for (int i = 0; i < dataTypes.Length; i++)
            {
                if (dataTypes[i] == type)
                {
                    return i;
                }
            }
            return 999;
        }
        
        private static int GetMessageBehaviourOrderIndex(Type type)
        {
            if (_behaviourExecution == null)
            {
                return 999;
            }
            Type[] messageTypes = _behaviourExecution.GetMessageBehaviourExecution();
            for (int i = 0; i < messageTypes.Length; i++)
            {
                if (messageTypes[i] == type)
                {
                    return i;
                }
            }
            return 999;
        }
        
        /// <summary>
        /// 依赖注入方法，自动注入标记了[Inject]特性的字段和属性
        /// </summary>
        private static void InjectDependencies(object target)
        {
            if (target == null)
            {
                return;
            }

            Type targetType = target.GetType();
            
            // 注入字段
            FieldInfo[] fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(InjectAttribute), true))
                {
                    Type fieldType = field.FieldType;
                    object injectedValue = GetManagerInstance(fieldType);
                    
                    if (injectedValue == null)
                    {
                        UnityEngine.Debug.LogError($"[依赖注入失败] 类型: {targetType.Name}, 字段: {field.Name}, 需要注入的类型: {fieldType.Name} 未找到或未实例化！请检查Manager的初始化顺序。");
                    }
                    else
                    {
                        field.SetValue(target, injectedValue);
                    }
                }
            }
            
            // 注入属性
            PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(InjectAttribute), true) && property.CanWrite)
                {
                    Type propertyType = property.PropertyType;
                    object injectedValue = GetManagerInstance(propertyType);
                    
                    if (injectedValue == null)
                    {
                        UnityEngine.Debug.LogError($"[依赖注入失败] 类型: {targetType.Name}, 属性: {property.Name}, 需要注入的类型: {propertyType.Name} 未找到或未实例化！请检查Manager的初始化顺序。");
                    }
                    else
                    {
                        property.SetValue(target, injectedValue);
                    }
                }
            }
        }
        
        /// <summary>
        /// 根据类型从World中获取对应的Manager实例
        /// </summary>
        private static object GetManagerInstance(Type managerType)
        {
            // 直接调用World的非泛型查找方法，避免反射
            return World.GetManagerByType(managerType);
        }
    }
}