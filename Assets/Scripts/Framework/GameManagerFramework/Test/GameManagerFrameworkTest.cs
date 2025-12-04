using System.Collections;
using System.Collections.Generic;
using Framework.GameManagerFramework.Test;
using UnityEngine;

public class GameManagerFrameworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WorldManager.CreateWorld<HallWorld>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WorldManager.CreateWorld<BattleWorld>();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            WorldManager.DestroyWorld<HallWorld>();
        }
    }
}
