using Dawnfall.Helper;
using Dawnfall.Helper.MapGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TL - 1
//TR - 2
//BR - 4
//BL - 8

namespace Dawnfall.Helper.MapGen
{
    public static class MarchingSquareMeshGen
    {
        //**************
        // Marching Square data
        //**************

        /// <summary>
        /// Create marching squares
        /// </summary>
        /// <param name="map"></param>
        /// <param name="wallSize"></param>
        static private MarchingSquare[] CreateSquareGrid(BinaryGridMap map, Vector3 gridOffset, float nodeSize)
        {
            MSNode[] allMeshNodes = new MSNode[map.NodeCount];
            for (int row = 0; row < map.RowCount; row++)
                for (int col = 0; col < map.ColCount; col++)
                {
                    allMeshNodes[row * map.ColCount + col] = new MSNode(
                        map.GetNodePos(gridOffset, row, col, nodeSize) - Vector3.forward * nodeSize,
                        map.GetNode(row, col).m_type == BinaryNodeType.FULL,
                        nodeSize
                        );
                }

            MarchingSquare[] marchingSquareGrid = new MarchingSquare[(map.RowCount - 1) * (map.ColCount - 1)];
            for (int sq = 0; sq < (map.RowCount - 1) * (map.ColCount - 1); sq++)
            {
                int row = sq / (map.ColCount - 1);
                int col = sq % (map.ColCount - 1);

                marchingSquareGrid[sq] = new MarchingSquare(
                    allMeshNodes[(row + 1) * map.ColCount + col],
                    allMeshNodes[(row + 1) * map.ColCount + col + 1],
                    allMeshNodes[row * map.ColCount + col],
                    allMeshNodes[row * map.ColCount + col + 1]

                    );
            }

            return marchingSquareGrid;
        }

        /// <summary>
        /// Triangulates every marching square based on formation it is in
        /// </summary>
        static void MarchingSquaresToMeshes(MarchingSquare[] marchingSquareGrid, ref List<Vector3> vertices, ref List<int> indices)
        {
            foreach (var square in marchingSquareGrid)
            {
                switch (square.GetConfiguration())
                {
                    case 0:
                        break;
                    case 1:
                        MarchingSquareToMesh(new NodeBase[] { square.CL, square.TL, square.CT }, ref vertices, ref indices);
                        break;
                    case 2:
                        MarchingSquareToMesh(new NodeBase[] { square.CT, square.TR, square.CR }, ref vertices, ref indices);
                        break;
                    case 3:
                        MarchingSquareToMesh(new NodeBase[] { square.CL, square.TL, square.TR, square.CR }, ref vertices, ref indices);
                        break;
                    case 4:
                        MarchingSquareToMesh(new NodeBase[] { square.CR, square.BR, square.CB }, ref vertices, ref indices);
                        break;
                    case 5:
                        MarchingSquareToMesh(new NodeBase[] { square.TL, square.CT, square.CR, square.BR, square.CB, square.CL }, ref vertices, ref indices);
                        break;
                    case 6:
                        MarchingSquareToMesh(new NodeBase[] { square.CT, square.TR, square.BR, square.CB }, ref vertices, ref indices);
                        break;
                    case 7:
                        MarchingSquareToMesh(new NodeBase[] { square.TL, square.TR, square.BR, square.CB, square.CL }, ref vertices, ref indices);
                        break;
                    case 8:
                        MarchingSquareToMesh(new NodeBase[] { square.CB, square.BL, square.CL }, ref vertices, ref indices);
                        break;
                    case 9:
                        MarchingSquareToMesh(new NodeBase[] { square.CB, square.BL, square.TL, square.CT }, ref vertices, ref indices);
                        break;
                    case 10:
                        MarchingSquareToMesh(new NodeBase[] { square.TR, square.CR, square.CB, square.BL, square.CL, square.CT }, ref vertices, ref indices);
                        break;
                    case 11:
                        MarchingSquareToMesh(new NodeBase[] { square.TL, square.TR, square.CR, square.CB, square.BL }, ref vertices, ref indices);
                        break;
                    case 12:
                        MarchingSquareToMesh(new NodeBase[] { square.CR, square.BR, square.BL, square.CL }, ref vertices, ref indices);
                        break;
                    case 13:
                        MarchingSquareToMesh(new NodeBase[] { square.TL, square.CT, square.CR, square.BR, square.BL }, ref vertices, ref indices);
                        break;
                    case 14:
                        MarchingSquareToMesh(new NodeBase[] { square.CL, square.CT, square.TR, square.BR, square.BL }, ref vertices, ref indices);
                        break;
                    case 15:
                        MarchingSquareToMesh(new NodeBase[] { square.TL, square.TR, square.BR, square.BL }, ref vertices, ref indices);
                        break;
                }
            }
        }

        private static void MarchingSquareToMesh(NodeBase[] meshNodes, ref List<Vector3> vertices, ref List<int> indices)
        {
            if (meshNodes == null || meshNodes.Length < 3)
                return;

            foreach (var node in meshNodes)
            {
                if (node.Index == -1)
                {
                    node.Index = vertices.Count;
                    vertices.Add(node.m_position);
                }
            }
        
            for (int i = 0; i < meshNodes.Length - 2; i++)
            {
                indices.Add(meshNodes[0].Index);
                indices.Add(meshNodes[i + 1].Index);
                indices.Add(meshNodes[i + 2].Index);
            }
        }

        public static void DeconstructMarchingSquaresToMeshData(BinaryGridMap map, Vector3 gridOffset, float NodeSize, out List<Vector3> vertices, out List<int> indices)
        {
            if (map == null)
            {
                vertices = null;
                indices = null;
                return;
            }

            vertices = new List<Vector3>();
            indices = new List<int>();

            MarchingSquare[] marchingSquareGrid = CreateSquareGrid(map, gridOffset, NodeSize);
            MarchingSquaresToMeshes(marchingSquareGrid, ref vertices, ref indices);
        }
    }
}





