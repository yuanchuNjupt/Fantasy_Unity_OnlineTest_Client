using System;
using System.Collections.Generic;
using Framework.GameManagerFramework.Base;
using Framework.GameManagerFramework.Runtime;

namespace Framework.GameManager.Core
{
    public abstract class WorldManager
    {
        /// <summary>
        /// 默认的游戏世界
        /// </summary>
        public static Framework.GameManager.Core.World DefaultWorld { get; private set; }

        private static List<Framework.GameManager.Core.World> _worlds = new();
    
    
        /// <summary>
        /// 构建一个游戏世界
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateWorld<T>(Action onWorldCreateFinishedCallBack = null) where T : Framework.GameManager.Core.World , new()
        {
            T world = new T();
            DefaultWorld = world;
        
        
            TypeManager.InitializeWorldAssemblies(world , GetBehaviourExecution(world));
        
            //初始化游戏世界的程序集
            onWorldCreateFinishedCallBack?.Invoke();
            world.OnCreate();
            _worlds.Add(world);
        }

        /// <summary>
        /// 销毁对应的游戏世界
        /// </summary>
        /// <param name="world"></param>
        /// <typeparam name="T"></typeparam>
        public static void DestroyWorld<T>() where T : Framework.GameManager.Core.World, new()
        {
            for (int i = 0; i < _worlds.Count; i++)
            {
                if (_worlds[i] is T)
                {
                    _worlds[i].DestroyWorld();
                    _worlds.RemoveAt(i);
                    break;
                }
            }
        }
    
        public static void DestroyAllWorld()
        {
            _worlds.ForEach(world => world.DestroyWorld());
            _worlds.Clear();
        }
    

        /// <summary>
        /// 需要在游戏主脚本Main中的Update方法中调用此方法
        /// </summary>
        public static void OnWorldUpdate()
        {
            _worlds.ForEach(world => world.OnUpdate());
        }
    
    

        public static IBehaviourExecution GetBehaviourExecution(Framework.GameManager.Core.World world)
        {
            if (world.GetType().Name == "HallWorld")
            {
                return new HallWorldScriptExecutionOrder();
            }

            if (world.GetType().Name == "BattleWorld")
            {
                return new BattleWorldScriptExecutionOrder();
            }
            return null;
        }
    
    
    
    
    }
}
