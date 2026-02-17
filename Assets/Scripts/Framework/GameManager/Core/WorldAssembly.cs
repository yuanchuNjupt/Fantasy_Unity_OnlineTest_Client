
namespace Framework.GameManager.Core
{
    public partial class World
    {
        public bool AddLogicManager(ILogicBehaviour behaviour)
        {
            string key = behaviour.GetType().Name;
            if (_logicBehaviours.ContainsKey(key))
            {
                return false;
            }
            _logicBehaviours.Add(key, (behaviour , GetType()));
            return true;
        }

        public bool AddDataManager(IDataBehaviour behaviour)
        {
            string key = behaviour.GetType().Name;
            if (_dataBehaviours.ContainsKey(key))
            {
                return false;
            }
            _dataBehaviours.Add(key, (behaviour , GetType()));
            return true;
        }

        public bool AddMessageManager(IMessageBehaviour behaviour)
        {
            string key = behaviour.GetType().Name;
            if (_messageBehaviours.ContainsKey(key))
            {
                return false;
            }
            _messageBehaviours.Add(key, (behaviour , GetType()));
            return true;
        }
    
    
    }
}
