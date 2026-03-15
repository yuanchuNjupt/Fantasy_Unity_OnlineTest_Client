//此文件由UIViewTemplate自动生成，任何手动修改将会被下一次生成覆盖，若需手动修改请避免自动生成
//Author : 原初z

using UnityEngine;
using UnityEngine.UI;
using UIFramework.Core;
using TMPro;

namespace UIFramework.ViewPath
{
	public class EnemyHpRectView : BaseUIPanelView
	{
		[Header("可绑定组件")] public TextMeshProUGUI tmp_Name;
		public Image img_Hp;

		private float _initWidth;

		public void Init(string playerName)
		{
			tmp_Name.text = playerName;
			_initWidth = img_Hp.rectTransform.rect.width;
		}

		public void UpdateHp(float currentHp, float maxHp)
		{
			float fillAmount = currentHp / maxHp;
			img_Hp.rectTransform.sizeDelta = new Vector2(_initWidth * fillAmount, img_Hp.rectTransform.sizeDelta.y);
		}
	}
}
