
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Framework.MessageManagers;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterNamePanelPresenter : BasePresenter<RegisterNamePanelView>
{
    private void Awake()
    {
        View.Commit.onClick.AddListener(OnCommitButtonClick);
    }
    
    private void OnDestroy()
    {
        View.Commit.onClick.RemoveListener(OnCommitButtonClick);
    }

    private void OnCommitButtonClick()
    {
        World.GetExitsLogicManager<LoginLogicManager>().RegisterName(View.InputName.text);
    }
    
}
