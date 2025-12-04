using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.GameManagerFramework.Base;

namespace Framework.GameManagerFramework.Runtime
{
    public class TypeManager
    {
        
        
        private static IBehaviourExecution _behaviourExecution;
        
        public static void InitializeWorldAssemblies(World world , IBehaviourExecution behaviourExecution)
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
            for (int i = 0; i < dataBehaviourList.Count; i++)
            {
                IDataBehaviour data = Activator.CreateInstance(dataBehaviourList[i].type) as IDataBehaviour;
                world.AddDataManager(data);
            }
            for (int i = 0; i < messageBehaviourList.Count; i++)
            {
                IMessageBehaviour message = Activator.CreateInstance(messageBehaviourList[i].type) as IMessageBehaviour;
                world.AddMessageManager(message);
            }

            for (int i = 0; i < logicBehaviourList.Count; i++)
            {
                ILogicBehaviour logic = Activator.CreateInstance(logicBehaviourList[i].type) as ILogicBehaviour;
                world.AddLogicManager(logic);
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
    }
}