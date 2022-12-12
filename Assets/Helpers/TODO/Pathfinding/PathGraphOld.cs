//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////TODO: needs further work....
////just for refference ;  meant to be deleted
//namespace pathfinding
//{
//    public class PathGraph : MonoBehaviour
//    {
//        public int NodeCount;

//        public Path GeneratePath(Vector3 start, Vector3 end, PathFindingAlgorithm algorithm)
//        {
//            //TODO: detect node
//            GraphNode fromNode = null;
//            GraphNode toNode = null;

//            List<GraphNode> path = null;
//            switch (algorithm)
//            {
//                case PathFindingAlgorithm.ASTAR:
//                    path = PathfindingAlgorithms.Astar(fromNode, toNode, this);
//                    break;
//                case PathFindingAlgorithm.GREEDY_BEST_FIRST:
//                    path = PathfindingAlgorithms.GreedyFirst(fromNode, toNode, this);
//                    break;
//                case PathFindingAlgorithm.DIJKSTRA:
//                    path = PathfindingAlgorithms.Dijkstra(fromNode, toNode, this);
//                    break;
//                case PathFindingAlgorithm.BREADTH_FIRST:
//                    path = PathfindingAlgorithms.BreadthFirstSearch(fromNode, toNode, this);
//                    break;
//            }
//            if (path == null)
//                return new Path(null, false);

//            List<Vector3> waypoints = new List<Vector3>();
//            foreach (GraphNode node in path)
//                waypoints.Add(node.worldPos);

//            return new Path(waypoints, true);
//        }
//    }

//    public class GraphNode
//    {
//        public Vector3 worldPos;
//        public List<GraphConnection> connections;
//        public bool walkable;

//        public static float GetDistance(GraphNode a,GraphNode b)
//        {
//            return Vector3.Distance(a.worldPos, b.worldPos);
//        }
//    }
//    public class GraphConnection
//    {
//        public GraphNode from;
//        public GraphNode to;


//    }
//}