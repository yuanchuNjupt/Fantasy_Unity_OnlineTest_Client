
using System;
using System.Collections;
using System.Collections.Generic;
using ARPG.Combat;
using ARPG.ComboData;
using ARPG.ComboData.ComboData;
using ARPG.Health;
using GGG.Tool;
using Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCombatControl : CharacterCombatBase
{
   //1.传入组合技逻辑   普通组合技 / 重击组合技

   
   
   //2.有一个当前组合技
   
   [SerializeField , Header("是否开启攻击结束重置连招") , Space(10)] private bool _resetComboOnEndAttack;
   

   
   //检测方向
   private Vector3 _detectionDirection;
   [SerializeField, Header("攻击检测")] private float _detectionRang;
   [SerializeField] private float _detectionDistance;
   private Collider[] _colliders;
   
   
   

   private void OnEnable()
   {
       GameEventManager.MainInstance.AddListener<bool>("激活处决" , EnableFinishEventHandle);
   }

   private void OnDisable()
   {
       GameEventManager.MainInstance.RemoveListener<bool>("激活处决" , EnableFinishEventHandle);

   }


   private void Start()
   {
       _canAttackInput = true;
       _currentCombo = _baseCombo;
       _canFinish = false;

   }

   protected override void Update()
   {
       // UpdateDetectionDirection();
       
       //不要一直去检测敌人 应该在每次攻击的时候检测一次
       // GetOneEnemy();
       // ClearEnemy();
       CharacterBaseAttack();

       base.Update();

       if (_resetComboOnEndAttack)
       {
           OnEndAttack();
       }
       // CharacterSpecialAttackInput();
       CharacterAssassinationInput();

   }

  


   


   #region 检测
    
   

   private void DetectionTarget()
   {
       if (Physics.SphereCast((transform.position + transform.up * 0.7f), _detectionRang, _detectionDirection,
               out var hit, _detectionDistance, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
       {
           _currentEnemy = hit.collider.transform;
       }
       
   }

   #region 范围检测

   
   //TODO : 暂时弃用
   
   // private void RangeDetection()
   // {
   //     //1.以玩家为中心 取一个半径
   //     //2.在当前玩家没有目标的时候 取一个最近的的敌人
   //     //3.自选
   //     //1.当前有目标 不再更新
   //     //2.只要目标大于一定距离 就更新
   //     
   //     // if(_animator.GetFloat(AnimationID.Movement) > 0.7f) return;
   //     if(_currentEnemy!=null && DevelopmentToos.DistanceForTarget(_currentEnemy , transform) < 1.5f) return;
   //
   //     _colliders = Physics.OverlapSphere((transform.position + transform.up * 0.7f), _detectionRang,
   //         LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
   //
   // }

   //得到一个敌人的函数
   private void GetOneEnemy()
   {
       //进行一次球形检测 获得周围一定范围内的所有敌人
       _colliders = Physics.OverlapSphere((transform.position + transform.up * 0.7f), _detectionRang,
           LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
       //未扫描到敌人 直接返回null
       if(_colliders.Length == 0)
           _currentEnemy = null;
       
       //说明范围内有碰撞器 但不确定是哪个敌人 以及此碰撞器是否有效
       //临时的敌人信息
       Transform temp_enemy = null;
       var distance = Mathf.Infinity;
       foreach (var collider1 in _colliders)
       {
           //这个人已经死了 跳过
           if(collider1.gameObject.GetComponent<Animator>().GetBool(AnimationID.Die))
               continue;
           var dis = DevelopmentToos.DistanceForTarget(collider1.transform, transform);
           if (dis < distance)
           {
               temp_enemy = collider1.transform;
               distance = dis;
               
           }
           
       }
       //遍历完成后 我们理应该得到一个敌人 除非全是死亡的敌人 temp_enmy也是null

       if (!temp_enemy)
       {
           //如果没得到任何有效的敌人
           //不允许处决
           _canFinish = false;
           _currentEnemy = null;
       }
       else
       {
           _currentEnemy = temp_enemy;
           //设置处决属性
           _canFinish = _currentEnemy.gameObject.GetComponent<EnemyHealthControl>()._canBeKilled;

       }
       
       
       
       
       if(_colliders.Length== 0) return;
       
       if(_currentEnemy!=null && DevelopmentToos.DistanceForTarget(_currentEnemy , transform) < 1.5f) return;
       
       if(!_animator.AnimationAtTag("Attack")) return;
       
       // if(_animator.GetFloat(AnimationID.Movement) > 0.7f) return;

       // Transform temp_enemy = null;
       // var distance = Mathf.Infinity;
       // foreach (var collider1 in _colliders)
       // {
       //     if(collider1.gameObject.GetComponent<Animator>().GetBool(AnimationID.Die))
       //         continue;
       //     var dis = DevelopmentToos.DistanceForTarget(collider1.transform, transform);
       //     if (dis < distance)
       //     {
       //         temp_enemy = collider1.transform;
       //         distance = dis;
       //         
       //     }
       //     
       // }
       // _currentEnemy = temp_enemy!= null? temp_enemy : _currentEnemy;
       // //设置处决属性
       // _canFinish = _currentEnemy.gameObject.GetComponent<EnemyHealthControl>()._canBeKilled;

   }


   private void ClearEnemy()
   {
       if(_currentEnemy == null) return;
       if (_animator.GetFloat(AnimationID.Movement) > 0.7f)
       {
           _currentEnemy = null;
       }
   }

   

   #endregion
   
   private void OnDrawGizmos()
   {
       Gizmos.DrawWireSphere(transform.position + (transform.up * 0.7f) + (_detectionDirection * _detectionDistance), _detectionRang);
   }

   #endregion


  
   


   #region 角色的基础攻击

   private bool CanBaseAttackInput()
   {
       //1._canAttackInput = false
       //2.如果角色正在挨打状态，则不能进行攻击
       //3.角色正在格挡
       //4.角色正在处决
       //5.角色正在爬墙
       //6.角色死亡
       if(!_canAttackInput) return false;
       if(_animator.AnimationAtTag("Hit")) return false;
       if(_animator.AnimationAtTag("Parry")) return false;
       if(_animator.AnimationAtTag("KillSkill")) return false;
       if(_animator.AnimationAtTag("Climb")) return false;
       if (_animator.AnimationAtTag("Die")) return false;
       
       return true;
   }

   #region 角色基础攻击输入

   protected override void CharacterBaseAttack()
   {
       if (!CanBaseAttackInput())
       {
           return;
       }

       if (GameInputManager.MainInstance.LAttack)  //这里是正常的输入逻辑
       {
           
           //获取当前敌人
           GetOneEnemy();
           
           //如果满足处决条件 就播放处决动画
           if (CanSpecialAttack() && DevelopmentToos.DistanceForTarget(_currentEnemy , transform) < 1.5f )
           {
               //1.播放对应处决动画
               _finishComboIndex =Random.Range(0 , _finishCombo.TryGetComboMaxCount());
               _animator.Play(_finishCombo.TryGetOneComboAction(_finishComboIndex));
               //2.呼叫事件中心 调用敌人注册的处决事件
               GameEventManager.MainInstance.CallEvent("触发处决" , _finishCombo.TryGetOneHitname(_finishComboIndex , 0) , transform,_currentEnemy);
               EnemyManager.MainInstance.RemoveEnemyUnit(_currentEnemy.gameObject);


               ResetComboInfo(); //?
               // 清空当前敌人信息
                // _currentEnemy = null;
                EnemyManager.MainInstance.StopAllAttackCommand();
           }
           else
           {
               //1.判断当前的组合技是否为空 或者不为基础攻击
               if (_currentCombo == null || _currentCombo != _baseCombo)
                   //放这里 可能提前累计 或不累计 
               {
                   ChangeComboData(_baseCombo);
               }
               Debug.Log("按下左键 开始执行攻击");
               ExecuteComboAttack();
               Debug.Log(_currentComboCount);
           }
           
           

       }
       else if (GameInputManager.MainInstance.RAttack)
       {
           //如果按了右键 
           //如果允许变招 就 按照左键累计的次数变招

           ChangeComboData(_changeHeavyCombo);
           Debug.Log("_currentComboCount:" + _currentComboCount);
           Debug.Log("_currentComboIndex:" + _currentComboIndex);
           switch (_currentComboCount)
           {
               
               case 3:
                   Debug.Log("匹配到3连招");
                   _currentComboIndex = 0;
                   ExecuteComboAttack();
                   _currentComboCount = 0;
                   break;
               case 4:
                   _currentComboIndex = 1;
                   ExecuteComboAttack();
                   _currentComboCount = 0;

                   break;
               case 5:
                   _currentComboIndex = 2;
                   ExecuteComboAttack();
                   _currentComboCount = 0;

                   break;
           }
       }
   }

   #endregion



   

   



  

   
   

   #endregion

  


   #region 处决

   //是否允许执行特殊攻击
   private bool CanSpecialAttack()
   {
       if (_animator.AnimationAtTag("KillSkill") || _animator.AnimationAtTag("BlackKill"))
       {
           return false;
       }

       if (_currentEnemy == null)
       {
           return false;
       }

       // if (_currentComboCount < 2) return false; //没必要
       if (!_canFinish) return false; 
       return true;
   }

    
   //TODO : 暂时弃用
   
   
   // private void CharacterSpecialAttackInput()
   // {
   //     if(!CanSpecialAttack()) return;
   //     if (GameInputManager.MainInstance.Grab && DevelopmentToos.DistanceForTarget(_currentEnemy , transform) < .8f)
   //     {
   //         //1.播放对应处决动画
   //         _finishComboIndex =Random.Range(0 , _finishCombo.TryGetComboMaxCount());
   //         _animator.Play(_finishCombo.TryGetOneComboAction(_finishComboIndex));
   //         //2.呼叫事件中心 调用敌人注册的处决事件
   //         GameEventManager.MainInstance.CallEvent("触发处决" , _finishCombo.TryGetOneHitname(_finishComboIndex , 0) , transform,_currentEnemy);
   //         
   //         // _currentComboIndex = 0;//???
   //
   //         ResetComboInfo(); //?
   //
   //     }
   //         
   // }
   

   #endregion

   #region 暗杀事件

   private bool CanAssassination()
   {
       //1.没有目标 
       //2.距离太远
       //3.角度太大
       //4.现在正在暗杀或处决
       if (_currentEnemy == null)
       {
           return false;
       }

       if (DevelopmentToos.DistanceForTarget(_currentEnemy, transform) > 1.5f)
       {
           return false;
       }

       if (Vector3.Angle(transform.forward , _currentEnemy.forward) > 20f)
       {
           return false;
       }

       if (_animator.AnimationAtTag("BlackKill") || _animator.AnimationAtTag("KillSkill"))
       {
           return false;
       }
       return true;
   }

   private void CharacterAssassinationInput()
   {
       if (!CanAssassination()) return;
       if (GameInputManager.MainInstance.TakeOut)
       {
           //1.播放对应暗杀动画
           _finishComboIndex = Random.Range(0, _assassinationCombo.TryGetComboMaxCount());
           _animator.Play(_assassinationCombo.TryGetOneComboAction(_finishComboIndex) , 0 , 0f);
           //2.呼叫事件中心 调用敌人注册的暗杀事件
           GameEventManager.MainInstance.CallEvent("触发处决", _assassinationCombo.TryGetOneHitname(_finishComboIndex, 0 ),
               transform, _currentEnemy);
           ResetComboInfo();
       }
   }

   #endregion
   
   //事件 
   private void EnableFinishEventHandle(bool apply)
   {
       if(_canFinish) return;
       _canFinish = apply;
   }
   
   
}
