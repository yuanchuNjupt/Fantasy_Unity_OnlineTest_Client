using UnityEngine;


namespace ARPG.ComboData
{
    [CreateAssetMenu(fileName = "ComboData", menuName = "Create/Character/ComboData", order = 0)]
    public class CharacterComboDataSO : ScriptableObject
    {
        //招式名称(某个动画片段的名字) , 要播放某个动画 只需要去取得某个动作的动作名就可以了
        [SerializeField] private string _comboName;
        [SerializeField] private float _damage;
        [SerializeField] private string[] _comboHitName;  //连击动画的名字
        [SerializeField] private string[] _comboParryName;  //格挡动画
        [SerializeField] private float _comboTime;  //衔接下一段攻击的间隔时间
        [SerializeField] private float _comboPositionOffest;  //让这段攻击与目标之间保持最佳距离
        
        
        
        public string ComboName { get { return _comboName; } }
        public float Damage { get { return _damage; } }
        public string[] ComboHitName { get { return _comboHitName; } }
        public string[] ComboParryName { get { return _comboParryName; } }
        public float ComboTime { get { return _comboTime; } }
        public float ComboPositionOffest { get { return _comboPositionOffest; } }
        
        //获取当前动作最大的受伤数
        public int GetHitAndParryMaxCount() => _comboHitName.Length;
    }
}
