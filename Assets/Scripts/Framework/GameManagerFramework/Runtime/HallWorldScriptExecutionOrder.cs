using System;
using Framework.GameManagerFramework.Base;

namespace Framework.GameManagerFramework.Runtime
{
    public class HallWorldScriptExecutionOrder : IBehaviourExecution
    {
        public static Type[] LogicBehaviourExecutions = new Type[]
        {
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