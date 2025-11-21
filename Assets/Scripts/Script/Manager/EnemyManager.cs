using System;
using System.Collections;
using System.Collections.Generic;
using ARPG.Combat;
using ARPG.Movement;
using GGG.Tool.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Manager
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        private Transform _mainPlayer;

        [SerializeField]private List<GameObject> _allEnemies = new List<GameObject>();
        [SerializeField]private List<GameObject> _activeEnemies = new List<GameObject>();
        private Coroutine e; //攻击协程
        private bool _closeattackCommandCoroutine; //外部控制是否关闭攻击协程


        private WaitForSeconds _waitTime;
        
        //停止所有攻击命令
        public void StopAllAttackCommand()
        {
            foreach (var activeEnemy in _activeEnemies)
            {
                EnemyCombatControl _enemyCombatControl;
                if (activeEnemy.TryGetComponent(out _enemyCombatControl))
                {
                    _enemyCombatControl.StopAllAction();
                }
            }

            // StopCoroutine(e);
            //?
        }

        private void OnDestroy()
        {
            StopCoroutine(e);
        }

        protected override void Awake()
        {
            base.Awake();
            
            _mainPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            
            _waitTime = new WaitForSeconds(6f);
        }

        private void Start()
        {
            InitActiveEnemies();
            if(_activeEnemies.Count > 0)
                _closeattackCommandCoroutine = false;
            e =  StartCoroutine(EnableEnemyUnitAttackCommand());
            
        }


        public void AddEnemyUnit(GameObject enemy)
        {
            if (!_allEnemies.Contains(enemy))
            {
                _allEnemies.Add(enemy);
            }
        }

        public void RemoveEnemyUnit(GameObject enemy)
        {
            if (_activeEnemies.Contains(enemy))
            {
                EnemyMovementControl em;
                if (enemy.TryGetComponent(out em))
                {
                    em.EnableCharacterController(false);
                }
                _activeEnemies.Remove(enemy);
            }
        }
        
        
        
        
        
        
        
        public Transform GetMainPlayerTransform() => _mainPlayer;

        IEnumerator EnableEnemyUnitAttackCommand()
        {
            if(_activeEnemies == null)
                yield break;
            if(_activeEnemies.Count == 0)
                yield break;
            while (_activeEnemies.Count > 0)
            {
                if (_closeattackCommandCoroutine)
                    yield break;
                var index = Random.Range(0, _activeEnemies.Count);

                if (index < _activeEnemies.Count)
                {
                    EnemyCombatControl _enemyCombatControl;
                    GameObject temp = _activeEnemies[index];
                    if (temp.TryGetComponent(out  _enemyCombatControl))
                    {
                        _enemyCombatControl.SetAttackCommand(true);
                    }
                }
               
                

                yield return _waitTime;

            }
            yield break;
        }


        private void InitActiveEnemies()
        {
            foreach (var allEnemy in _allEnemies)
            {
                if (allEnemy.activeSelf)
                {
                    EnemyMovementControl em;
                    if (allEnemy.TryGetComponent(out em))
                    {
                        em.EnableCharacterController(true);
                    }
                    else
                    {
                        Debug.Log("未找到 EnemyMovementControl 组件");
                    }
                    _activeEnemies.Add(allEnemy);
                }
            }
        }

        public void CloseAttackCoroutine()
        {
            _closeattackCommandCoroutine = true;
            StopCoroutine(e);
        }
        
    }
}