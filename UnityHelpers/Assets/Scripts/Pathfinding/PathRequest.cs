using UnityEngine;
using System;
using System.Collections.Generic;

namespace pathfinding
{
    public class PathRequest
    {
        public PathfindAgent PathFindAgent
        {
            get; private set;
        }
        public Vector3 PathStart
        {
            get; private set;
        }
        public Vector3 PathEnd
        {
            get; private set;
        }

        public List<Vector3> Path
        {
            get; set;
        }
        public bool IsSuccess
        {
            get; set;
        }
        public bool IsValid { get; set; }

        public PathRequest(PathfindAgent agent, Vector3 pathStart, Vector3 pathEnd)
        {
            PathFindAgent = agent;
            PathStart = pathStart;
            PathEnd = pathEnd;

            IsValid = true;
        }
    }
}
//public enum PathRequestStatus
//{
//    FAILURE,
//    SUCCESS,
//    PENDING,
//    CANCELED
//}
//private PathRequestStatus m_status;
//public PathRequestStatus Status
//{
//    get { return m_status; }
//}
//private TestUnit m_unit;
//public TestUnit Unit
//{
//    get { return m_unit; }
//}