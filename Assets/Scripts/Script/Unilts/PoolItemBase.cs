using System;
using UnityEngine;

namespace Script.Unilts
{
    
    public interface IPoolItem
    {
        void Spawn();
        void Recycle();
    }
    
    public abstract class PoolItemBase : MonoBehaviour , IPoolItem
    {
        private void OnEnable()
        {
            Spawn();
        }
        
        private void OnDisable()
        {
            Recycle();
        }


        public virtual void Spawn()
        {
            
        }

        public virtual void Recycle()
        {
            
        }
    }
}