using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class MonoPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private int initialPoolSize = 5;
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;
        private Stack<T> _available;
        private List<T> _active;

        protected virtual void Awake()
        {
            _available = new Stack<T>();
            _active = new List<T>();
            AddItemsToPool(initialPoolSize);
        }
    
        public T Get()
        {
            if (_available.Count == 0)
            {
                AddItemsToPool(initialPoolSize);
            }

            var obj = _available.Pop();

            if (obj == null)
            {
                return null;
            }

            obj.gameObject.SetActive(true);
            obj.Reset();
            _active.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _available.Push(obj);
            _active.Remove(obj);
        }
    
        private void AddItemsToPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (prefab == null)
                {
                    continue;
                }
                var obj = Instantiate(prefab, parent, true);
                if (obj == null)
                {
                    continue;
                }
                obj.gameObject.SetActive(false);
                _available.Push(obj);
            }
        }
        public List<T> GetAllActiveObjects()
        {
            return new List<T>(_active); 
        }
    }
}