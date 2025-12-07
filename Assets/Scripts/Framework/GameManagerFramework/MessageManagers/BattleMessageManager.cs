using Framework.GameManagerFramework.WorldScripts;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleMessageManager : IMessageBehaviour
    {
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
        }
    }
}