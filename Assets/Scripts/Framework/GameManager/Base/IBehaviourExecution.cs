using System;

namespace Framework.GameManagerFramework.Base
{
    public interface IBehaviourExecution
    {
        Type[] GetLogicBehaviourExecution();

        Type[] GetDataBehaviourExecution();
        
        Type[] GetMessageBehaviourExecution();
        
        
        
    }
}