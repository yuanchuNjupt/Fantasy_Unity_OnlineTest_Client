
public partial class World
{
    public void AddLogicManager(ILogicBehaviour behaviour)
    {
        _logicBehaviours.Add(behaviour.GetType().Name, (behaviour , GetType()));
        behaviour.OnCreate();
    }

    public void AddDataManager(IDataBehaviour behaviour)
    {
        _dataBehaviours.Add(behaviour.GetType().Name, (behaviour , GetType()));
        behaviour.OnCreate();
    }

    public void AddMessageManager(IMessageBehaviour behaviour)
    {
        _messageBehaviours.Add(behaviour.GetType().Name, (behaviour , GetType()));
        behaviour.OnCreate();
        
        
        
    }
    
    
}
