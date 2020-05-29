using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public class NodeBase
    {
        public int m_index = -1;
        public Vector3 m_position;

        public NodeBase(Vector3 pos)
        {
            m_position = pos;
        }
    }

    public static class MeshHelper
    {
        public static void PolygonToMesh(NodeBase[] meshNodes, List<Vector3> vertices, List<int> indices, HashSet<Edge<NodeBase>>[] outlineEdgesSingleAndMultiple)
        {
            if (meshNodes == null || meshNodes.Length < 3)
                return;

            AssignPolygonVertices(meshNodes, vertices);

            List<Triangle<NodeBase>> triangles = new List<Triangle<NodeBase>>();
            for (int i = 0; i < meshNodes.Length - 2; i++)
            {
                Triangle<NodeBase> newTriangle = new Triangle<NodeBase>(meshNodes[0], meshNodes[i + 1], meshNodes[i + 2]);
                triangles.Add(newTriangle);
                AssignTriangleIndices(newTriangle, indices);
                DetectEdges(newTriangle, outlineEdgesSingleAndMultiple);
            }
        }

        private static void AssignPolygonVertices(NodeBase[] meshNodes, List<Vector3> vertices)
        {
            foreach (var node in meshNodes)
            {
                if (node.m_index == -1)
                {
                    node.m_index = vertices.Count;
                    vertices.Add(node.m_position);
                }
            }
        }
        private static void AssignTriangleIndices(Triangle<NodeBase> triangle, List<int> indices)
        {
            indices.Add(triangle.a.m_index);
            indices.Add(triangle.b.m_index);
            indices.Add(triangle.c.m_index);
        }

        static void DetectEdges(Triangle<NodeBase> triangle, HashSet<Edge<NodeBase>>[] outlineEdgesSingleAndMultiple)
        {
            CheckForOutlineEdge(triangle.AB, outlineEdgesSingleAndMultiple);
            CheckForOutlineEdge(triangle.BC, outlineEdgesSingleAndMultiple);
            CheckForOutlineEdge(triangle.CA, outlineEdgesSingleAndMultiple);
        }
        static void CheckForOutlineEdge(Edge<NodeBase> edge, HashSet<Edge<NodeBase>>[] outlineEdgesSingleAndMultiple)
        {
            if (outlineEdgesSingleAndMultiple[0].Contains(edge))
            {
                outlineEdgesSingleAndMultiple[0].Remove(edge);
                outlineEdgesSingleAndMultiple[1].Add(edge);
            }
            else
                outlineEdgesSingleAndMultiple[0].Add(edge);
        }
    }
}