﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*****************
// this class just handles object by adding/removing them from pool instead of Instatiate/Destroy

namespace Dawnfall.Helper
{


    public class ObjectPool<T> where T : MonoBehaviour
    {
        private T m_prefab = null;
        private List<T> m_objectPool = null;

        public ObjectPool(T prefab, int startPoolSize)
        {
            if (prefab == null)
                throw new System.Exception("null prefab when creating object pool!");
            if (startPoolSize < 0)
                startPoolSize = 0;

            m_prefab = prefab;
            m_prefab.gameObject.SetActive(false);
            m_objectPool = new List<T>();
            InstatiateObjects(startPoolSize);
        }
        public T GetFromPool()
        {
            if (m_objectPool.Count == 0)
                InstatiateObjects(1);

            T objectToReturn = m_objectPool[m_objectPool.Count - 1];
            m_objectPool.RemoveAt(m_objectPool.Count - 1);

            return objectToReturn;
        }
        public void AddToPool(T objectToRemove)
        {
            m_objectPool.Add(objectToRemove);
        }
        private void InstatiateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T newGO = GameObject.Instantiate(m_prefab);
                m_objectPool.Add(newGO);
            }
        }
    }

    public class ObjectPool
    {
        private GameObject m_prefab = null;
        private List<GameObject> m_unusedObjects = null;

        public ObjectPool(GameObject prefab, int startPoolSize)
        {
            if (prefab == null)
                throw new System.Exception("null prefab when creating object pool!");
            m_prefab = prefab;
            m_prefab.SetActive(false);
            m_unusedObjects = new List<GameObject>();
            InstatiateObjects(startPoolSize);
        }
        public GameObject GetFromPool()
        {
            if (m_unusedObjects.Count == 0)
                InstatiateObjects(1);

            GameObject newGO = m_unusedObjects[m_unusedObjects.Count - 1];
            m_unusedObjects.RemoveAt(m_unusedObjects.Count - 1);

            return newGO;
        }
        public void AddToPool(GameObject objectToRemove)
        {
            m_unusedObjects.Add(objectToRemove);
        }
        private void InstatiateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newGO = GameObject.Instantiate(m_prefab);
                m_unusedObjects.Add(newGO);
            }
        }
    }
}