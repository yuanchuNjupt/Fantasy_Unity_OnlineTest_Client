using System;
using Framework.GameManagerFramework.Base;
using Framework.GameManagerFramework.Test;

namespace Framework.GameManagerFramework.Runtime
{
    public class HallWorldScriptExecutionOrder : IBehaviourExecution
    {
        public static Type[] LogicBehaviourExecutions = new Type[]
        {
            typeof(RegisterLogicManager) , typeof(LoginLogicManager),
        };

        public static Type[] DataBehaviourExecutions = new Type[]
        {

        };
        
        public static Type[] MessageBehaviourExecutions = new Type[]
        {

        };


        public Type[] GetLogicBehaviourExecution()
        {
            return LogicBehaviourExecutions;
        }

        public Type[] GetDataBehaviourExecution()
        {
            return DataBehaviourExecutions;
        }

        public Type[] GetMessageBehaviourExecution()
        {
            return MessageBehaviourExecutions;
        }
    }
}