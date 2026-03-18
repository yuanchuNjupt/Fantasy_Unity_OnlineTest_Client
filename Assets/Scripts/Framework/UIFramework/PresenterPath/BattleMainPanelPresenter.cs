using System.Collections;
using System.Collections.Generic;
using Framework.AdvancedLog;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class BattleMainPanelPresenter : BasePresenter<BattleMainPanelView>
{
    
    
    public Dictionary<long , EnemyHpRectView> enemyHpRectViews = new Dictionary<long, EnemyHpRectView>();


    private float _initWidth = 650;
    
    
    public void AddEnemyHpRectView(long enemyId , string enemyName)
    {
        if (enemyHpRectViews.ContainsKey(enemyId))
        {
            Debug.LogError($"敌人id {enemyId} 已经存在血条了");
            return;
        }

        var instance = Instantiate(View.enemyHpRectPrefab, View.EnemyHpArea, true);
        var view = instance.GetComponent<EnemyHpRectView>();
        if (view != null)
        {
            view.Init(enemyName);
        }
        
        enemyHpRectViews.Add(enemyId , view);
    }
    
    
    public void UpdatePlayerHp(float curHp , float maxHp)
    {
        var hpPercent = curHp / maxHp;
        
        View.img_HP.rectTransform.sizeDelta = new Vector2(hpPercent * _initWidth , View.img_HP.rectTransform.sizeDelta.y);
    }
    
    public void UpdateEnemyHp(long enemyId , float curHp , float maxHp)
    {
        if (!enemyHpRectViews.TryGetValue(enemyId, out var enemyHpRectView))
        {
            Debug.LogError($"敌人id {enemyId} 不存在血条，无法更新");
            return;
        }
        
        
        Log.Info(LogColor.Green , "更新敌人血量" , $"敌人id:{enemyId},当前血量:{curHp},最大血量:{maxHp}");
        enemyHpRectView.UpdateHp(curHp, maxHp);
    }
    
    
    
    
    
    
    
}
