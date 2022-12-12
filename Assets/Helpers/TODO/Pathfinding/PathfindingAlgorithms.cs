using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Dawnfall;
using Dawnfall.Helper.Pathfinding;
using Dawnfall.Helper;

namespace Dawnfall.Helper.Pathfinding
{
    public class DijkstraNode<T> : IHeapable, IComparable<DijkstraNode<T>>
    {
        public T m_item;
        int m_index;
        float m_gCost;

        public DijkstraNode(T item, float gCost)
        {
            m_item = item;
            m_gCost = gCost;
        }

        public int HeapIndex
        {
            get { return m_index; }
            set { m_index = value; }
        }
        public float Gcost
        {
            get { return m_gCost; }
            set { m_gCost = value; }
        }
        public int CompareTo(DijkstraNode<T> other)
        {
            return -m_gCost.CompareTo(other.m_gCost);
        }
    }
    public class GreedyNode<T> : IHeapable, IComparable<GreedyNode<T>>
    {
        public T m_item;
        int m_index;
        float m_hCost;

        public GreedyNode(T item, float hCost)
        {
            m_item = item;
            m_hCost = hCost;
        }

        public int HeapIndex
        {
            get { return m_index; }
            set { m_index = value; }
        }
        public float Hcost
        {
            get { return m_hCost; }
            set { m_hCost = value; }
        }
        public int CompareTo(GreedyNode<T> other)
        {
            return -m_hCost.CompareTo(other.m_hCost);
        }
    }
    public class AStarNode<T> : IHeapable, IComparable<AStarNode<T>>
    {
        public T m_item;
        int m_index;
        float m_gCost;
        float m_hCost;

        public AStarNode(T item, float gCost, float hCost)
        {
            m_item = item;
            m_gCost = gCost;
            m_hCost = hCost;
        }

        public int HeapIndex
        {
            get { return m_index; }
            set { m_index = value; }
        }
        public float Gcost
        {
            get { return m_gCost; }
            set { m_gCost = value; }
        }
        public float Hcost
        {
            get { return m_hCost; }
            set { m_hCost = value; }
        }
        public float Fcost
        {
            get { return m_gCost + m_hCost; }
        }

        public int CompareTo(AStarNode<T> other)
        {
            int compare = Fcost.CompareTo(other.Fcost);
            if (compare == 0)
                compare = Gcost.CompareTo(other.Gcost);
            return -compare;
        }
    }


    public static class PathfindingAlgorithms
    {
        //**********************
        // BREADTH FIRST SEARCH
        //**********************
        // - guaranties shortest path
        // - assumes all edges are of equal cost(uniform heuristic)
        // - best for 1:n or n:1

        //public static Dictionary<T, T> BreadthFirstSearch(T startNode, T endNode, INavigatable<T> graph)
        //{
        //    Queue<T> openSet = new Queue<T>();
        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();

        //    openSet.Enqueue(startNode);
        //    comeFromDict.Add(startNode, default(T));

        //    while (openSet.Count > 0)
        //    {
        //        T currNode = openSet.Dequeue();

        //        if (currNode.Equals(endNode))
        //            return comeFromDict;

        //        foreach (T neighbour in graph.GetNeighbours(currNode))
        //            if (!comeFromDict.ContainsKey(neighbour))
        //            {
        //                openSet.Enqueue(neighbour);
        //                comeFromDict.Add(neighbour, currNode);
        //            }
        //    }
        //    return comeFromDict;
        //}

        //*****************************
        // DIJKSTRA (uniform cost search)
        //*****************************
        // - guaranties shortest path
        // - can deal with diferent weights
        // - best for 1:n or n:1

        //public static Dictionary<T, T> Dijkstra(T startNode, T endNode, INavigatable<T> graph)
        //{
        //    Heap<DijkstraNode<T>> openSet = new Heap<DijkstraNode<T>>(graph.NodeCount);
        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();
        //    Dictionary<T, DijkstraNode<T>> allDijkstraNodes = new Dictionary<T, DijkstraNode<T>>();

        //    DijkstraNode<T> newDijkstraNode = new DijkstraNode<T>(startNode, 0);
        //    openSet.Add(newDijkstraNode);
        //    allDijkstraNodes[startNode] = newDijkstraNode;

        //    ////until there are no more unchecked nodes...
        //    while (openSet.Count > 0)
        //    {
        //        //we take best (Fcost or if equal Hcost) node on the heap
        //        DijkstraNode<T> currDijkstra = openSet.Extract();
        //        T currNode = currDijkstra.m_item;

        //        //if we reached end node we are done
        //        if (currNode.Equals(endNode))
        //            return comeFromDict;

        //        //we check all its neighbours
        //        foreach (T neighbour in graph.GetNeighbours(currNode))
        //        {
        //            float newCostToNeighbour = currDijkstra.Gcost + graph.GetDistance(currNode, neighbour);

        //            if (!comeFromDict.ContainsKey(neighbour))
        //            {
        //                DijkstraNode<T> neighbourDijkstra = new DijkstraNode<T>(neighbour, newCostToNeighbour);

        //                openSet.Add(neighbourDijkstra);
        //                comeFromDict.Add(neighbour, currNode);
        //                allDijkstraNodes[neighbour] = neighbourDijkstra;
        //            }

        //            else if (newCostToNeighbour < allDijkstraNodes[neighbour].Gcost)
        //            {
        //                comeFromDict[neighbour] = currNode;
        //                DijkstraNode<T> neigbourDijkstra = allDijkstraNodes[neighbour];
        //                neigbourDijkstra.Gcost = newCostToNeighbour;
        //                openSet.Update(neigbourDijkstra);
        //            }
        //        }
        //    }
        //    return comeFromDict;
        //}

        //***************************
        // GREEDY FIRST SEARCH
        //***************************
        // - does NOT guarantee shortest path
        // - generally very fast
        // - works for 1:1 
        //public static Dictionary<T, T> GreedyFirst(T startNode, T endNode, INavigatable<T> graph)
        //{
        //    Heap<GreedyNode<T>> openSet = new Heap<GreedyNode<T>>(graph.NodeCount);
        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();

        //    GreedyNode<T> newGreedyNode = new GreedyNode<T>(startNode, graph.GetDistance(startNode, endNode));
        //    openSet.Add(newGreedyNode);

        //    //until there are no more unchecked nodes...
        //    while (openSet.Count > 0)
        //    {
        //        //we take best (Fcost or if equal Hcost) node on the heap
        //        GreedyNode<T> currGreedyNode = openSet.Extract();
        //        T currNode = currGreedyNode.m_item;

        //        //if we reached end node we are done
        //        if (currNode.Equals(endNode))
        //            return comeFromDict;

        //        //we check all its neighbours
        //        foreach (T neighbour in graph.GetNeighbours(currNode))
        //        {
        //            if (!comeFromDict.ContainsKey(neighbour))
        //            {
        //                openSet.Add(new GreedyNode<T>(neighbour, graph.GetDistance(neighbour, endNode)));
        //                comeFromDict[neighbour] = currNode;
        //            }
        //        }
        //    }
        //    return comeFromDict;
        //}

        //*******************
        // A*
        //*******************
        // - guaranties shortest path
        // - mostly use this one
        // - works for 1:1

        //public static Dictionary<T, T> Astar(T startNode, T endNode, INavigatable<T> graph)
        //{
        //    Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(graph.NodeCount);
        //    Dictionary<T, T> comeFromDict = new Dictionary<T, T>();
        //    Dictionary<T, AStarNode<T>> allAstarNodes = new Dictionary<T, AStarNode<T>>();

        //    AStarNode<T> newAstarNode = new AStarNode<T>(startNode, 0, graph.GetDistance(startNode, endNode));
        //    openSet.Add(newAstarNode);
        //    allAstarNodes.Add(startNode, newAstarNode);

        //    //until there are no more unchecked nodes...
        //    while (openSet.Count > 0)
        //    {
        //        AStarNode<T> currAstarNode = openSet.Extract();
        //        T currNode = currAstarNode.m_item;

        //        if (currNode.Equals(endNode))
        //            return comeFromDict;

        //        //we check all its neighbours
        //        foreach (T neighbour in graph.GetNeighbours(currNode))
        //        {
        //            if (!neighbour.IsWalkable)
        //                continue;

        //            float newCostToNeighbour = currAstarNode.Gcost + graph.GetDistance(currNode, neighbour);


        //            if (!comeFromDict.ContainsKey(neighbour))
        //            {
        //                AStarNode<T> neighbourAstar = new AStarNode<T>(neighbour, newCostToNeighbour, graph.GetDistance(neighbour, endNode));
        //                openSet.Add(neighbourAstar);
        //                comeFromDict.Add(neighbour, currNode);
        //                allAstarNodes.Add(neighbour, neighbourAstar);
        //            }
        //            else if (newCostToNeighbour < allAstarNodes[neighbour].Gcost)
        //            {
        //                comeFromDict[neighbour] = currNode;
        //                AStarNode<T> neigbourAstar = allAstarNodes[neighbour];
        //                neigbourAstar.Gcost = newCostToNeighbour;
        //                openSet.Update(neigbourAstar);
        //            }
        //        }
        //    }
        //    return comeFromDict;
        //}

        //******************
        // A* distance
        //******************
        // - using A* finds only the distance between two nodes

        //public static float AstarDistance(T startNode, T endNode, INavigatable<T> graph)
        //{
        //    Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(graph.NodeCount);
        //    HashSet<T> closedSet = new HashSet<T>();

        //    Dictionary<T, AStarNode<T>> allAstarNodes = new Dictionary<T, AStarNode<T>>();

        //    AStarNode<T> newAstarNode = new AStarNode<T>(startNode, 0, graph.GetDistance(startNode, endNode));
        //    openSet.Add(newAstarNode);
        //    closedSet.Add(startNode);
        //    allAstarNodes.Add(startNode, newAstarNode);

        //    //until there are no more unchecked nodes...
        //    while (openSet.Count > 0)
        //    {
        //        AStarNode<T> currAstarNode = openSet.Extract();
        //        T currNode = currAstarNode.m_item;

        //        if (currNode.Equals(endNode))
        //            return 1000000; //TODO....

        //        //we check all its neighbours
        //        foreach (T neighbour in graph.GetNeighbours(currNode))
        //        {
        //            //if (neighbour.IsWalkable)
        //            //{
        //            float newCostToNeighbour = currAstarNode.Gcost + graph.GetDistance(currNode, neighbour);

        //            if (!closedSet.Contains(neighbour))
        //            {
        //                AStarNode<T> neighbourAstar = new AStarNode<T>(neighbour, newCostToNeighbour, graph.GetDistance(neighbour, endNode));
        //                openSet.Add(neighbourAstar);
        //                closedSet.Add(neighbour);
        //                allAstarNodes.Add(neighbour, neighbourAstar);
        //            }
        //            else if (newCostToNeighbour < allAstarNodes[neighbour].Gcost)
        //            {
        //                AStarNode<T> neigbourAstar = allAstarNodes[neighbour];
        //                neigbourAstar.Gcost = newCostToNeighbour;
        //                openSet.Update(neigbourAstar);
        //            }
        //        }
        //    }
        //    return -1;
        //}

        //public static System.Tuple<bool, List<Vector3>> RetracePath(INavigatable<T> graph, T startNode, T endNode, Dictionary<T, T> comeFromDict, bool reversePath)
        //{
        //    List<Vector3> path = new List<Vector3>();

        //    T currNode = endNode;
        //    while (!currNode.Equals(startNode))
        //    {
        //        path.Add(graph.GetPosition(currNode));
        //        if (!comeFromDict.TryGetValue(currNode, out currNode))
        //            return new Tuple<bool, List<Vector3>>(false, path);
        //    }
        //    path.Add(graph.GetPosition(startNode));

        //    if (reversePath)
        //        path.Reverse();
        //    return new Tuple<bool, List<Vector3>>(true, path);
        //}

        public static Dictionary<Vector2Int, Vector2Int> Astar(Vector2Int startCoord, Vector2Int endCoord, PathfindGrid grid)
        {
            Heap<AStarNode<Vector2Int>> openSet = new Heap<AStarNode<Vector2Int>>(grid.NodeCount);
            Dictionary<Vector2Int, Vector2Int> comeFromDict = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, AStarNode<Vector2Int>> allAstarNodes = new Dictionary<Vector2Int, AStarNode<Vector2Int>>();

            AStarNode<Vector2Int> newAstarNode = new AStarNode<Vector2Int>(startCoord, 0, grid.GetDistance(startCoord, endCoord));
            openSet.Add(newAstarNode);
            allAstarNodes.Add(startCoord, newAstarNode);

            //until there are no more unchecked nodes...
            while (openSet.Count > 0)
            {
                AStarNode<Vector2Int> currAstarNode = openSet.Extract();
                Vector2Int currCoord = currAstarNode.m_item;

                if (currCoord == endCoord)
                    return comeFromDict;

                //we check all its neighbours
                foreach (Vector2Int neighbour in grid.Get8Neighbours(currCoord))
                {
                    if (!grid.IsWalkable(neighbour))
                        continue;

                    float newCostToNeighbour = currAstarNode.Gcost + grid.GetDistance(currCoord, neighbour);

                    if (!comeFromDict.ContainsKey(neighbour))
                    {
                        AStarNode<Vector2Int> neighbourAstar = new AStarNode<Vector2Int>(neighbour, newCostToNeighbour, grid.GetDistance(neighbour, endCoord));
                        openSet.Add(neighbourAstar);
                        comeFromDict.Add(neighbour, currCoord);
                        allAstarNodes.Add(neighbour, neighbourAstar);
                    }
                    else if (newCostToNeighbour < allAstarNodes[neighbour].Gcost)
                    {
                        comeFromDict[neighbour] = currCoord;
                        AStarNode<Vector2Int> neigbourAstar = allAstarNodes[neighbour];
                        neigbourAstar.Gcost = newCostToNeighbour;
                        openSet.Update(neigbourAstar);
                    }
                }
            }
            return comeFromDict;
        }
        public static System.Tuple<bool, List<Vector3>> RetracePath(PathfindGrid grid, Vector2Int startCoord, Vector2Int endCoord, Dictionary<Vector2Int, Vector2Int> comeFromDict, bool reversePath)
        {
            List<Vector3> path = new List<Vector3>();

            Vector2Int currCoord = endCoord;
            while (currCoord != startCoord)
            {
                path.Add(grid.CoordsToWorld(currCoord));
                if (!comeFromDict.TryGetValue(currCoord, out currCoord))
                    return new Tuple<bool, List<Vector3>>(false, path);
            }
            path.Add(grid.CoordsToWorld(startCoord));

            if (reversePath)
                path.Reverse();
            return new Tuple<bool, List<Vector3>>(true, path);
        }
    }
}


// just for refference meant to be deleted!

//namespace pathfinding
//{
//    public static class GridPathfindingAlgorithms
//    {
//        public class GridPathNode
//        {
//            public Vector2Int Item { get; set; }
//            public GridPathNode FromNode { get; set; }
//        }

//        public class GridDijkstraNode : GridPathNode, IComparable<GridDijkstraNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Gcost { get; set; }
//            public int CompareTo(GridDijkstraNode other)
//            {
//                return -Gcost.CompareTo(other.Gcost);
//            }
//        }

//        public class GridGreedyNode : GridPathNode, IComparable<GridGreedyNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Hcost { get; set; }
//            public int CompareTo(GridGreedyNode other)
//            {
//                return -Hcost.CompareTo(other.Hcost);
//            }
//        }

//        public class GridAStarNode : GridPathNode, IComparable<GridAStarNode>, IHeapable
//        {
//            public int HeapIndex { get; set; }
//            public float Gcost { get; set; }
//            public float Hcost { get; set; }
//            public float Fcost { get { return Gcost + Hcost; } }

//            public int CompareTo(GridAStarNode other)
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

//        public static List<Vector2Int> BreadthFirstSearch(Vector2Int start, Vector2Int end, PathfindGrid grid)
//        {
//            Queue<GridPathNode> openSet = new Queue<GridPathNode>();
//            HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();

//            GridPathNode startBreadthNode = new GridPathNode { Item = start, FromNode = null };
//            visitedNodes.Add(start);
//            openSet.Enqueue(startBreadthNode);

//            GridPathNode currentBreadthNode = null;
//            while (openSet.Count > 0)
//            {
//                currentBreadthNode = openSet.Dequeue();

//                if (currentBreadthNode.Item.Equals(end))
//                    break;

//                foreach (Vector2Int neighbour in grid.Get8Neighbours(currentBreadthNode.Item))
//                {
//                    if (!grid.IsWalkable(neighbour))
//                        continue;

//                    if (!visitedNodes.Contains(neighbour))
//                    {
//                        GridPathNode neighbourBreathNode = new GridPathNode { Item = neighbour, FromNode = currentBreadthNode };
//                        visitedNodes.Add(neighbour);
//                        openSet.Enqueue(neighbourBreathNode);
//                    }
//                }
//            }
//            if (currentBreadthNode == null || currentBreadthNode.Item != end)
//                return null;

//            return Retrace(currentBreadthNode, start);
//        }

//        //***************************
//        // GREEDY FIRST SEARCH
//        //***************************
//        // - does NOT guarantee shortest path
//        // - generally very fast
//        // - works for 1:1 
//        public static List<Vector2Int> GreedyFirst(Vector2Int start, Vector2Int end, PathfindGrid grid) //needs a bit of checking
//        {
//            Heap<GridGreedyNode> openSet = new Heap<GridGreedyNode>(grid.NodeCount);
//            Dictionary<Vector2Int, GridGreedyNode> allNodesDict = new Dictionary<Vector2Int, GridGreedyNode>();

//            GridGreedyNode newGreedyNode = new GridGreedyNode { Item = start, FromNode = null, Hcost = grid.GetDistance(start, end) };
//            openSet.Add(newGreedyNode);
//            allNodesDict[start] = newGreedyNode;

//            //until there are no more unchecked nodes...
//            GridGreedyNode currGreedyNode = null;
//            while (openSet.Count > 0)
//            {

//                currGreedyNode = openSet.Extract();

//                //if we reached end node we are done
//                if (currGreedyNode.Item.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (Vector2Int neighbour in grid.Get8Neighbours(currGreedyNode.Item))
//                {
//                    if (!grid.IsWalkable(neighbour))
//                        continue;

//                    GridGreedyNode neighbourGreedyNode;
//                    if (!allNodesDict.TryGetValue(neighbour, out neighbourGreedyNode))
//                    {
//                        neighbourGreedyNode = new GridGreedyNode { Item = neighbour, FromNode = currGreedyNode, Hcost = grid.GetDistance(neighbour, end) };
//                        openSet.Add(neighbourGreedyNode);
//                        allNodesDict.Add(neighbour, neighbourGreedyNode);
//                    }
//                }
//                currGreedyNode.HeapIndex = -2;
//            }
//            if (currGreedyNode == null || currGreedyNode.Item != end)
//                return null;

//            return Retrace(currGreedyNode, start);
//        }

//        public static List<Vector2Int> Dijkstra(Vector2Int start, Vector2Int end, PathfindGrid grid)
//        {
//            Heap<GridDijkstraNode> openSet = new Heap<GridDijkstraNode>(grid.NodeCount);
//            Dictionary<Vector2Int, GridDijkstraNode> allNodeDict = new Dictionary<Vector2Int, GridDijkstraNode>();

//            GridDijkstraNode newDijkstraNode = new GridDijkstraNode { Item = start, FromNode = null, Gcost = 0 };
//            openSet.Add(newDijkstraNode);
//            allNodeDict[start] = newDijkstraNode;

//            GridDijkstraNode currDijkstra = null;
//            //until there are no more unchecked nodes...
//            while (openSet.Count > 0)
//            {
//                //we take best (Fcost or if equal Hcost) node on the heap
//                currDijkstra = openSet.Extract();

//                //if we reached end node we are done
//                if (currDijkstra.Item.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (Vector2Int neighbour in grid.Get8Neighbours(currDijkstra.Item))
//                {
//                    if (!grid.IsWalkable(neighbour))
//                        continue;

//                    float newCostToNeighbour = currDijkstra.Gcost + grid.GetDistance(currDijkstra.Item, neighbour);

//                    GridDijkstraNode neighbourDijkstra;
//                    if (allNodeDict.TryGetValue(neighbour, out neighbourDijkstra))
//                    {
//                        if (neighbourDijkstra.HeapIndex > 0 && neighbourDijkstra.Gcost > newCostToNeighbour) //is on the openSet
//                        {
//                            neighbourDijkstra.FromNode = currDijkstra;
//                            neighbourDijkstra.Gcost = newCostToNeighbour;
//                            openSet.Update(neighbourDijkstra);
//                        }
//                    }
//                    else
//                    {
//                        neighbourDijkstra = new GridDijkstraNode { Item = neighbour, FromNode = currDijkstra, Gcost = newCostToNeighbour };
//                        allNodeDict[neighbour] = neighbourDijkstra;
//                        openSet.Add(neighbourDijkstra);
//                    }
//                }
//                currDijkstra.HeapIndex = -2; //closed set
//            }

//            if (currDijkstra == null || currDijkstra.Item != end)
//                return null;

//            return Retrace(currDijkstra, start);
//        }

//        public static List<Vector2Int> Astar(Vector2Int start, Vector2Int end, PathfindGrid grid)
//        {
//            Heap<GridAStarNode> openSet = new Heap<GridAStarNode>(grid.NodeCount);
//            Dictionary<Vector2Int, GridAStarNode> allNodeDict = new Dictionary<Vector2Int, GridAStarNode>();

//            GridAStarNode newAstarNode = new GridAStarNode { Item = start, FromNode = null, Gcost = 0, Hcost = grid.GetDistance(start, end) };
//            openSet.Add(newAstarNode);
//            allNodeDict.Add(start, newAstarNode);

//            GridAStarNode currAstarNode = null;
//            while (openSet.Count > 0)
//            {
//                currAstarNode = openSet.Extract();
//                if (currAstarNode.Item.Equals(end))
//                    break;

//                //we check all its neighbours
//                foreach (Vector2Int neighbour in grid.Get8Neighbours(currAstarNode.Item))
//                {
//                    if (!grid.IsWalkable(neighbour))
//                        continue;

//                    float newCostToNeighbour = currAstarNode.Gcost + grid.GetDistance(currAstarNode.Item, neighbour);

//                    GridAStarNode neighbourAstar;
//                    if (allNodeDict.TryGetValue(neighbour, out neighbourAstar))
//                    {
//                        if (neighbourAstar.HeapIndex > 0 && neighbourAstar.Gcost > newCostToNeighbour)
//                        {
//                            neighbourAstar.FromNode = currAstarNode;
//                            neighbourAstar.Gcost = newCostToNeighbour;
//                            openSet.Update(neighbourAstar);
//                        }
//                    }
//                    else
//                    {
//                        neighbourAstar = new GridAStarNode { Item = neighbour, FromNode = currAstarNode, Gcost = newCostToNeighbour, Hcost = grid.GetDistance(neighbour, end) };
//                        allNodeDict.Add(neighbour, neighbourAstar);
//                        openSet.Add(neighbourAstar);
//                    }

//                }
//                currAstarNode.HeapIndex = -2;
//            }
//            if (currAstarNode == null || currAstarNode.Item != end)
//                return null;

//            List<Vector2Int> path = Retrace(currAstarNode, start);
//            path = SmoothPath(path, grid);
//            return path;
//        }

//        private static List<Vector2Int> Retrace(GridPathNode endNode, Vector2Int start) //can be optimized
//        {
//            List<Vector2Int> path = new List<Vector2Int>();
//            GridPathNode Node_2 = null;
//            GridPathNode Node_1 = null;
//            while (endNode.Item != start)
//            {
//                if (Node_2 == null || Node_1 == null)
//                    path.Add(endNode.Item);
//                else
//                {
//                    Vector2Int prevDir = new Vector2Int(Node_1.Item.x - Node_2.Item.x, Node_1.Item.y - Node_2.Item.y);
//                    Vector2Int currDir = new Vector2Int(endNode.Item.x - Node_1.Item.x, endNode.Item.y - Node_1.Item.y);
//                    if (prevDir == currDir)
//                        path[path.Count - 1] = endNode.Item;
//                    else
//                        path.Add(endNode.Item);
//                }
//                Node_2 = Node_1;
//                Node_1 = endNode;
//                endNode = endNode.FromNode;
//            }

//            return path;
//        }

//        private static List<Vector2Int> SmoothPath(List<Vector2Int> path, PathfindGrid grid)
//        {
//            List<Vector2Int> resultPath = new List<Vector2Int>();

//            resultPath.Add(path[0]);
//            int startIndex = 0;
//            while (startIndex < path.Count - 1)
//            {
//                int endIndex = startIndex + 2;
//                while (endIndex < path.Count)
//                {
//                    Vector2Int start = path[startIndex];
//                    Vector2Int end = path[endIndex];

//                    if (!IsValidPath(start, end, grid))
//                        break;
//                    endIndex++;
//                }
//                resultPath.Add(path[endIndex - 1]);
//                startIndex = endIndex - 1;
//            }

//            return resultPath;
//        }

//        private static bool IsValidPath(Vector2Int start, Vector2Int end, PathfindGrid grid)
//        {
//            try
//            {
//                Vector2 line = new Vector2(end.x - start.x, end.y - start.y);
//                if (line.x == 0 || line.y == 0)
//                    return true;

//                float magnitude = line.magnitude;
//                //Debug.Log("magnitude: " + magnitude);
//                Vector2 dir = line.normalized;

//                float borderX = (dir.x < 0) ? 0f : 1f;
//                float borderY = (dir.y < 0) ? 0f : 1f;

//                float t = 0f;
//                Vector2 currPos = new Vector2(start.x + 0.5f, start.y + 0.5f);
//                Vector2Int nextCoords = start;
//                while (t < magnitude)
//                {
//                    if (!grid.IsWalkable(nextCoords))
//                    {
//                        Debug.Log("false");
//                        return false;
//                    }

//                    float tX = (borderX - (currPos.x - nextCoords.x)) / dir.x;
//                    float tY = (borderY - (currPos.x - nextCoords.y)) / dir.y;

//                    if (tX < tY)
//                    {
//                        nextCoords.x += (dir.x > 0) ? 1 : -1;
//                    }
//                    else
//                    {
//                        nextCoords.y += (dir.y > 0) ? 1 : -1;
//                    }
//                    t += Mathf.Min(tX, tY);
//                    currPos += dir * t;

//                    //Debug.Log("currX: " + currX + " ; currY: " + currY + " ; coordX: " + currentCoords.x + " ; coordY: " + currentCoords.y + " , t: " + t);
//                }

//                Debug.Log("true");
//                return true;
//            }
//            catch (Exception e)
//            {
//                Debug.Log("error");
//                return false;
//            }
//        }
//    }
//}