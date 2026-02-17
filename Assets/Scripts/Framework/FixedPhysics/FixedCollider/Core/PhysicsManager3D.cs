using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Algorithm;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Core
{
    /// <summary>
    /// 3D碰撞对，用于唯一标识两个3D碰撞体之间的碰撞关系
    /// </summary>
    public struct CollisionPair3D : System.IEquatable<CollisionPair3D>
    {
        public readonly FixedIntCollider3D ColliderA;
        public readonly FixedIntCollider3D ColliderB;

        public CollisionPair3D(FixedIntCollider3D a, FixedIntCollider3D b)
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

        public bool Equals(CollisionPair3D other)
        {
            return ColliderA == other.ColliderA && ColliderB == other.ColliderB;
        }

        public override bool Equals(object obj)
        {
            return obj is CollisionPair3D other && Equals(other);
        }

        public override int GetHashCode()
        {
            // 使用异或确保碰撞对的唯一性
            return ColliderA.GetHashCode() ^ ColliderB.GetHashCode();
        }
    }

    public class PhysicsManager3D
    {
        private static PhysicsManager3D _instance = new PhysicsManager3D();

        public static PhysicsManager3D Instance => _instance;
        
        //管理所有3D碰撞体
        private readonly List<FixedIntCollider3D> _collidersList = new List<FixedIntCollider3D>();

        // 记录上一帧的碰撞对，用于判断碰撞生命周期
        // Key: 碰撞对的唯一标识, Value: 是否在上一帧发生碰撞
        private readonly Dictionary<CollisionPair3D, bool> _lastFrameCollisions = new Dictionary<CollisionPair3D, bool>();
        private readonly Dictionary<CollisionPair3D, bool> _currentFrameCollisions = new Dictionary<CollisionPair3D, bool>();

        /// <summary>
        /// 添加3D碰撞体到物理管理器
        /// </summary>
        public void AddCollider3D(FixedIntCollider3D collider)
        {
            if (!_collidersList.Contains(collider))
            {
                _collidersList.Add(collider);
            }
        }

        /// <summary>
        /// 从物理管理器移除3D碰撞体
        /// </summary>
        public void RemoveCollider3D(FixedIntCollider3D collider)
        {
            _collidersList.Remove(collider);
        }

        /// <summary>
        /// 逻辑帧更新，进行碰撞检测
        /// </summary>
        public void OnLogicFrameUpdate()
        {
            DetectAllCollider();
            
            // 处理完当前帧后，更新上一帧的碰撞记录
            UpdateCollisionHistory();
        }

        #region 碰撞检测

        /// <summary>
        /// 检测两个盒体碰撞器的碰撞
        /// </summary>
        public bool DetectCollision(FixedIntBoxCollider boxColliderA, FixedIntBoxCollider boxColliderB,
            bool isUseAdjustPos = false)
        {
            return SATCollisionDetection.DetectCollision(boxColliderA, boxColliderB);
        }

        /// <summary>
        /// 检测球体与盒体碰撞器的碰撞
        /// </summary>
        public bool DetectCollision(FixedIntSphereCollider sphereCollider, FixedIntBoxCollider boxCollider,
            bool isUseAdjustPos = false)
        {
            return AABBCollisionDetection.DetectCollision(sphereCollider, boxCollider);
        }
        
        /// <summary>
        /// 检测盒体与球体碰撞器的碰撞
        /// </summary>
        public bool DetectCollision(FixedIntBoxCollider boxCollider, FixedIntSphereCollider sphereCollider,
            bool isUseAdjustPos = false)
        {
            return AABBCollisionDetection.DetectCollision(boxCollider, sphereCollider);
        }

        /// <summary>
        /// 检测两个球体碰撞器的碰撞
        /// </summary>
        public bool DetectCollision(FixedIntSphereCollider sphereColliderA, FixedIntSphereCollider sphereColliderB,
            bool isUseAdjustPos = false)
        {
            return AABBCollisionDetection.DetectCollision(sphereColliderA, sphereColliderB);
        }

        /// <summary>
        /// 检测所有碰撞体之间的碰撞并触发相应的生命周期回调
        /// </summary>
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
                    if (colliderA is FixedIntBoxCollider boxA && colliderB is FixedIntBoxCollider boxB)
                    {
                        result = DetectCollision(boxA, boxB);
                    }
                    else if (colliderA is FixedIntSphereCollider sphereA &&
                             colliderB is FixedIntSphereCollider sphereB)
                    {
                        result = DetectCollision(sphereA, sphereB);
                    }
                    else if (colliderA is FixedIntSphereCollider sphereCollider &&
                             colliderB is FixedIntBoxCollider boxCollider)
                    {
                        result = DetectCollision(sphereCollider, boxCollider);
                    }
                    else if (colliderA is FixedIntBoxCollider boxCollider2 &&
                             colliderB is FixedIntSphereCollider sphereCollider2)
                    {
                        result = DetectCollision(sphereCollider2, boxCollider2);
                    }

                    // 创建碰撞对
                    var collisionPair = new CollisionPair3D(colliderA, colliderB);
                    
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

        /// <summary>
        /// 根据Unity的BoxCollider创建FixedIntBoxCollider
        /// </summary>
        public FixedIntBoxCollider CreateFixedIntBoxColliderByUnity(BoxCollider uCollider, FixedIntCollider3DType type ,
            bool isManagerCollider = true)
        {
            // 使用隐式转换从 Vector3 转换为 FixedIntVector3
            FixedIntVector3 position = uCollider.transform.position;
            FixedIntVector3 offset = uCollider.center;
            FixedIntVector3 size = uCollider.size;
            FixedIntVector3 rotation = uCollider.transform.eulerAngles;

            var collider = new FixedIntBoxCollider(position, offset, size, rotation, type);
            
            if (isManagerCollider)
            {
                AddCollider3D(collider);
            }

            return collider;
        }

        /// <summary>
        /// 根据Unity的SphereCollider创建FixedIntSphereCollider
        /// </summary>
        public FixedIntSphereCollider CreateFixedIntSphereColliderByUnity(SphereCollider uCollider,
            bool isManagerCollider = true)
        {
            // 使用隐式转换从 Vector3 转换为 FixedIntVector3
            FixedIntVector3 position = uCollider.transform.position;
            FixedIntVector3 offset = uCollider.center;
            var radius = new FixedInt(uCollider.radius);

            var collider = new FixedIntSphereCollider(position, offset, radius);
            
            if (isManagerCollider)
            {
                AddCollider3D(collider);
            }

            return collider;
        }
        
    }
}