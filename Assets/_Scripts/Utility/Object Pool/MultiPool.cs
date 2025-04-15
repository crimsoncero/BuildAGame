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
        
        
        // Debug Tracking
        private bool _debug;
        
        
        public MultiPool(List<T> prefabs, Transform container, uint initialCount = 10, bool initPoolNow = true)
        {
            _debug = false;
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
            _debug = false;
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
            _debug = options.Debug;
            if (_debug)
                Application.quitting += LogPools;
            
            
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
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initialCounts[i], options.InitPoolNow, _debug);
                else
                    pool = new ObjectPool<T>(prefab, poolContainer.transform, initPoolNow: options.InitPoolNow, debug: _debug);
                
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
            // In case the prefab doesn't exist in the keys, create a new pool for this prefab, with a small initial size.
            // This is mainly for user-friendliness, but it will log a warning to let the user know they need to implement the prefab in the multipool setup.
            if (!_objectPoolDict.ContainsKey(prefabKey))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"The prefab {prefabKey.name} has not been given as one of the Multi Pool prefabs. A temporary pool was made, make sure to set up the prefabs properly!");           
                GameObject poolContainer = new GameObject();
                poolContainer.transform.SetParent(Container);
                poolContainer.name = prefabKey.name;
                _objectPoolDict.Add(prefabKey, new ObjectPool<T>(prefabKey, Container));
                #else
                Debug.LogError($"The prefab {prefabkKey.name} has not been given as one of the Multi Pool prefabs.");
                return;
                #endif
            }

            return _objectPoolDict[prefabKey].Take();
        }

        public void Return(T prefabKey, T obj)
        {
            if (!_objectPoolDict.ContainsKey(prefabKey))
                throw new ArgumentNullException();

            _objectPoolDict[prefabKey].Return(obj);
        }


        public void LogPools()
        {
            if (!_debug) return;
            string log = "Multi Pool Log:\n";
            foreach (var pool in _objectPoolDict)
            {
                ObjectPool<T>.DebugData data = pool.Value.GetDebugData();
                log += $"The {pool.Key.name} Pool:\n";
                log += $"Total Created: {data.TotalCreated}\n";
                log += $"Runtime Created: {data.RuntimeCreated}\n";
                log += $"Objects Taken: {data.TotalTaken}\n";
                log += $"Objects Returned: {data.TotalReturned}\n\n";
            }
            Debug.Log(log);
        }
    }
}