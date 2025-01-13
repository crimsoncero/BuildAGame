using System.Collections.Generic;
using UnityEngine;

namespace Pool_System
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : PoolGameObject
    {
        private readonly List<T> _totalPool = new();
        private readonly Queue<int> _readyObjectsIndices = new();

        private int _initialPoolSize;
        private T _objectPrefab;
        private Transform _parentTransform;

        protected void InitPool(int initialPoolSize, T objectPrefab, Transform parentTransform = null)
        {
            _initialPoolSize = initialPoolSize;
            _objectPrefab = objectPrefab;
            _parentTransform = parentTransform;
            InitialPopulatePool(initialPoolSize, objectPrefab, parentTransform);
        }

        public T GetReadyObject()
        {
            if (_readyObjectsIndices.Count == 0)
            {
                ExpandPool(_initialPoolSize);
            }

            var readyObjectIndex = _readyObjectsIndices.Dequeue();
            
            var objectToPull = _totalPool[readyObjectIndex];
            return objectToPull;
        }
        
        private void ClearObject(PoolGameObject poolGameObjectToClear)
        {
            var typedObject = poolGameObjectToClear as T;
            if (!_totalPool.Contains(typedObject))
            {
                if (typedObject != null)
                    Debug.LogError($"Trying to clear {typedObject.name} from the pool but it cannot be found!");
                return;
            }
            
            ResetObject(typedObject);

            var indexInPool = _totalPool.IndexOf(typedObject);
            _readyObjectsIndices.Enqueue(indexInPool);
        }

        private static void ResetObject(PoolGameObject poolGameObjectToClear)
        {
            poolGameObjectToClear.ResetSelf();
            poolGameObjectToClear.gameObject.SetActive(false);
        }

        private void InitialPopulatePool(int initialPoolSize, T objectPrefab, Transform parentTransform)
        {
            for (var i = 0; i < initialPoolSize; i++)
            {
                var parent = parentTransform ??= transform;
                var newObject = Instantiate(objectPrefab, parent);
                
                ResetObject(newObject);

                newObject.OnObjectCanBeCleaned += ClearObject;
                
                _totalPool.Add(newObject);
                _readyObjectsIndices.Enqueue(i);
            }
        }
        
        private void ExpandPool(int expandByCount)
        {
            for (var i = 0; i < expandByCount; i++)
            {
                var newObject = Instantiate(_objectPrefab, _parentTransform);
                
                ResetObject(newObject);

                newObject.OnObjectCanBeCleaned += ClearObject;
                
                _totalPool.Add(newObject);
                var indexInPool = _totalPool.IndexOf(newObject);
                _readyObjectsIndices.Enqueue(indexInPool);
            }
        }
    }
}