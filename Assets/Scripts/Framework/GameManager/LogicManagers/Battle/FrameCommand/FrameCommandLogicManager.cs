using Framework.GameManager.Base;
using Framework.GameManager.Core;
using Framework.GameManager.DataManagers;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.LogicManagers.FrameCommand
{
    [WorldSource(typeof(BattleWorld))]
    public class FrameCommandLogicManager : ILogicBehaviour
    {
        [Inject]
        private FrameCommandDataManager _frameCommandDataManager;
        
        
        
        
        public void OnCreate()
        {
            
            
        }

        public void OnDestroy()
        {
        }
    }
}