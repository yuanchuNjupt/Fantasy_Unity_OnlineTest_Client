using System;
using Battle;
using Framework.GameManagerFramework.Base;
using Framework.GameManagerFramework.LogicManagers;

namespace Framework.GameManagerFramework.Runtime
{
    public class BattleWorldScriptExecutionOrder : IBehaviourExecution
    {
        public static Type[] LogicBehaviourExecutions = new Type[]
        {
            typeof(BattlePlayerLogicManager) , typeof(BattleLogicManager)
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