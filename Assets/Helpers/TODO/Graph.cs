using System.Collections.Generic;

namespace Dawnfall
{
    //directed graph
    public class Graph<T>
    {
        HashSet<T> m_nodes;
        Dictionary<T, HashSet<T>> m_connections;

        public Graph()
        {
            m_nodes = new HashSet<T>();
            m_connections = new Dictionary<T, HashSet<T>>();
        }

        /// <summary>
        /// checks if node is in graph
        /// </summary>
        /// <param name="node"> node to be checked </param>
        /// <returns> true if it is part of graph </returns>
        public bool Contains(T node)
        {
            return m_nodes.Contains(node);
        }

        /// <summary>
        /// Tries to add a node to graph
        /// </summary>
        /// <param name="newNode"> node to add </param>
        /// <returns> true if successfull </returns>
        public bool AddNode(T newNode)
        {
            if (!m_nodes.Add(newNode))
                return false;

            m_connections.Add(newNode, new HashSet<T>());
            return true;
        }

        /// <summary>
        /// tries to remove node and its connections
        /// </summary>
        /// <param name="nodeToRemove"> node to remove</param>
        public void RemoveNode(T nodeToRemove) //TODO this is slow for now
        {
            m_nodes.Remove(nodeToRemove);

            m_connections.Remove(nodeToRemove);
            foreach (var item in m_connections)
                item.Value.Remove(nodeToRemove);
        }

        /// <summary>
        /// tries to add directed edge between two nodes; nodes are created if not yet exist
        /// </summary>
        /// <param name="fromNode"> start node </param>
        /// <param name="toNode"> end node </param>
        /// <returns> true if successfull(link doesnt exist yet) </returns>
        public bool AddEdge(T n1, T n2)
        {
            if (!Contains(n1))
                AddNode(n1);
            if (!Contains(n2))
                AddNode(n2);
            return m_connections[n1].Add(n2);
        }

        /// <summary>
        /// tries to remove directed edge between n1->n2
        /// </summary>
        /// <param name="n1"> start node </param>
        /// <param name="n2"> end node </param>
        public void RemoveEdge(T n1, T n2)
        {
            if (m_nodes.Contains(n1))
                m_connections[n1].Remove(n2);
        }

        /// <summary>
        /// returns all neighbouring node of given node
        /// </summary>
        /// <param name="node"> node to find neighbours of </param>
        /// <returns> hashset of node; null if node is not in graph </returns>
        public HashSet<T> GetNeighbours(T node)
        {
            if (!Contains(node))
                return null;
            return m_connections[node];
        }

        public float GetDistance(T fromNode, T toNode)
        {
            if (Contains(fromNode) && m_connections[fromNode].Contains(toNode))
                return 1.0f;
            return 0.0f;

        }

        /// <summary>
        /// checks if there is a directed edge n1->n2
        /// </summary>
        /// <param name="n1"> start node </param>
        /// <param name="n2"> end node </param>
        /// <returns> true if there is a directed edge n1->n2 </returns>
        public bool IsEdge(T n1, T n2)
        {
            if (!(Contains(n1) && Contains(n2)))
                return false;
            if (m_connections[n1].Contains(n2))
                return true;
            return false;
        }
    }
}