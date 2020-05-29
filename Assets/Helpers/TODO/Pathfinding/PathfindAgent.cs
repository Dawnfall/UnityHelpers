using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper.Pathfinding
{
    public class PathfindAgent : MonoBehaviour
    {
        private Rigidbody m_rigidBody;

        protected Vector3 m_walkDestination; //TODO: set some bool for not having desitnation
        protected PathRequest m_pathRequest = null;
        protected Path m_path = null;

        //*************
        // INIT
        //*************
        protected void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }
        protected void Update()
        {
            //TODO: look for path and walk
        }

        public bool IsOnDestination
        {
            get { return Vector3.Distance(transform.position, m_walkDestination) < 0.3f; } //TODO: hardcoded value
        }

        /// <summary>
        /// method that sends a path request to pathfind manager
        /// </summary>
        private void SearchForPath()
        {
            m_pathRequest = new PathRequest(this,transform.position, m_walkDestination);
            PathFindManager.Instance.RequestPath(m_pathRequest);
        }

        /// <summary>
        /// method that tries to walk on path if it exists
        /// </summary>
        private void WalkOnPath()
        {
            Vector3 nextDestination;
            if (m_path != null && m_path.TryMoveToNext(transform.position, out nextDestination))
            {
                Vector3 direction = (nextDestination - transform.position).normalized;
                direction = direction.normalized;

                RaycastHit hitInfo;
                if (Physics.Raycast(new Ray(transform.position + Vector3.up * 50, Vector3.down), out hitInfo, 1000,PathFindManager.Instance.TerrainMask, QueryTriggerInteraction.Ignore))
                    transform.position = hitInfo.point;

                m_rigidBody.velocity = direction * 3; //TODO: hardcoded value
            }
            else
            {
                m_path = null;
                m_rigidBody.velocity = Vector3.zero;
            }
        }

        public bool m_doDrawPath;
        private void OnDrawGizmos()
        {
            if (m_doDrawPath)
            {
                Gizmos.color = Color.red;
                if (m_path != null && m_path.m_path.Count > 0)
                {
                    for (int i = 0; i < m_path.m_path.Count - 1; i++)
                    {
                        Gizmos.DrawLine(m_path.m_path[i], m_path.m_path[i + 1]);
                        Gizmos.DrawCube(m_path.m_path[i], new Vector3(2, 2, 2));
                    }
                    Gizmos.DrawCube(m_path.m_path[m_path.m_path.Count - 1], new Vector3(2, 2, 2));

                }
            }
        }
    }


}
