using UnityEngine;

namespace PlayerNS
{
    public class Poolable : MonoBehaviour
    {
        [SerializeField] private PoolObjectType _poolObjectType;
        protected ObjectPoolar pool;
        private bool spawned;

        public void SetObjectPool(ObjectPoolar op)
        {
            pool = op;
        }

        public virtual void OnSpawned()
        {
            spawned = true;
        }
        
        public virtual void OnDespawned()
        {
            spawned = false;
        }
        
        // public virtual void ReturnToPool()
        // {
        //     pool.DespawnObjectToPool(this);
        // }
        //
        // public PoolObjectType GetPoolObjectType()
        // {
        //     return poolObjectType;
        // }
        
    }
}