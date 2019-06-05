using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////works in x/y plane
////****
//// QUADRANTS
////
//// 0 | 1
//// -----
//// 2 | 3


//namespace helper
//{
//    public class QuadTree<T>
//    {
//        private enum QuadTreeDir
//        {
//            L,
//            R,
//            U,
//            D,
//            LU,
//            LD,
//            RU,
//            RD,

//            STOP
//        }

//        private class FSM_QuadDirPair
//        {
//            public char m_quadrant;
//            public QuadTreeDir m_dir;

//            public FSM_QuadDirPair(char quadrant, QuadTreeDir dir)
//            {
//                m_quadrant = quadrant;
//                m_dir = dir;
//            }

//            public static bool operator ==(FSM_QuadDirPair a, FSM_QuadDirPair b)
//            {
//                return (a.m_quadrant == b.m_quadrant && a.m_dir == b.m_dir);
//            }
//            public static bool operator !=(FSM_QuadDirPair a, FSM_QuadDirPair b)
//            {
//                return (a.m_quadrant != b.m_quadrant || a.m_dir != b.m_dir);
//            }
//            public override bool Equals(object obj)
//            {
//                return this == (FSM_QuadDirPair)obj;
//            }
//            public override int GetHashCode()
//            {
//                return m_quadrant.GetHashCode() * 17 + m_dir.GetHashCode();
//            }
//        }

//        static private Dictionary<FSM_QuadDirPair, FSM_QuadDirPair> m_FSM;

//        public readonly Vector3 m_centerPos;
//        public readonly int m_maxLevel;
//        public readonly float m_sideLength;

//        Dictionary<string, T> m_linearNodes;

//        static QuadTree()
//        {
//            m_FSM = new Dictionary<FSM_QuadDirPair, FSM_QuadDirPair>();

//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.R), new FSM_QuadDirPair('1', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.L), new FSM_QuadDirPair('1', QuadTreeDir.L));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.D), new FSM_QuadDirPair('2', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.U), new FSM_QuadDirPair('2', QuadTreeDir.U));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.RU), new FSM_QuadDirPair('3', QuadTreeDir.U));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.RD), new FSM_QuadDirPair('3', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.LD), new FSM_QuadDirPair('3', QuadTreeDir.L));
//            m_FSM.Add(new FSM_QuadDirPair('0', QuadTreeDir.LU), new FSM_QuadDirPair('3', QuadTreeDir.LU));

//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.R), new FSM_QuadDirPair('0', QuadTreeDir.R));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.L), new FSM_QuadDirPair('0', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.D), new FSM_QuadDirPair('3', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.U), new FSM_QuadDirPair('3', QuadTreeDir.U));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.RU), new FSM_QuadDirPair('2', QuadTreeDir.RU));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.RD), new FSM_QuadDirPair('2', QuadTreeDir.R));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.LD), new FSM_QuadDirPair('2', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('1', QuadTreeDir.LU), new FSM_QuadDirPair('2', QuadTreeDir.U));

//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.R), new FSM_QuadDirPair('3', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.L), new FSM_QuadDirPair('3', QuadTreeDir.L));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.D), new FSM_QuadDirPair('0', QuadTreeDir.D));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.U), new FSM_QuadDirPair('0', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.RU), new FSM_QuadDirPair('1', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.RD), new FSM_QuadDirPair('1', QuadTreeDir.D));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.LD), new FSM_QuadDirPair('1', QuadTreeDir.LD));
//            m_FSM.Add(new FSM_QuadDirPair('2', QuadTreeDir.LU), new FSM_QuadDirPair('1', QuadTreeDir.L));

//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.R), new FSM_QuadDirPair('2', QuadTreeDir.R));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.L), new FSM_QuadDirPair('2', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.D), new FSM_QuadDirPair('1', QuadTreeDir.D));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.U), new FSM_QuadDirPair('1', QuadTreeDir.STOP));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.RU), new FSM_QuadDirPair('0', QuadTreeDir.R));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.RD), new FSM_QuadDirPair('0', QuadTreeDir.RD));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.LD), new FSM_QuadDirPair('0', QuadTreeDir.D));
//            m_FSM.Add(new FSM_QuadDirPair('3', QuadTreeDir.LU), new FSM_QuadDirPair('0', QuadTreeDir.STOP));
//        }
//        public QuadTree(Vector3 centerWorldPos, float sideLength, int maxLevel)
//        {
//            if (sideLength <= 0)
//                throw new System.Exception("Side length must be greater than zero!");
//            if (maxLevel < 0)
//                throw new System.Exception("Max level must not be negative!");

//            m_centerPos = centerWorldPos;
//            m_sideLength = sideLength;
//            m_maxLevel = maxLevel;

//            m_linearNodes = new Dictionary<string, T>();
//            m_linearNodes.Add("", new QuadTreeNode("", new Vector3(), m_sideLength));
//        }

//        public int NodeCount
//        {
//            get { return m_linearNodes.Count; }
//        }
//        public Dictionary<string, QuadTreeNode> Nodes
//        {
//            get { return m_linearNodes; }
//        }

//        public void Subdivide(QuadTreeNode node)
//        {
//            float childSideLength = m_sideLength / Mathf.Pow(2.0f, node.Level + 1);
//            float childSideLengthHalf = childSideLength / 2.0f;

//            string code0 = node.Code + 0;
//            string code1 = node.Code + 1;
//            string code2 = node.Code + 2;
//            string code3 = node.Code + 3;


//            m_linearNodes[code0] = new QuadTreeNode(code0, node.RelativePos + new Vector3(-1, 0, 1) * childSideLengthHalf, childSideLength);
//            m_linearNodes[code1] = new QuadTreeNode(code1, node.RelativePos + new Vector3(1, 0, 1) * childSideLengthHalf, childSideLength);
//            m_linearNodes[code2] = new QuadTreeNode(code2, node.RelativePos + new Vector3(-1, 0, -1) * childSideLengthHalf, childSideLength);
//            m_linearNodes[code3] = new QuadTreeNode(code3, node.RelativePos + new Vector3(1, 0, -1) * childSideLengthHalf, childSideLength);

//            m_linearNodes.Remove(node.Code);
//        }

//        public bool Contains(string code)
//        {
//            return m_linearNodes.ContainsKey(code);
//        }

//        public Vector3 GetNodeWorldPosition(QuadTreeNode node)
//        {
//            if (!Contains(node.Code))
//                throw new System.Exception("Node not from this quadtree!");

//            return m_centerPos + node.RelativePos;
//        }

//        public float GetDistance(QuadTreeNode a, QuadTreeNode b)
//        {
//            return (a.RelativePos - b.RelativePos).magnitude;
//        }

//        //TODO: isnt fully implemented yet
//        public HashSet<QuadTreeNode> GetNeighbours(QuadTreeNode node)
//        {
//            HashSet<QuadTreeNode> allNeighbours = new HashSet<QuadTreeNode>();
//            for (int i = 0; i < 8; i++)
//            {
//                QuadTreeDir dir = (QuadTreeDir)i;
//                string equalNeigbourCode = getEqualNeigbourCode(node.Code, dir);
//                if (Contains(equalNeigbourCode))
//                    allNeighbours.Add(m_linearNodes[equalNeigbourCode]);
//            }
//            return allNeighbours;
//        }

//        private static string getEqualNeigbourCode(string code, QuadTreeDir dir)
//        {
//            string resultCode = "";
//            for (int i = code.Length; i >= 0; i--)
//            {
//                var newQuadDirPair = m_FSM[new FSM_QuadDirPair(code[i], dir)];
//                resultCode = newQuadDirPair.m_quadrant + resultCode;

//                if (newQuadDirPair.m_dir == QuadTreeDir.STOP && i > 0)
//                {
//                    resultCode = code.Substring(0, i - 1) + resultCode;
//                }
//            }
//            return resultCode;
//        }
//    }
//}

//public class QuadTreeNode
//{
//    public readonly string Code;
//    public readonly int Level;
//    public readonly Vector3 RelativePos;
//    public readonly float SideLength;

//    bool m_isWalkable = true;

//    public QuadTreeNode(string code, Vector3 relativePos, float sideLength)
//    {
//        Code = code;
//        Level = Code.Length;
//        RelativePos = relativePos;
//        SideLength = sideLength;
//    }

//    public bool IsWalkable
//    {
//        get { return m_isWalkable; }
//        set { m_isWalkable = value; }
//    }
//}
