using Account;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(PermanentlyWorld))]
    public class UserDataManager : IDataBehaviour
    {
        
        public UserData UserData { get; private set; }
        
        
        
        public void OnCreate()
        {
            UserData = new UserData();
        }

        public void OnDestroy()
        {
        }
    }
}