using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public class ComponentPool<T> where T : Component
    {
        public ComponentPool(GameObject go, int size)
        {
            _go = go;
            for (int i = 0; i < size; i++)
                m_readyPool.Add(_go.AddComponent<T>());
        }

        private GameObject _go;
        private List<T> m_usedPool = new List<T>();
        private List<T> m_readyPool = new List<T>();

        public List<T> Used => m_usedPool;

        //******************
        // Source pool
        public T GetItem()
        {
            if (m_readyPool.Count == 0)
                return _go.AddComponent<T>();

            T source = m_readyPool[m_readyPool.Count - 1]; //TODO: what if we need more??!?!?
            m_readyPool.RemoveAt(m_readyPool.Count - 1);
            m_usedPool.Add(source);
            return source;
        }
        public void ReturnItem(T item)
        {
            if (m_readyPool.Remove(item))
                m_readyPool.Add(item);
        }
        public void ReturnItem(int indexOfItem)
        {
            T item = m_usedPool[indexOfItem];
            Helper.Swap(m_usedPool, indexOfItem, m_usedPool.Count - 1);
            m_usedPool.RemoveAt(m_usedPool.Count - 1);
            m_readyPool.Add(item);
        }
    }
}