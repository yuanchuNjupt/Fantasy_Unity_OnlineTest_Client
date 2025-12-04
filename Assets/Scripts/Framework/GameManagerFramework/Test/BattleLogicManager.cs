using UnityEngine;

namespace Framework.GameManagerFramework.Test
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleLogicManager : ILogicBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("BattleLogicManager OnCreate");
        }

        public void OnDestroy()
        {
        }
    }
}