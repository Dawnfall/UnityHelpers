using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper.Pathfinding
{
    public class Path
    {
        public PathRequest m_originalRequest;

        public List<Vector3> m_path;
        bool m_isSuccess;

        public Path(List<Vector3> path, bool isSuccess, PathRequest originalRequest)
        {
            m_path = path;
            m_isSuccess = isSuccess;
            m_originalRequest = originalRequest;
        }

        public bool TryMoveToNext(Vector3 currentPosition, out Vector3 nextDestination)
        {
            nextDestination = new Vector3();
            for (int i = m_path.Count - 1; i >= 0; i--)
            {
                nextDestination = m_path[i];
                if (Vector3.Distance(currentPosition, nextDestination) > 0.1f)
                    return true;
                m_path.RemoveAt(m_path.Count - 1);
            }
            return false;
        }

        public bool IsSuccess
        {
            get { return m_isSuccess; }
        }
    }
}