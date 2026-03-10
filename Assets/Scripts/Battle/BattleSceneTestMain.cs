using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;

public class BattleSceneTestMain : MonoBehaviour
{
    
    public UserDataManager userDataManager;
    
    void Awake()
    {
        WorldManager.CreateWorld<GlobalWorld>();
        userDataManager = World.GetExitsDataManager<UserDataManager>();
        userDataManager.UserData.AccountId = 1000;
        userDataManager.UserData.AccountName = "TestAccount";
        userDataManager.UserData.UserName = "TestPlayer";
        
        
        
        
        
        WorldManager.CreateWorld<BattleWorld>();
        World.GetExitsDataManager<BattleDataManager>().InitBattlePlayerData(new List<BattlePlayerData>() {new BattlePlayerData()
        {
            playerId = 1000,
            playerName = "TestPlayer"
        } , new BattlePlayerData()
        {
            playerId =  1001,
            playerName = "OtherPlayer"
        }
        });
        World.GetExitsLogicManager<BattleLogicManager>().OnStartBattle();
        
        

    }

    // Update is called once per frame
    void Update()
    {
        WorldManager.OnWorldUpdate();
    }
}
