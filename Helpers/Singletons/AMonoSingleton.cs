using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public abstract class AMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = GameObject.FindObjectOfType<T>();
                return m_instance;
            }
        }

        private void Awake()
        {
            if (Instance != this)
            {
#if UNITY_EDITOR
                DestroyImmediate(this);
#else
            Destroy(this);
#endif
                return;
            }

            OnAfterAwake();
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                OnBeforeDestroy();
                m_instance = null;
            }
        }

        protected virtual void OnAfterAwake() { }
        protected virtual void OnBeforeDestroy() { }

    }
}