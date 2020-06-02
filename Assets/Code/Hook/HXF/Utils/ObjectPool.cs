using System.Collections.Generic;
using UnityEngine;

namespace Hook.HXF
{
    public class ObjectPool
    {
        #region Properties

        public int CurrentAvailablePoolSize
        {
            get { return _available.Count; }
        }

        public int CurrentUsedPoolSize
        {
            get { return _used.Count; }
        }
        
        private GameObject _prefab;
        private Transform _parent;
        private Stack<GameObject> _available;
        private Stack<GameObject> _used;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the ObjectPool class
        /// </summary>
        /// <param name="prefab">GameObject prefab that will be pooled</param>
        /// <param name="poolSize">Initial size of the pool</param>
        /// <param name="container">Option parent transform that pool objects will be children of</param>
        public ObjectPool(GameObject prefab, int poolSize, Transform container = null)
        {
            _prefab = prefab;
            _parent = container;
            _available = new Stack<GameObject>();
            _used = new Stack<GameObject>();
            
            UpdatePoolSize(poolSize);
        }

        #endregion
        
        #region Class Methods

        private void UpdatePoolSize(int poolSize)
        {
            GameObject entity;
            for (int i = 0; i < poolSize; i++)
            {
                if (_parent == null)
                {
                    entity = GameObject.Instantiate(_prefab);
                }
                else
                {
                    entity = GameObject.Instantiate(_prefab, _parent);
                }
                
                entity.SetActive(false);
                _available.Push(entity);
            }
            
            Debug.LogFormat("[UpdatePoolSize] Available: {0}", _available.Count);
        }

        public GameObject GetObject()
        {
            GameObject entity;
            
            // checking pool size if entity is available
            if (_available.Count == 0)
            {
                UpdatePoolSize(1);
            }
            
            // remove from available pool
            entity = _available.Pop();
            
            // add to used pool
            _used.Push(entity);
            
            // updating state of entity
            entity.SetActive(true);

            Debug.LogFormat("[GetObject] [ Available: {0}, Used: {1} ]", _available.Count, _used.Count);
            
            return entity;
        }

        public void ReturnAllObjects()
        {
            if (_used.Count == 0)
            {
                Debug.Log("No objects being used that need to be returned.");

                return;
            }

            int totalUsed = _used.Count;
            for (int i = 0; i < totalUsed; i++)
            {
                var entity = _used.Pop();
                _available.Push(entity);
                entity.SetActive(false);
            }
            
            Debug.LogFormat("[ReturnAllObjects] [ Available: {0}, Used: {1} ]", _available.Count, _used.Count);
        }
        
        #endregion
    }
}