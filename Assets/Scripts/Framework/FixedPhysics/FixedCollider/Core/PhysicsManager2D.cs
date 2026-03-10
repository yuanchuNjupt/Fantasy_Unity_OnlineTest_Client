using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Algorithm;
using FixedPhysics.FixedCollider.Colliders._2D;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Core
{
    /// <summary>
    /// 碰撞对，用于唯一标识两个碰撞体之间的碰撞关系
    /// </summary>
    public struct CollisionPair : System.IEquatable<CollisionPair>
    {
        public readonly FixedIntCollider2D ColliderA;
        public readonly FixedIntCollider2D ColliderB;

        public CollisionPair(FixedIntCollider2D a, FixedIntCollider2D b)
        {
            // 确保较小的引用在前，保证唯一性
            if (a.GetHashCode() < b.GetHashCode())
            {
                ColliderA = a;
                ColliderB = b;
            }
            else
            {
                ColliderA = b;
                ColliderB = a;
            }
        }

        public bool Equals(CollisionPair other)
        {
            return ColliderA == other.ColliderA && ColliderB == other.ColliderB;
        }

        public override bool Equals(object obj)
        {
            return obj is CollisionPair other && Equals(other);
        }

        public override int GetHashCode()
        {
            // 使用异或确保碰撞对的唯一性
            return ColliderA.GetHashCode() ^ ColliderB.GetHashCode();
        }
    }

    public class PhysicsManager2D
    {
        private static PhysicsManager2D _instance = new PhysicsManager2D();

        public static PhysicsManager2D Instance => _instance;

        //管理所有碰撞体
        private readonly List<FixedIntCollider2D> _collidersList = new List<FixedIntCollider2D>();

        // 记录上一帧的碰撞对，用于判断碰撞生命周期
        // Key: 碰撞对的唯一标识, Value: 是否在上一帧发生碰撞
        private readonly Dictionary<CollisionPair, bool> _lastFrameCollisions = new Dictionary<CollisionPair, bool>();
        private readonly Dictionary<CollisionPair, bool> _currentFrameCollisions = new Dictionary<CollisionPair, bool>();


        public void AddCollider2D(FixedIntCollider2D collider)
        {
            _collidersList.Add(collider);
        }

        public void RemoveCollider2D(FixedIntCollider2D collider)
        {
            _collidersList.Remove(collider);
        }

        /// <summary>
        /// 清空所有碰撞体和碰撞历史，用于场景切换/重新进入时重置状态
        /// </summary>
        public void Reset()
        {
            _collidersList.Clear();
            _lastFrameCollisions.Clear();
            _currentFrameCollisions.Clear();
        }

        public void OnLogicFrameUpdate()
        {
            DetectAllCollider();
            
            // 处理完当前帧后，更新上一帧的碰撞记录
            UpdateCollisionHistory();
        }


        #region 碰撞检测

        public bool DetectCollision(FixedIntBoxCollider2D boxColliderA, FixedIntBoxCollider2D boxColliderB,
            bool isUseAdjustPos = false)
        {
            if (boxColliderA.ColliderType is FixedIntCollider2DType.WithRotation ||
                boxColliderB.ColliderType is FixedIntCollider2DType.WithRotation)
            {
                return SATCollisionDetection.DetectCollision(boxColliderA, boxColliderB);
            }
            else
            {
                return AABBCollisionDetection.DetectCollision(boxColliderA, boxColliderB);
            }
            
            
            
        }

        public bool DetectCollision(FixedIntCircleCollider2D circleCollider, FixedIntBoxCollider2D boxCollider,
            bool isUseAdjustPos = false)
        {
            if (boxCollider.ColliderType is FixedIntCollider2DType.WithRotation)
            {
                return  SATCollisionDetection.DetectCollision(circleCollider, boxCollider);
            }
            else
            {
                return AABBCollisionDetection.DetectCollision(circleCollider, boxCollider);
            }
            
        }
        
        public bool DetectCollision(FixedIntBoxCollider2D boxCollider, FixedIntCircleCollider2D circleCollider,
            bool isUseAdjustPos = false)
        {
            return DetectCollision(circleCollider , boxCollider, isUseAdjustPos);
        }
        
        

        public bool DetectCollision(FixedIntCircleCollider2D circleColliderA, FixedIntCircleCollider2D circleColliderB,
            bool isUseAdjustPos = false)
        {
            return AABBCollisionDetection.DetectCollision(circleColliderA, circleColliderB);
        }

        private void DetectAllCollider()
        {
            // 清空当前帧的碰撞记录
            _currentFrameCollisions.Clear();
            
            for (int i = 0; i < _collidersList.Count; i++)
            {
                for (int j = i + 1; j < _collidersList.Count; j++)
                {
                    var colliderA = _collidersList[i];
                    var colliderB = _collidersList[j];
                    bool result = false;

                    // 执行碰撞检测
                    if (colliderA is FixedIntBoxCollider2D boxA && colliderB is FixedIntBoxCollider2D boxB)
                    {
                        result = DetectCollision(boxA, boxB);
                    }
                    else if (colliderA is FixedIntCircleCollider2D sphereA &&
                             colliderB is FixedIntCircleCollider2D sphereB)
                    {
                        result = DetectCollision(sphereA, sphereB);
                    }
                    else if (colliderA is FixedIntCircleCollider2D sphereCollider &&
                             colliderB is FixedIntBoxCollider2D boxCollider)
                    {
                        result = DetectCollision(sphereCollider, boxCollider);
                    }
                    else if (colliderA is FixedIntBoxCollider2D boxCollider2 &&
                             colliderB is FixedIntCircleCollider2D sphereCollider2)
                    {
                        result = DetectCollision(sphereCollider2, boxCollider2);
                    }

                    // 创建碰撞对
                    var collisionPair = new CollisionPair(colliderA, colliderB);
                    
                    if (result)
                    {
                        // 记录当前帧发生了碰撞
                        _currentFrameCollisions[collisionPair] = true;
                        
                        // 检查上一帧是否有碰撞
                        bool wasCollidingLastFrame = _lastFrameCollisions.ContainsKey(collisionPair) && 
                                                      _lastFrameCollisions[collisionPair];
                        
                        if (wasCollidingLastFrame)
                        {
                            // 碰撞保持 - Stay
                            colliderA.TriggerOnCollisionStay(colliderB);
                            colliderB.TriggerOnCollisionStay(colliderA);
                        }
                        else
                        {
                            // 碰撞开始 - Enter
                            colliderA.TriggerOnCollisionEnter(colliderB);
                            colliderB.TriggerOnCollisionEnter(colliderA);
                        }
                    }
                    else
                    {
                        // 当前帧没有碰撞，检查上一帧是否有碰撞
                        bool wasCollidingLastFrame = _lastFrameCollisions.ContainsKey(collisionPair) && 
                                                      _lastFrameCollisions[collisionPair];
                        
                        if (wasCollidingLastFrame)
                        {
                            // 碰撞结束 - Exit
                            colliderA.TriggerOnCollisionExit(colliderB);
                            colliderB.TriggerOnCollisionExit(colliderA);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 更新碰撞历史记录，将当前帧的碰撞状态保存到上一帧
        /// </summary>
        private void UpdateCollisionHistory()
        {
            _lastFrameCollisions.Clear();
            foreach (var pair in _currentFrameCollisions)
            {
                _lastFrameCollisions[pair.Key] = pair.Value;
            }
        }

        #endregion

        public FixedIntBoxCollider2D CreateFixedIntBoxCollider2DByUnity(BoxCollider2D uCollider, FixedIntCollider2DType collider2DType,
            bool isManagerCollider = true)
        {
            var fCollider = new FixedIntBoxCollider2D(uCollider.transform.position, uCollider.offset,
                uCollider.size, 0, collider2DType);

            if (isManagerCollider)
            {
                AddCollider2D(fCollider);
            }

            return fCollider;
        }

        public FixedIntCircleCollider2D CreateFixedIntCircleCollider2DByUnity(CircleCollider2D uCollider,
            bool isManagerCollider = true)
        {
            
            var fCollider = new FixedIntCircleCollider2D(uCollider.transform.position, uCollider.offset,
                new FixedInt(uCollider.radius));

            if (isManagerCollider)
            {
                AddCollider2D(fCollider);
            }
            
            return fCollider;
        }
    }
}