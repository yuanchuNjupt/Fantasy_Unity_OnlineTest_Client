using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class MainPanelPresenter : BasePresenter<MainPanelView>
{
    public void UpdateHP(int maxHP, int currentHP)
    {
        float x = (float)currentHP / maxHP * 1700;
        float y = 90f;
        View.HP.rectTransform.sizeDelta = new Vector2(x, y);
    }
}
