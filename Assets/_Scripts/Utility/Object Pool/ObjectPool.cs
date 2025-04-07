using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SeraphUtil.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private List<T> _activeObjects;
        private Queue<T> _freeObjects;
        
        private readonly T _prefab;
        private uint _initialCount;
        
        private bool _initialized;
        public Transform Container { get; }
        public int TotalCount { get { return _activeObjects.Count + _freeObjects.Count; } }
        public int ActiveCount { get {return _activeObjects.Count; } }

        public List<T> TotalList
        {
            get
            {
                var l = new List<T>(_activeObjects.Count + _freeObjects.Count);
                l.AddRange(_activeObjects);
                l.AddRange(_freeObjects);
                return l;
            }
        }
        public List<T> ActiveList { get { return _activeObjects; } }
        
        
        
        public ObjectPool(T prefab, Transform container, uint initialCount = 10, bool initPoolNow = true)
        {
            _prefab = prefab;
            Container = container;
            _initialized = false;
            _initialCount = initialCount;
            
            if(initPoolNow)
                InitPool();
        }
        
        /// <summary>
        /// Initializes the objects in the pool, for safety allowed only once unless enabled specifically as it will overwrite the pool.
        /// </summary>
        /// <returns>Whether the initialization happened or not</returns>
        public bool InitPool(bool forceInitialize = false)
        {
            if (_initialized && !forceInitialize)
                return false;
            
            _initialized = true;
            _activeObjects = new List<T>();
            _freeObjects = new Queue<T>();
            
            for (int i = 0; i < _initialCount; i++)
                InstantiateObject();
            
            return true;
        }
        
        public T Take()
        {
            T obj = _freeObjects.Count > 0 ? _freeObjects.Dequeue() : InstantiateObject();

            _activeObjects.Add(obj);
            obj.OnTakeFromPool();
            
            return obj;
        }

        public void Return(T obj)
        {
            obj.OnReturnToPool();
            var returnedObj = _activeObjects.Remove(obj);
            
            if(!returnedObj)
                throw new ArgumentNullException(nameof(obj), "Trying to remove an object that is not in the pool.");
            
            _freeObjects.Enqueue(obj);
        }
        
        private T InstantiateObject()
        {
            T obj = Object.Instantiate(_prefab, Container, false);
            obj.gameObject.SetActive(false);
            _freeObjects.Enqueue(obj);
            return obj;
        }

       
        
        
    }

    
}