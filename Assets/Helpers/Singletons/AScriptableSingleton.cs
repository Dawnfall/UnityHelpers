using UnityEngine;

namespace Dawnfall.Helper
{
    public abstract class AScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        protected static T m_instance = null;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var allInst = Resources.FindObjectsOfTypeAll<T>();  //TODO: more general, Resources?!?
                    if (allInst.Length > 0)
                        m_instance = allInst[0];
                    if (m_instance == null)
                        m_instance = CreateInstance<T>();
                }

                return m_instance;
            }
        }
    }
}