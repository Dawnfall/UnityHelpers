using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;



namespace pathfinding
{
    public class PathFindManager : MonoBehaviour
    {
        public static PathFindManager Instance;//TODO: remove this!!!!!!!!!

        public int TerrainMask;//TODO: layermask!

        [Range(1, 8)]
        public int m_threadCount;

        public const float MinPathFindDistance = 0.01f; //TODO: prolly not to be here

        private PathfindGrid m_navGrid;
        private bool m_doRunThreads = false;
        Queue<PathRequest> m_pathRequests = new Queue<PathRequest>();

        private void ThreadWork(object o)
        {
            PathRequest currRequest = null;

            while (m_doRunThreads)
            {
                if (m_pathRequests.Count > 0)
                {
                    lock (m_pathRequests)
                        currRequest = m_pathRequests.Dequeue();

                    FindPath(currRequest);
                }
            }
        }

        /// <summary>
        /// Request a path search by pathrequest
        /// </summary>
        /// <param name="pathRequest"></param>
        public void RequestPath(PathRequest pathRequest)
        {
            lock (m_pathRequests)
                m_pathRequests.Enqueue(pathRequest);
        }

        private void FindPath(PathRequest request)
        {
            Vector2Int start = m_navGrid.WorldToCoords(request.PathStart);
            Vector2Int end = m_navGrid.WorldToCoords(request.PathEnd);

            var comeFromDict = PathfindingAlgorithms.Astar(start, end, m_navGrid);
            var result = PathfindingAlgorithms.RetracePath(m_navGrid, start, end, comeFromDict, false);

            lock(request)
            {
                request.IsSuccess = result.Item1;
                request.Path = result.Item2;
            }
        }

        private void OnEnable() //TODO: clear requests!?
        {
            m_doRunThreads = true;
            for (int i = 0; i < m_threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(ThreadWork);
            }
        }
        private void OnDisable()
        {
            m_doRunThreads = false;
        }


        public void Update()
        {
            UpdateGrid();
        }
        public bool IsGridDirty = true;
        public void UpdateGrid()
        {
            if (IsGridDirty)
            {
                //bounds of levelController;
                //size of tile of levelController


                //TODO: cancel all requests!
                IsGridDirty = false;
            }
        }


    }

    public enum PathFindingAlgorithm
    {
        BREADTH_FIRST,
        DIJKSTRA,
        GREEDY_BEST_FIRST,
        ASTAR
    }
}
//private void FindPath(PathRequest pathRequest)
//{
//    if (m_navGrid == null)
//    {
//        Debug.Log("No pathfinding grid");
//        return;
//    }

//    NavNode startNode = m_navGrid.GetNodeFromWorldPosition(pathRequest.m_pathStart);
//    NavNode endNode = m_navGrid.GetNodeFromWorldPosition(pathRequest.m_pathEnd);
//    Dictionary<NavNode, NavNode> comeFromDict;

//    PathfindingAlgorithms<NavNode>.Astar(startNode, endNode, m_navGrid, out comeFromDict);

//    List<Vector3> path;
//    bool isSuccess;
//    PathfindingAlgorithms<NavNode>.RetracePath(startNode, endNode, comeFromDict, false, out path, out isSuccess);

//    lock (m_pathResults)
//        m_pathResults.Enqueue(new PathResult(pathRequest, new Path(path, isSuccess, pathRequest)));
//}
////****************
//// PARAMETERS
////****************
//public float m_nodeEdge;
//public LayerMask m_unwalkableMask;
//public LayerMask m_walkableMask;













//switch (Algorithm)
//{
//    case PathFindingAlgorithm.ASTAR:
//        PathfindingAlgorithms.Astar(startNode, endNode, mapData, out comeFromDict);
//        break;
//    case PathFindingAlgorithm.BREADTH_FIRST:
//        PathfindingAlgorithms.BreadthFirstSearch(startNode, endNode, mapData, out comeFromDict);
//        break;
//    case PathFindingAlgorithm.DIJKSTRA:
//        PathfindingAlgorithms.Dijkstra(startNode, endNode, mapData, out comeFromDict);
//        break;
//    case PathFindingAlgorithm.GREEDY_BEST_FIRST:
//        PathfindingAlgorithms.GreedyFirst(startNode, endNode, mapData, out comeFromDict);
//        break;
//    default:
//        Debug.Log("Pathfinding algorithm unknown!");
//        break;
//}

//*******************
// Agents
//*******************
//HashSet<PathFindAgent> m_allAgents = new HashSet<PathFindAgent>();
//public void RegisterAgent(PathFindAgent agent)
//{
//    m_allAgents.Add(agent);
//}
//public void UnRegisterAgent(PathFindAgent agent)
//{
//    m_allAgents.Remove(agent);
//}

//*******************
// USE
//*******************
