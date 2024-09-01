using System;
using PlayerNS;

[Serializable]
public class Pool
{
    public PoolObjectType objectType;
    public Poolable prefab;
    public int startSize = 10;
}