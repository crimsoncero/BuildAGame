using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeraphUtil.ObjectPool
{
    public class MultiPoolOptions<T> : ScriptableObject where T : MonoBehaviour, IPoolable
    {
        [Serializable]
        public class PoolOptions
        {
            public T Prefab;
            public uint InitialCount;
        }

        [field: SerializeField]
        public List<PoolOptions> PoolList { get; protected set; }
        [field: SerializeField]
        public bool InitPoolNow { get; protected set; } = true;
    }
}