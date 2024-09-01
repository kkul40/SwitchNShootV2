using System.Collections.Generic;
using PlayerNS;
using UnityEngine;

public class ObjectPoolar : MonoBehaviour
{
    public static ObjectPoolar instance;
    [SerializeField] public Pool[] pools;
    private Transform mTransform;
    private Dictionary<PoolObjectType, Queue<Poolable>> poolDictionary;

    private void Awake()
    {
        instance = this;
        mTransform = transform;
        poolDictionary = new Dictionary<PoolObjectType, Queue<Poolable>>();
        foreach (var pool in pools)
        {
            // Queue<Poolable> queue
        }
    }

    private void AddObjectsToPool()
    {
        
    }
}