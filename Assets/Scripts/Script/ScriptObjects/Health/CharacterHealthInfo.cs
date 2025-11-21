using UnityEngine;
using UnityEngine.Serialization;

namespace ARPG.HealthData
{
    [CreateAssetMenu(fileName = "HealthInfo", menuName = "Create/Character/HealthInfo", order = 0)]
    public class CharacterHealthInfo : ScriptableObject
    {
        //1.最大生命值
        //2.最大体力值
        //3.当前生命值
        //4.当前体力值
        //5.是否死亡
        //6.体力是否耗尽
        private float _currentHealth;
        private float _currentStrength;
        private float _maxHealth;
        private float _maxStrength;
        private bool _isDead => _currentHealth <= 0; //死亡判断
        private bool _isHaveStrength;
        
        
        public bool IsDead => _isDead;
        public bool IsHaveStrength => _isHaveStrength;
        public float CurrentHealth => _currentHealth;
        public float CurrentStrength => _currentStrength;
        public float MaxHealth => _maxHealth;
        public float MaxStrength => _maxStrength;

        [FormerlySerializedAs("_characterHealthBase")] [SerializeField] private CharacterHealthBaseData characterHealthBaseData;
        
        
        //初始化的时候赋值

        public void Init()
        {
            _maxHealth = characterHealthBaseData.MaxHP;
            _maxStrength = characterHealthBaseData.MaxStrength;
            _currentHealth = _maxHealth;
            _currentStrength = _maxStrength;
            _isHaveStrength = true;
        }


        public void Damage(float damage)
        {
            //1.如果体力充沛 扣除体力值
            //2.敌人现在正在攻击动作中 还没打到玩家 而被玩家打到了 全都扣除
 
            // if (_isHaveStrength)
            // {
            //     _currentStrength = Clmap(_currentStrength , damage , 0 , _maxStrength, false);
            //
            //     if (_currentStrength <= 0)
            //     {
            //         _isHaveStrength = false;
            //     }
            // }
            _currentHealth = Clmap(_currentHealth , damage , 0 , _maxHealth, false);
           
            
        }

        public void DamageToStrenght(float damage)
        {
            if (_isHaveStrength)
            {
                _currentStrength = Clmap(_currentStrength , damage , 0 , _maxStrength, false);

                if (_currentStrength <= 0)
                {
                    _isHaveStrength = false;
                }
            }
        }

        private float Clmap(float value,float offsetValue ,float min, float max, bool Add = false)
        {
            return Mathf.Clamp(Add ? value + offsetValue : value - offsetValue, min, max);
        }

        public void AddHp(float value)
        {
            _currentHealth = Clmap(_currentHealth , value , 0 , _maxHealth, true);

        } 
        
        public void AddStrength(float value)
        {
            _currentStrength = Clmap(_currentStrength , value , 0 , _maxStrength, true);
            if(_currentStrength >= _maxStrength)
                _isHaveStrength = true;
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}