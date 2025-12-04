using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.GameManagerFramework.Editor
{
    /// <summary>
    /// 用于持久化存储 GameManager 数据的 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "GameManagerData", menuName = "Framework/GameManagerData")]
    public class GameManagerData : ScriptableObject
    {
        [SerializeField]
        public List<string> worlds = new List<string>();
        
        [SerializeField]
        public List<ManagerInfo> logicManagers = new List<ManagerInfo>();
        
        [SerializeField]
        public List<ManagerInfo> dataManagers = new List<ManagerInfo>();
        
        [SerializeField]
        public List<ManagerInfo> messageManagers = new List<ManagerInfo>();
    }
    
    /// <summary>
    /// Manager 信息（名称和所属世界）
    /// </summary>
    [Serializable]
    public class ManagerInfo
    {
        public string name;
        public string world;
        
        public ManagerInfo(string name, string world)
        {
            this.name = name;
            this.world = world;
        }
    }
}

