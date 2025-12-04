using UnityEngine;



[WorldSource(typeof(HallWorld))]
public class RegisterLogicManager : ILogicBehaviour
{
    public void OnCreate()
    {
        Debug.Log("RegisterLogicManager.OnCreate");
    }

    public void OnDestroy()
    {
        Debug.Log("RegisterLogicManager.OnDestroy");
    }
}
