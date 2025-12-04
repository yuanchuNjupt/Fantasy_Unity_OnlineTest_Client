using UnityEngine;

namespace Framework.GameManagerFramework.Test
{
    
    [WorldSource(typeof(HallWorld))]
    public class RegisterDataManager : IDataBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("RegisterDataManager OnCreate");
        }

        public void OnDestroy()
        {
            Debug.Log("RegisterDataManager OnDestroy");
        }
    }
}