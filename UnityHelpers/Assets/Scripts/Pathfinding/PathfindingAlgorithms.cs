using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using helper;

namespace pathfinding
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