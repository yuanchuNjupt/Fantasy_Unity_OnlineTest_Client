using UnityEngine;


    
[WorldSource(typeof(HallWorld))]
public class LoginLogicManager : ILogicBehaviour
{
    public void OnCreate()
    {
        Debug.Log("LoginLogicManager OnCreate");
    }

    public void OnDestroy()
    {
        Debug.Log("LoginLogicManager OnDestroy");
    }
}
