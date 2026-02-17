using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.LogicManagers
{
    
    [WorldSource(typeof(GlobalWorld))]
    public class UserMouseLogicManager : ILogicBehaviour
    {
        
        public GameInputAction GameInput { get; private set; }
        
        
        public void OnCreate()
        {
            GameInput = new GameInputAction();
            GameInput.Enable();
        }

        public void OnDestroy()
        {
            GameInput.Disable();
            GameInput = null;
        }
    }
}