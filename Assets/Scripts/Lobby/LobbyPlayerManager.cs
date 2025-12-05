﻿using System.Collections.Generic;
using Config;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using Helper;
using Lobby.TeamInfo;
using Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace Lobby
{
    public class LobbyPlayerManager : GGG.Tool.Singleton.Singleton<LobbyPlayerManager>
    {
        public TeamManagerComponent teamManager = new TeamManagerComponent();
        
        
    }



}







