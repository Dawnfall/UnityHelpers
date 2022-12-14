using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// Current state: Vertical walls need fixing, floor mesh could be inverse of wall mesh, colliders need adding

namespace Dawnfall.Helper.MapGen
{
    public enum BinaryNodeType
    {
        EMPTY,
        FULL
    }

    public class BinaryGridNode
    {
        public readonly int m_row;
        public readonly int m_col;

        public BinaryNodeType m_type;
        public BinaryGridNode(int row, int col, BinaryNodeType type)
        {
            m_row = row;
            m_col = col;
            m_type = type;
        }
    }

    public class BinaryGridMap
    {
        public BinaryGridNode[] Nodes;
        private int m_rowCount;
        private int m_colCount;

        public BinaryGridMap(int rowCount, int colCount)
        {
            m_rowCount = rowCount;
            m_colCount = colCount;

            Nodes = new BinaryGridNode[m_rowCount * m_colCount];
            for (int row = 0; row < m_rowCount; row++)
                for (int col = 0; col < m_colCount; col++)
                    Nodes[row * m_colCount + col] = new BinaryGridNode(row, col, BinaryNodeType.EMPTY);
        }

        public int NodeCount
        {
            get { return m_rowCount * m_colCount; }
        }
        public int ColCount => m_colCount;
        public int RowCount => m_rowCount;

        public BinaryGridNode GetNode(int row, int col)
        {
            if (row < 0 || row >= m_rowCount || col < 0 || col >= m_colCount)
                return null;

            return Nodes[row * m_colCount + col];
        }

        public Vector3 GetNodePos(Vector3 gridOffset,int row, int col, float NodeSize)
        {
            if (row < 0 || row >= m_rowCount || col < 0 || col >= m_colCount)
                throw new System.Exception("Room coodinates out of boundries!");

            return
                gridOffset +
                Vector3.left * ((float)m_colCount / 2.0f - ((float)col + 0.5f)) * NodeSize +
                Vector3.down * ((float)m_rowCount / 2.0f - ((float)row + 0.5f)) * NodeSize
                ;
        }
        public Vector3 GetNodePos(Vector3 gridOffset,BinaryGridNode node, float NodeSize)
        {
            return GetNodePos(gridOffset, node.m_row, node.m_col, NodeSize);
        }

        public List<BinaryGridNode> Get4Neighbours(int row, int col)
        {
            List<BinaryGridNode> neigbours = new List<BinaryGridNode>();
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == j)
                        continue;
                    BinaryGridNode ne = GetNode(row + i, col + j);
                    if (ne != null)
                        neigbours.Add(ne);
                }
            return neigbours;
        }
        public List<BinaryGridNode> Get8Neighbours(int row, int col)
        {
            List<BinaryGridNode> neigbours = new List<BinaryGridNode>();
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    BinaryGridNode ne = GetNode(row + i, col + j);
                    if (ne != null)
                        neigbours.Add(ne);
                }
            return neigbours;
        }

        /// <summary>
        /// Creates Wall Edge around the map with given thickness
        /// </summary>
        /// <param name="borderthickness"></param>
        public void CreateMapBorder(int borderthickness)
        {
            for (int edge = 0; edge < borderthickness; edge++)
            {
                for (int row = 0; row < m_rowCount; row++)
                {
                    GetNode(row, edge).m_type = BinaryNodeType.FULL;
                    GetNode(row, m_colCount - edge - 1).m_type = BinaryNodeType.FULL;
                }

                for (int col = 0; col < m_colCount; col++)
                {
                    GetNode(edge, col).m_type = BinaryNodeType.FULL;
                    GetNode(m_rowCount - edge - 1, col).m_type = BinaryNodeType.FULL;
                }
            }
        }

        /// <summary>
        /// Fills Map with random node values based of given wall chance
        /// </summary>
        /// <param name="wallChance"></param>
        public void FillWithRandom(float wallChance)
        {
            //...Fill with random...
            for (int row = 0; row < m_rowCount; row++)
                for (int col = 0; col < m_colCount; col++)
                {
                    GetNode(row, col).m_type = (Random.Range(0, 100) < wallChance) ? BinaryNodeType.FULL : BinaryNodeType.EMPTY;
                }
        }
    }
}

//TODO: make a generic grid!