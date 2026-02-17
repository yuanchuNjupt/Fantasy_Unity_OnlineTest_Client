using System;
using System.Collections.Generic;
using System.Linq;
using Framework.GameManagerFramework.Base;
using Framework.GameManagerFramework.Runtime;

namespace Framework.GameManager.Core
{
    public abstract class WorldManager
    {
        /// <summary>
        /// 默认的游戏世界
        /// </summary>
        public static World DefaultWorld { get; private set; }

        /// <summary>
        /// 世界不可重复
        /// </summary>
        private static readonly HashSet<World> Worlds = new();
    
    
        /// <summary>
        /// 构建一个游戏世界
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateWorld<T>(Action onWorldCreateFinishedCallBack = null) where T : World , new()
        {

            if (Worlds.OfType<T>().Any())
            {
                UnityEngine.Debug.LogWarning(nameof(T) + "已经创建，不可重复创建！");
                return;
            }
            
            T world = new T();
            DefaultWorld = world;
        
        
            TypeManager.InitializeWorldAssemblies(world , GetBehaviourExecution(world));
        
            //初始化游戏世界的程序集
            onWorldCreateFinishedCallBack?.Invoke();
            world.OnCreate();
            Worlds.Add(world);
        }

        public World GetWorld<T>() where T : World, new()
        {
            foreach (World world in Worlds)
            {
                if (world is T)
                {
                    return world as T;
                }
            }
            
            UnityEngine.Debug.LogWarning("未找到世界 : "+nameof(T) + "!");
            
            
            return null;
        }

        /// <summary>
        /// 销毁对应的游戏世界
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DestroyWorld<T>() where T : World, new()
        {
            foreach (var world in Worlds.OfType<T>())
            {
                world.DestroyWorld();
                Worlds.Remove(world);
            }
        }
    
        public static void DestroyAllWorld()
        {
            foreach (World world in Worlds)
            {
                world.DestroyWorld();
            }
            Worlds.Clear();
        }
    

        /// <summary>
        /// 需要在游戏主脚本Main中的Update方法中调用此方法
        /// </summary>
        public static void OnWorldUpdate()
        {
            foreach (World world in Worlds)
            {
                world.OnUpdate();
            }
        }
    
    

        public static IBehaviourExecution GetBehaviourExecution(World world)
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
