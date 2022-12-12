//using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine;
//using helper;


//// TODO: just for reference ; meant to be deleted

//namespace pathfinding
//{
//    public static class PathfindingAlgorithms
//    {
//        public class PathNode
//        {
//            public GraphConnection fromConnection;
//        }

//        public class DijkstraNode : PathNode, IComparable<DijkstraNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Gcost { get; set; }

//            public int CompareTo(DijkstraNode other)
//            {
//                return -Gcost.CompareTo(other.Gcost);
//            }
//        }

//        public class GreedyNode : PathNode, IComparable<GreedyNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Hcost { get; set; }
//            public int CompareTo(GreedyNode other)
//            {
//                return -Hcost.CompareTo(other.Hcost);
//            }
//        }

//        public class AStarNode : PathNode, IComparable<AStarNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Gcost { get; set; }
//            public float Hcost { get; set; }
//            public float Fcost { get { return Gcost + Hcost; } }

//            public int CompareTo(AStarNode other)
//            {
//                int compare = Fcost.CompareTo(other.Fcost);
//                if (compare == 0)
//                    compare = Gcost.CompareTo(other.Gcost);
//                return -compare;
//            }
//        }

//        //**********************
//        // BREADTH FIRST SEARCH
//        //**********************
//        // - guaranties shortest path
//        // - assumes all edges are of equal cost(uniform heuristic)
//        // - best for 1:n or n:1

//        public static List<GraphNode> BreadthFirstSearch(GraphNode start, GraphNode end, PathGraph graph)
//        {
//            Queue<PathNode> openSet = new Queue<PathNode>();
//            HashSet<GraphNode> visitedNodes = new HashSet<GraphNode>();

//            PathNode startBreadthNode = new PathNode { fromConnection = new GraphConnection { from = null, to = start } };
//            visitedNodes.Add(start);
//            openSet.Enqueue(startBreadthNode);

//            PathNode currentBreadthNode = null;
//            while (openSet.Count > 0)
//            {
//                currentBreadthNode = openSet.Dequeue();

//                if (currentBreadthNode.fromConnection.to.Equals(end))
//                    break;

//                foreach (GraphConnection connection in currentBreadthNode.fromConnection.to.connections)
//                {
//                    if (!connection.to.walkable || visitedNodes.Contains(connection.to))
//                        continue;

//                    PathNode neighbourPathNode = new PathNode { fromConnection = connection };
//                    visitedNodes.Add(connection.to);
//                    openSet.Enqueue(neighbourPathNode);

//                }
//            }
//            if (currentBreadthNode == null || currentBreadthNode.fromConnection.to != end)
//                return null;

//            return Retrace(currentBreadthNode, start);
//        }

//        //***************************
//        // GREEDY FIRST SEARCH
//        //***************************
//        // - does NOT guarantee shortest path
//        // - generally very fast
//        // - works for 1:1 
//        public static List<GraphNode> GreedyFirst(GraphNode start, GraphNode end, PathGraph graph) //needs a bit of checking
//        {
//            Heap<GreedyNode> openSet = new Heap<GreedyNode>(graph.NodeCount);
//            Dictionary<GraphNode, GreedyNode> allNodesDict = new Dictionary<GraphNode, GreedyNode>();

//            GreedyNode newGreedyNode = new GreedyNode
//            {
//                fromConnection = new GraphConnection { from = null, to = start },
//                Hcost = GraphNode.GetDistance(start, end)
//            };
//            openSet.Add(newGreedyNode);
//            allNodesDict[start] = newGreedyNode;

//            //until there are no more unchecked nodes...
//            GreedyNode currGreedyNode = null;
//            while (openSet.Count > 0)
//            {
//                currGreedyNode = openSet.Extract();

//                //if we reached end node we are done
//                if (currGreedyNode.fromConnection.to.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (GraphConnection connection in currGreedyNode.fromConnection.to.connections)
//                {
//                    if (!connection.to.walkable)
//                        continue;

//                    GreedyNode neighbourGreedyNode;
//                    if (!allNodesDict.TryGetValue(connection.to, out neighbourGreedyNode))
//                    {
//                        neighbourGreedyNode = new GreedyNode { fromConnection = connection, Hcost = GraphNode.GetDistance(connection.to, end) };
//                        openSet.Add(neighbourGreedyNode);
//                        allNodesDict.Add(connection.to, neighbourGreedyNode);
//                    }
//                }
//                currGreedyNode.HeapIndex = -2;
//            }
//            if (currGreedyNode == null || currGreedyNode.fromConnection.to != end)
//                return null;

//            return Retrace(currGreedyNode, start);
//        }

//        public static List<GraphNode> Dijkstra(GraphNode start, GraphNode end, PathGraph grid)
//        {
//            Heap<DijkstraNode> openSet = new Heap<DijkstraNode>(grid.NodeCount);
//            Dictionary<GraphNode, DijkstraNode> allNodeDict = new Dictionary<GraphNode, DijkstraNode>();

//            DijkstraNode newDijkstraNode = new DijkstraNode
//            {
//                fromConnection = new GraphConnection { from = null, to = start },
//                Gcost = 0
//            };
//            openSet.Add(newDijkstraNode);
//            allNodeDict[start] = newDijkstraNode;

//            DijkstraNode currDijkstra = null;
//            //until there are no more unchecked nodes...
//            while (openSet.Count > 0)
//            {
//                //we take best (Fcost or if equal Hcost) node on the heap
//                currDijkstra = openSet.Extract();

//                //if we reached end node we are done
//                if (currDijkstra.fromConnection.to.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (GraphConnection connection in currDijkstra.fromConnection.to.connections)
//                {
//                    if (!connection.to.walkable)
//                        continue;

//                    float newCostToNeighbour = currDijkstra.Gcost + GraphNode.GetDistance(currDijkstra.fromConnection.to, connection.to);

//                    DijkstraNode neighbourDijkstra;
//                    if (allNodeDict.TryGetValue(connection.to, out neighbourDijkstra))
//                    {
//                        if (neighbourDijkstra.HeapIndex > 0 && neighbourDijkstra.Gcost > newCostToNeighbour) //is on the openSet
//                        {
//                            neighbourDijkstra.fromConnection.from = currDijkstra.fromConnection.to;
//                            neighbourDijkstra.Gcost = newCostToNeighbour;
//                            openSet.Update(neighbourDijkstra);
//                        }
//                    }
//                    else
//                    {
//                        neighbourDijkstra = new DijkstraNode { fromConnection = connection, Gcost = newCostToNeighbour };
//                        allNodeDict[connection.to] = neighbourDijkstra;
//                        openSet.Add(neighbourDijkstra);
//                    }
//                }
//                currDijkstra.HeapIndex = -2; //closed set
//            }

//            if (currDijkstra == null || currDijkstra.fromConnection.to != end)
//                return null;

//            return Retrace(currDijkstra, start);
//        }

//        public static List<GraphNode> Astar(GraphNode start, GraphNode end, PathGraph graph)
//        {
//            Heap<AStarNode> openSet = new Heap<AStarNode>(graph.NodeCount);
//            Dictionary<GraphNode, AStarNode> allNodeDict = new Dictionary<GraphNode, AStarNode>();

//            AStarNode newAstarNode = new AStarNode {
//                fromConnection=new GraphConnection { from = null, to = start },
//                Gcost = 0, 
//                Hcost = GraphNode.GetDistance(start, end)
//            };
//            openSet.Add(newAstarNode);
//            allNodeDict.Add(start, newAstarNode);

//            AStarNode currAstarNode = null;
//            while (openSet.Count > 0)
//            {
//                currAstarNode = openSet.Extract();
//                if (currAstarNode.fromConnection.to.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (GraphConnection connection in currAstarNode.fromConnection.to.connections)
//                {
//                    if (!connection.to.walkable)
//                        continue;

//                    float newCostToNeighbour = currAstarNode.Gcost + GraphNode.GetDistance(currAstarNode.fromConnection.to,connection.to);

//                    AStarNode neighbourAstar;
//                    if (allNodeDict.TryGetValue(connection.to, out neighbourAstar))
//                    {
//                        if (neighbourAstar.HeapIndex > 0 && neighbourAstar.Gcost > newCostToNeighbour)
//                        {
//                            neighbourAstar.fromConnection.from = currAstarNode.fromConnection.to;
//                            neighbourAstar.Gcost = newCostToNeighbour;
//                            openSet.Update(neighbourAstar);
//                        }
//                    }
//                    else
//                    {
//                        neighbourAstar = new AStarNode {
//                            fromConnection=connection, 
//                            Gcost = newCostToNeighbour,
//                            Hcost = GraphNode.GetDistance(connection.to, end) 
//                        };
//                        allNodeDict.Add(connection.to, neighbourAstar);
//                        openSet.Add(neighbourAstar);
//                    }

//                }
//                currAstarNode.HeapIndex = -2;
//            }
//            if (currAstarNode == null || currAstarNode.fromConnection.to != end)
//                return null;

//            return Retrace(currAstarNode, start);
//        }

//        private static List<GraphNode> Retrace(PathNode endNode, GraphNode start) //can be optimized
//        {
//            return null;
//            //List<GraphNode> path = new List<GraphNode>();
//            //while (endNode.fromConnection.to != start)
//            //{
//            //    path.Add(endNode.fromConnection.to); 
//            //    endNode = endNode.fromConnection.from;
//            //}

//            //return path;
//        }



















//        //**********************
//        // BREADTH FIRST SEARCH
//        //**********************
//        // - guaranties shortest path
//        // - assumes all edges are of equal cost(uniform heuristic)
//        // - best for 1:n or n:1

//        //public static Dictionary<T, T> BreadthFirstSearch(T startNode, T endNode, INavigatable<T> graph)
//        //{
//        //    Queue<T> openSet = new Queue<T>();
//        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();

//        //    openSet.Enqueue(startNode);
//        //    comeFromDict.Add(startNode, default(T));

//        //    while (openSet.Count > 0)
//        //    {
//        //        T currNode = openSet.Dequeue();

//        //        if (currNode.Equals(endNode))
//        //            return comeFromDict;

//        //        foreach (T neighbour in graph.GetNeighbours(currNode))
//        //            if (!comeFromDict.ContainsKey(neighbour))
//        //            {
//        //                openSet.Enqueue(neighbour);
//        //                comeFromDict.Add(neighbour, currNode);
//        //            }
//        //    }
//        //    return comeFromDict;
//        //}

//        //*****************************
//        // DIJKSTRA (uniform cost search)
//        //*****************************
//        // - guaranties shortest path
//        // - can deal with diferent weights
//        // - best for 1:n or n:1

//        //public static Dictionary<T, T> Dijkstra(T startNode, T endNode, INavigatable<T> graph)
//        //{
//        //    Heap<DijkstraNode<T>> openSet = new Heap<DijkstraNode<T>>(graph.NodeCount);
//        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();
//        //    Dictionary<T, DijkstraNode<T>> allDijkstraNodes = new Dictionary<T, DijkstraNode<T>>();

//        //    DijkstraNode<T> newDijkstraNode = new DijkstraNode<T>(startNode, 0);
//        //    openSet.Add(newDijkstraNode);
//        //    allDijkstraNodes[startNode] = newDijkstraNode;

//        //    ////until there are no more unchecked nodes...
//        //    while (openSet.Count > 0)
//        //    {
//        //        //we take best (Fcost or if equal Hcost) node on the heap
//        //        DijkstraNode<T> currDijkstra = openSet.Extract();
//        //        T currNode = currDijkstra.m_item;

//        //        //if we reached end node we are done
//        //        if (currNode.Equals(endNode))
//        //            return comeFromDict;

//        //        //we check all its neighbours
//        //        foreach (T neighbour in graph.GetNeighbours(currNode))
//        //        {
//        //            float newCostToNeighbour = currDijkstra.Gcost + graph.GetDistance(currNode, neighbour);

//        //            if (!comeFromDict.ContainsKey(neighbour))
//        //            {
//        //                DijkstraNode<T> neighbourDijkstra = new DijkstraNode<T>(neighbour, newCostToNeighbour);

//        //                openSet.Add(neighbourDijkstra);
//        //                comeFromDict.Add(neighbour, currNode);
//        //                allDijkstraNodes[neighbour] = neighbourDijkstra;
//        //            }

//        //            else if (newCostToNeighbour < allDijkstraNodes[neighbour].Gcost)
//        //            {
//        //                comeFromDict[neighbour] = currNode;
//        //                DijkstraNode<T> neigbourDijkstra = allDijkstraNodes[neighbour];
//        //                neigbourDijkstra.Gcost = newCostToNeighbour;
//        //                openSet.Update(neigbourDijkstra);
//        //            }
//        //        }
//        //    }
//        //    return comeFromDict;
//        //}

//        //***************************
//        // GREEDY FIRST SEARCH
//        //***************************
//        // - does NOT guarantee shortest path
//        // - generally very fast
//        // - works for 1:1 
//        //public static Dictionary<T, T> GreedyFirst(T startNode, T endNode, INavigatable<T> graph)
//        //{
//        //    Heap<GreedyNode<T>> openSet = new Heap<GreedyNode<T>>(graph.NodeCount);
//        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();

//        //    GreedyNode<T> newGreedyNode = new GreedyNode<T>(startNode, graph.GetDistance(startNode, endNode));
//        //    openSet.Add(newGreedyNode);

//        //    //until there are no more unchecked nodes...
//        //    while (openSet.Count > 0)
//        //    {
//        //        //we take best (Fcost or if equal Hcost) node on the heap
//        //        GreedyNode<T> currGreedyNode = openSet.Extract();
//        //        T currNode = currGreedyNode.m_item;

//        //        //if we reached end node we are done
//        //        if (currNode.Equals(endNode))
//        //            return comeFromDict;

//        //        //we check all its neighbours
//        //        foreach (T neighbour in graph.GetNeighbours(currNode))
//        //        {
//        //            if (!comeFromDict.ContainsKey(neighbour))
//        //            {
//        //                openSet.Add(new GreedyNode<T>(neighbour, graph.GetDistance(neighbour, endNode)));
//        //                comeFromDict[neighbour] = currNode;
//        //            }
//        //        }
//        //    }
//        //    return comeFromDict;
//        //}

//        //*******************
//        // A*
//        //*******************
//        // - guaranties shortest path
//        // - mostly use this one
//        // - works for 1:1

//        //public static Dictionary<T, T> Astar(T startNode, T endNode, INavigatable<T> graph)
//        //{
//        //    Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(graph.NodeCount);
//        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();
//        //    Dictionary<T, AStarNode<T>> allAstarNodes = new Dictionary<T, AStarNode<T>>();

//        //    AStarNode<T> newAstarNode = new AStarNode<T>(startNode, 0, graph.GetDistance(startNode, endNode));
//        //    openSet.Add(newAstarNode);
//        //    allAstarNodes.Add(startNode, newAstarNode);

//        //    //until there are no more unchecked nodes...
//        //    while (openSet.Count > 0)
//        //    {
//        //        AStarNode<T> currAstarNode = openSet.Extract();
//        //        T currNode = currAstarNode.m_item;

//        //        if (currNode.Equals(endNode))
//        //            return comeFromDict;

//        //        //we check all its neighbours
//        //        foreach (T neighbour in graph.GetNeighbours(currNode))
//        //        {
//        //            if (!neighbour.IsWalkable)
//        //                continue;

//        //            float newCostToNeighbour = currAstarNode.Gcost + graph.GetDistance(currNode, neighbour);


//        //            if (!comeFromDict.ContainsKey(neighbour))
//        //            {
//        //                AStarNode<T> neighbourAstar = new AStarNode<T>(neighbour, newCostToNeighbour, graph.GetDistance(neighbour, endNode));
//        //                openSet.Add(neighbourAstar);
//        //                comeFromDict.Add(neighbour, currNode);
//        //                allAstarNodes.Add(neighbour, neighbourAstar);
//        //            }
//        //            else if (newCostToNeighbour < allAstarNodes[neighbour].Gcost)
//        //            {
//        //                comeFromDict[neighbour] = currNode;
//        //                AStarNode<T> neigbourAstar = allAstarNodes[neighbour];
//        //                neigbourAstar.Gcost = newCostToNeighbour;
//        //                openSet.Update(neigbourAstar);
//        //            }
//        //        }
//        //    }
//        //    return comeFromDict;
//        //}

//        //******************
//        // A* distance
//        //******************
//        // - using A* finds only the distance between two nodes

//        //public static float AstarDistance(T startNode, T endNode, INavigatable<T> graph)
//        //{
//        //    Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(graph.NodeCount);
//        //    HashSet<T> closedSet = new HashSet<T>();

//        //    Dictionary<T, AStarNode<T>> allAstarNodes = new Dictionary<T, AStarNode<T>>();

//        //    AStarNode<T> newAstarNode = new AStarNode<T>(startNode, 0, graph.GetDistance(startNode, endNode));
//        //    openSet.Add(newAstarNode);
//        //    closedSet.Add(startNode);
//        //    allAstarNodes.Add(startNode, newAstarNode);

//        //    //until there are no more unchecked nodes...
//        //    while (openSet.Count > 0)
//        //    {
//        //        AStarNode<T> currAstarNode = openSet.Extract();
//        //        T currNode = currAstarNode.m_item;

//        //        if (currNode.Equals(endNode))
//        //            return 1000000; //TODO....

//        //        //we check all its neighbours
//        //        foreach (T neighbour in graph.GetNeighbours(currNode))
//        //        {
//        //            //if (neighbour.IsWalkable)
//        //            //{
//        //            float newCostToNeighbour = currAstarNode.Gcost + graph.GetDistance(currNode, neighbour);

//        //            if (!closedSet.Contains(neighbour))
//        //            {
//        //                AStarNode<T> neighbourAstar = new AStarNode<T>(neighbour, newCostToNeighbour, graph.GetDistance(neighbour, endNode));
//        //                openSet.Add(neighbourAstar);
//        //                closedSet.Add(neighbour);
//        //                allAstarNodes.Add(neighbour, neighbourAstar);
//        //            }
//        //            else if (newCostToNeighbour < allAstarNodes[neighbour].Gcost)
//        //            {
//        //                AStarNode<T> neigbourAstar = allAstarNodes[neighbour];
//        //                neigbourAstar.Gcost = newCostToNeighbour;
//        //                openSet.Update(neigbourAstar);
//        //            }
//        //        }
//        //    }
//        //    return -1;
//        //}

//        //public static System.Tuple<bool, List<Vector3>> RetracePath(INavigatable<T> graph, T startNode, T endNode, Dictionary<T, T> comeFromDict, bool reversePath)
//        //{
//        //    List<Vector3> path = new List<Vector3>();

//        //    T currNode = endNode;
//        //    while (!currNode.Equals(startNode))
//        //    {
//        //        path.Add(graph.GetPosition(currNode));
//        //        if (!comeFromDict.TryGetValue(currNode, out currNode))
//        //            return new Tuple<bool, List<Vector3>>(false, path);
//        //    }
//        //    path.Add(graph.GetPosition(startNode));

//        //    if (reversePath)
//        //        path.Reverse();
//        //    return new Tuple<bool, List<Vector3>>(true, path);
//        //}
//    }
//}