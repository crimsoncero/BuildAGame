using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeraphUtil.ObjectPool
{
    public class MultiPool<T> where T : MonoBehaviour, IPoolable
    {
        private Dictionary<T, ObjectPool<T>> _objectPoolDict;

        private bool _initialized;
        
        public ObjectPool<T> this[T prefab] { get { return _objectPoolDict[prefab]; } }

        public int TotalCount
        {
            get
            {
                int count = 0;
                foreach (var pool in _objectPoolDict.Values)
                    count += pool.TotalCount;
                return count;
            }
        }
        public int ActiveCount
        {
            get
            {
                int count = 0;
                foreach (var pool in _objectPoolDict.Values)
                    count += pool.ActiveCount;
                return count;
            }
        }
        public List<T> TotalList
        {
            get
            {
                var list = new List<T>(TotalCount);
                foreach(var pool in _objectPoolDict.Values)
                    list.AddRange(pool.TotalList);
                return list;
            }
        }
        public List<T> ActiveList 
        {
            get 
            {
                var list = new List<T>(ActiveCount);
                foreach(var pool in _objectPoolDict.Values)
                    list.AddRange(pool.ActiveList);
                return list;
            } 
        }
        
        public Transform Container { get; }

        public MultiPool(List<T> prefabs, Transform container, uint initialCount = 10, bool initPoolNow = true)
        {
            _objectPoolDict = new Dictionary<T, ObjectPool<T>>();
            Container = container;
            
            foreach (var prefab in prefabs)
            {
                GameObject poolContainer = new GameObject();
                poolContainer.transform.SetParent(Container);
                poolContainer.name = prefab.name;
                
                var pool = new ObjectPool<T>(prefab, poolContainer.transform, initialCount, initPoolNow);
                _objectPoolDict.Add(prefab, pool);
            }
            
        }
        
        public MultiPool(List<T> prefabs, List<uint> initialCounts, Transform container, bool initPoolNow = true)
        {
            _objectPoolDict = new Dictionary<T, ObjectPool<T>>();
            Container = container;
            
            for (int i = 0; i < prefabs.Count; i++)
            {
                T prefab = prefabs[i];
                ObjectPool<T> pool;
                   
                GameObject poolContainer = new GameObject();
                poolContainer.transform.SetParent(Container);
                poolContainer.name = prefab.name;
                
                if (initialCounts.Count < i)
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initialCounts[i], initPoolNow);
                else
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initPoolNow: initPoolNow);
                
                _objectPoolDict.Add(prefab, pool);
            }
        }

        public MultiPool(MultiPoolOptions<T> options, Transform container)
        {
            List<T> prefabs = options.PoolList.Select(o => o.Prefab).ToList();
            List<uint> initialCounts = options.PoolList.Select(o => o.InitialCount).ToList();
           
            _objectPoolDict = new Dictionary<T, ObjectPool<T>>();
            Container = container;

            for (int i = 0; i < prefabs.Count; i++)
            {
                T prefab = prefabs[i];
                ObjectPool<T> pool;
                
                GameObject poolContainer = new GameObject();
                poolContainer.transform.SetParent(Container);
                poolContainer.name = prefab.name;
                
                if (i < initialCounts.Count)
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initialCounts[i], options.InitPoolNow);
                else
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initPoolNow: options.InitPoolNow);
                
                _objectPoolDict.Add(prefab, pool);
            }
        }
        
        /// <summary>
        /// Initializes the objects in the pool, for safety allowed only once unless enabled specifically as it will overwrite the pool.
        /// </summary>
        /// <returns>Whether the initialization happened or not</returns>
        public bool InitPools(bool forceInitialize = false)
        {
            if (_initialized && !forceInitialize)
                return false;
            
            _initialized = true;
            foreach (var pool in _objectPoolDict.Values)
            {
                pool.InitPool(forceInitialize);
            }

            return true;
        }
        
        public T Take(T prefabKey)
        {
            if (!_objectPoolDict.ContainsKey(prefabKey))
                throw new ArgumentNullException();

            return _objectPoolDict[prefabKey].Take();
        }

        public void Return(T prefabKey, T obj)
        {
            if (!_objectPoolDict.ContainsKey(prefabKey))
                throw new ArgumentNullException();

            _objectPoolDict[prefabKey].Return(obj);
        }
        
        



    }
}