using UnityEngine;

namespace ARPG.HealthData
{
    [CreateAssetMenu(fileName = "BaseHealthData", menuName = "Create/Character/BaseHealthData", order = 0)]
    public class CharacterHealthBaseData : ScriptableObject
    {
        //1.处理每一种敌人的初始生命值信息
        //可能有多种敌人 Lv.1 比较低 Lv.2 比较高
        [SerializeField] private float _maxHP;
        [SerializeField] private float _maxStrength;
        
        public float MaxHP => _maxHP;
        public float MaxStrength => _maxStrength;
    }
}