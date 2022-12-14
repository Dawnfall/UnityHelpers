using Codice.Client.BaseCommands;
using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Dawnfall.Helper.MapGen
{
    public static class MapGenUtils
    {
        //**************
        // Outlines
        //**************

        public static void CreateTemp()
        {
            //CreateFloorMesh(map.MapData);
            //CreateWallsMesh();

            //map.WallsMesh.mesh = WallMesh;
            //map.FloorMesh.mesh = FloorMesh;

            //CreateEdgePaths();
            //if (map.VerticalWallSize > 0f)
            //{
            //    CreateVerticalWallsMesh(map.VerticalWallSize);
            //    map.VerticalWallsMesh.mesh = VerticalWallMesh;
            //}
            ////else
            ////{
            ////    map.PolygonCollider.pathCount = EdgePaths.Count;
            ////    for (int i = 0; i < EdgePaths.Count; i++)
            ////    {
            ////        List<Vector3> edges = EdgePaths[i];
            ////        Vector2[] vec2Edges = new Vector2[edges.Count];
            ////        for (int j = 0; j < edges.Count; j++)
            ////            vec2Edges[j] = edges[j];

            ////        map.PolygonCollider.SetPath(i, vec2Edges);
            ////    }
            ////}
        }

        public static MapGenResults Run(MapGenParams inParams)
        {
            if (inParams == null)
                return null;

            MapGenResults results = new MapGenResults();

            if (!inParams.DoBinaryGridMap)
                return results;

            results.BinaryGridMap = CreateBinaryGridMap(inParams.BinaryGridMapRowCount, inParams.BinaryGridMapColumnCount, inParams.BinaryGridMapFillChance, inParams.BinaryGridMapBorderSize);

            if (inParams.DoCellularAutomata)
                CellularAutomata(results.BinaryGridMap, inParams.CellularAutomataIterCount, inParams.CellularAutomataBorderThickness);

            if (!inParams.DoMarchingSquares)
                return results;

            MarchingSquareMeshGen.DeconstructMarchingSquaresToMeshData(results.BinaryGridMap, inParams.MarchingOffset, inParams.MarchingNodeSize, out List<Vector3> vertices, out List<int> indices);
            results.MarchingSquareVertices = vertices;
            results.MarchingSquareIndices = indices;

            results.EdgePaths = GetEdgePaths(ref vertices, ref indices);

            if (inParams.DoCreateWallMesh)
                results.WallMesh = CreateWallsMesh(ref vertices, ref indices);
            if (inParams.DoCreateVerticalWallMesh)
                results.VerticalWallMesh = CreateVerticalWallsMesh(inParams.VerticalWallSize, ref results.EdgePaths);
            if (inParams.DoCreateFloorMesh)
                results.FloorMesh = CreateFloorMesh(results.BinaryGridMap, inParams.MarchingNodeSize, inParams.MarchingOffset);

            if (inParams.DoCreateEdgeColliders)
            {

            }

            return results;
        }

        //************************
        // Binary Grid Map

        public static BinaryGridMap CreateBinaryGridMap(int rowCount, int colCount, float fillChance, int borderSize)
        {
            BinaryGridMap map = new BinaryGridMap(rowCount, colCount);

            if (fillChance > 0)
                map.FillWithRandom(fillChance);
            if (borderSize > 0)
                map.CreateMapBorder(borderSize);

            return map;
        }

        //************************
        // Celular Automata

        public static void CellularAutomata(BinaryGridMap map, int cellularAutomataIterCount, int excludeThickness)
        {
            if (cellularAutomataIterCount <= 0)
                return;

            int currCount = 0;
            while (currCount < cellularAutomataIterCount)
            {
                for (int row = excludeThickness; (row + excludeThickness) < map.RowCount; row++)
                    for (int col = excludeThickness; (col + excludeThickness) < map.ColCount; col++)
                    {
                        int currRow = row;
                        int currCol = col;
                        BinaryGridNode currNode = map.GetNode(currRow, currCol);

                        int neigbourWallCount = 0;
                        int neighbourEmptyCount = 0;

                        for (int i = -1; i <= 1; i++)
                            for (int j = -1; j <= 1; j++)
                            {
                                int neRow = currRow + i;
                                int neCol = currCol + j;

                                if (neRow == neCol)
                                    continue;

                                if (map.GetNode(neRow, neCol).m_type == BinaryNodeType.FULL)
                                    neigbourWallCount++;
                                else
                                    neighbourEmptyCount++;
                            }

                        if (neigbourWallCount > neighbourEmptyCount)
                            currNode.m_type = BinaryNodeType.FULL;
                        else if (neigbourWallCount < neighbourEmptyCount)
                            currNode.m_type = BinaryNodeType.EMPTY;
                    }
                currCount++;
            }
        }

        //************************
        // Meshes

        public static Mesh CreateWallsMesh(ref List<Vector3> vertices, ref List<int> indices)
        {
            Mesh newMesh = new Mesh();
            newMesh.vertices = vertices.ToArray();
            newMesh.triangles = indices.ToArray();
            newMesh.RecalculateNormals();

            return newMesh;
        }

        public static Mesh CreateFloorMesh(BinaryGridMap mapData, float NodeSize, Vector3 gridOffset)
        {
            Vector3 topLeft = mapData.GetNodePos(gridOffset, mapData.RowCount - 1, 0, NodeSize);
            Vector3 topRight = mapData.GetNodePos(gridOffset, mapData.RowCount - 1, mapData.ColCount - 1, NodeSize);
            Vector3 botLeft = mapData.GetNodePos(gridOffset, 0, 0, NodeSize);
            Vector3 botRight = mapData.GetNodePos(gridOffset, 0, mapData.ColCount - 1, NodeSize);

            List<Vector3> floorVertices = new();
            List<int> floorIndices = new();

            floorVertices.Add(topLeft);
            floorVertices.Add(topRight);
            floorVertices.Add(botLeft);
            floorVertices.Add(botRight);

            floorIndices.Add(0);
            floorIndices.Add(1);
            floorIndices.Add(2);

            floorIndices.Add(2);
            floorIndices.Add(1);
            floorIndices.Add(3);

            Mesh floorMesh = new Mesh();
            floorMesh.vertices = floorVertices.ToArray();
            floorMesh.triangles = floorIndices.ToArray();

            return floorMesh;
        }

        public static Mesh CreateVerticalWallsMesh(float verticalWallSize, ref List<List<Vector3>> edgePaths)
        {
            List<Vector3> verticalWallVertices = new();
            List<int> verticalWallIndices = new();

            foreach (List<Vector3> edgePath in edgePaths)
            {
                for (int i = 0; i < edgePath.Count; i++)
                {
                    Vector3 floorPoint = edgePath[i];
                    Vector3 roofPoint = floorPoint - Vector3.back * verticalWallSize;

                    verticalWallVertices.Add(floorPoint);
                    verticalWallVertices.Add(roofPoint);

                    if (i < edgePath.Count - 1)
                    {
                        int floorFirstIndex = verticalWallVertices.Count - 2;
                        int roofFirstIndex = verticalWallVertices.Count - 1;
                        int floorSecIndex = verticalWallVertices.Count;
                        int roofSecIndex = verticalWallVertices.Count + 1;

                        verticalWallIndices.Add(floorFirstIndex);
                        verticalWallIndices.Add(roofFirstIndex);
                        verticalWallIndices.Add(floorSecIndex);
                        verticalWallIndices.Add(floorSecIndex);
                        verticalWallIndices.Add(roofFirstIndex);
                        verticalWallIndices.Add(roofSecIndex);
                    }
                }
            }

            Mesh verticalWallMesh = new Mesh();
            verticalWallMesh.vertices = verticalWallVertices.ToArray();
            verticalWallMesh.triangles = verticalWallIndices.ToArray();

            return verticalWallMesh;
        }

        //***********************
        // Edges

        public static List<List<Vector3>> GetEdgePaths(ref List<Vector3> vertices, ref List<int> indices)
        {
            List<List<Vector3>> EdgePaths = new();

            HashSet<Edge<Vector3>> allEdges = CollectAllOutlineEdges(ref vertices, ref indices);

            Dictionary<Vector3, Vector3> edgesPerVertex = new Dictionary<Vector3, Vector3>();
            foreach (Edge<Vector3> edge in allEdges)
            {
                if (!edgesPerVertex.ContainsKey(edge.A))
                    edgesPerVertex.Add(edge.A, edge.B);
                else
                    Debug.Log("Problem, Outer edge starts 2 times!");
            }

            HashSet<Vector3> usedNodes = new HashSet<Vector3>();
            foreach (Edge<Vector3> edge in allEdges)
            {
                if (usedNodes.Contains(edge.A))
                {
                    continue;
                }
                Vector3 startPos = edge.A;
                Vector3 currentPos = startPos;

                List<Vector3> pathPoints = new List<Vector3>();
                do
                {
                    usedNodes.Add(currentPos);
                    pathPoints.Add(currentPos);
                    currentPos = edgesPerVertex[currentPos];
                } while (currentPos != startPos);

                pathPoints.Add(startPos);
                EdgePaths.Add(pathPoints);
            }

            return EdgePaths;
        }

        private static HashSet<Edge<Vector3>> CollectAllOutlineEdges(ref List<Vector3> vertices, ref List<int> indices)
        {
            HashSet<Edge<Vector3>> singleEdges = new();
            HashSet<Edge<Vector3>> multipleEdges = new();

            for (int i = 0; i < indices.Count - 2; i += 3)
            {
                Vector3 A = vertices[indices[i]];
                Vector3 B = vertices[indices[i + 1]];
                Vector3 C = vertices[indices[i + 2]];

                AddToOutline(new Edge<Vector3>(A, B), ref singleEdges, ref multipleEdges);
                AddToOutline(new Edge<Vector3>(B, C), ref singleEdges, ref multipleEdges);
                AddToOutline(new Edge<Vector3>(C, A), ref singleEdges, ref multipleEdges);
            }

            return singleEdges;
        }

        private static void AddToOutline(Edge<Vector3> edge, ref HashSet<Edge<Vector3>> single, ref HashSet<Edge<Vector3>> multiple)
        {
            if (single.Remove(edge))
                multiple.Add(edge);
            else
                single.Add(edge);
        }


    }

    public class MapGenParams
    {
        public bool DoBinaryGridMap;
        public int BinaryGridMapRowCount;
        public int BinaryGridMapColumnCount;
        public float BinaryGridMapFillChance;
        public int BinaryGridMapBorderSize;

        public bool DoCellularAutomata;
        public int CellularAutomataIterCount;
        public int CellularAutomataBorderThickness;

        public bool DoMarchingSquares;
        public float MarchingNodeSize;
        public Vector3 MarchingOffset;

        public bool DoCreateWallMesh;
        public bool Is3D;
        public bool DoCreateVerticalWallMesh;
        public float VerticalWallSize;

        public bool DoCreateFloorMesh;
        public bool DoCreateRoofMesh;
        public bool DoCreateEdgeColliders;
    }
    public class MapGenResults
    {
        public BinaryGridMap BinaryGridMap;

        public List<Vector3> MarchingSquareVertices;
        public List<int> MarchingSquareIndices;

        public Mesh WallMesh;
        public Mesh VerticalWallMesh;
        public Mesh FloorMesh;
        public Mesh RoofMesh;

        public List<List<Vector3>> EdgePaths;
    }
}

//TODO:....
//fix colliders:
  //-floor
  //walls 2D
  //vertical walls in 3D

//fix room detection
//fix partitioning

//public static void CreateMesh(IMarchingMap map)
//{
//    if (map == null || map.MapData == null)
//        return;

//    Restart();

//    CreateSquareGrid(map, map.VerticalWallSize);
//    MarchingSquaresToMeshes();

//    CreateFloorMesh(map.MapData);
//    CreateWallsMesh();

//    map.WallsMesh.mesh = WallMesh;
//    map.FloorMesh.mesh = FloorMesh;

//    CreateEdgePaths();
//    if (map.VerticalWallSize > 0f)
//    {
//        CreateVerticalWallsMesh(map.VerticalWallSize);
//        map.VerticalWallsMesh.mesh = VerticalWallMesh;
//    }
//    //else
//    //{
//    //    map.PolygonCollider.pathCount = EdgePaths.Count;
//    //    for (int i = 0; i < EdgePaths.Count; i++)
//    //    {
//    //        List<Vector3> edges = EdgePaths[i];
//    //        Vector2[] vec2Edges = new Vector2[edges.Count];
//    //        for (int j = 0; j < edges.Count; j++)
//    //            vec2Edges[j] = edges[j];

//    //        map.PolygonCollider.SetPath(i, vec2Edges);
//    //    }
//    //}
//}
//    }

//static void DetectAndUpdateRooms(MapData mapData, int minRoomSize)
//{
//    //bool doAgain = true;
//    //while (doAgain)
//    //{
//    //    doAgain = false;
//    //    DetectRoomsInArea(mapData, area);
//    //    foreach (Room room in area.m_allRooms)
//    //        if (room.Size < minRoomSize)
//    //        {
//    //            doAgain = true;
//    //            foreach (MapNode node in room.m_roomNodes)
//    //                node.m_type = (node.m_type == MapNodeType.EMPTY) ? MapNodeType.WALL : MapNodeType.EMPTY;
//    //        }
//    //}

//    //List<Room> finalRooms = new List<Room>();
//    //foreach (Room room in area.m_allRooms)
//    //{
//    //    if (room.m_roomType == MapNodeType.EMPTY)
//    //        finalRooms.Add(room);
//    //}
//    //area.m_allRooms = finalRooms;

//}
//static void DetectRoomsInArea(MapData map, MapArea area)
//{
//    //List<Room> allRooms = new List<Room>();
//    //HashSet<MapNode> checkedNodes = new HashSet<MapNode>();

//    //for (int row = area.RowStart; row < area.RowEnd; row++)
//    //    for (int col = area.ColStart; col < area.ColEnd; col++)
//    //    {
//    //        MapNode currNode = map.GetNode(row, col);
//    //        if (checkedNodes.Contains(currNode))
//    //            continue;

//    //        HashSet<MapNode> newRoomNodes = new HashSet<MapNode>();
//    //        Queue<MapNode> queuedNodes = new Queue<MapNode>();
//    //        queuedNodes.Enqueue(currNode);

//    //        while (queuedNodes.Count > 0)
//    //        {
//    //            MapNode node = queuedNodes.Dequeue();
//    //            if (checkedNodes.Contains(node))
//    //                continue;
//    //            newRoomNodes.Add(node);
//    //            checkedNodes.Add(node);
//    //            foreach (var neNode in map.Get4Neighbours(node.m_row, node.m_col))
//    //                if (area.IsInArea(neNode.m_row, neNode.m_col) && !checkedNodes.Contains(neNode) && neNode.m_type == currNode.m_type)
//    //                    queuedNodes.Enqueue(neNode);
//    //        }
//    //        allRooms.Add(new Room(newRoomNodes, currNode.m_type, area, Random.ColorHSV()));
//    //    }
//    //area.m_allRooms = allRooms;

//}
//static void ConnectAreaRooms(MapData map, MapArea area)
//{
//    //TODO
//}

//public static void CreateMapGOs(Map map, MapData mapData)
//{
//    map.m_mapData = mapData;

//    Mesh roofMesh = null;
//    Mesh wallMesh = null;
//    Mesh floorMesh = null;
//    List<List<Vector2>> edgePaths2D = null;

//    map.RoofMesh.sharedMesh = roofMesh;
//    map.WallMesh.sharedMesh = wallMesh;
//    map.FloorMesh.sharedMesh = floorMesh;

//    map.Collider2D.pathCount = edgePaths2D.Count;
//    for (int i = 0; i < edgePaths2D.Count; i++)
//        map.Collider2D.SetPath(i, edgePaths2D[i].ToArray());
//}


//public static void PartitionMap(MapData mapData, int minAreaSize)
//{
//    List<MapArea> finishedAreas = new List<MapArea>();
//    List<MapArea> unfinishedSet = new List<MapArea>();

//    while (unfinishedSet.Count > 0)
//    {
//        MapArea currArea = unfinishedSet[unfinishedSet.Count - 1];
//        unfinishedSet.RemoveAt(unfinishedSet.Count - 1);

//        bool areMoreCols = currArea.ColCount > currArea.RowCount;
//        int bigger = (areMoreCols) ? currArea.ColCount : currArea.RowCount;
//        int smaller = (areMoreCols) ? currArea.RowCount : currArea.ColCount;

//        if (bigger < 2 * minAreaSize)
//        {
//            finishedAreas.Add(currArea);
//        }
//        else
//        {
//            int newBiggerCount1 = Random.Range(minAreaSize, bigger - minAreaSize);
//            int newBiggerCount2 = bigger - newBiggerCount1;

//            MapArea newRoom1;
//            MapArea newRoom2;
//            if (areMoreCols)
//            {
//                newRoom1 = new MapArea(mapData, currArea.RowStart, currArea.ColStart, smaller, newBiggerCount1);
//                newRoom2 = new MapArea(mapData, currArea.RowStart, currArea.ColStart + newBiggerCount1, smaller, newBiggerCount2);
//            }
//            else
//            {
//                newRoom1 = new MapArea(mapData, currArea.RowStart, currArea.ColStart, newBiggerCount1, smaller);
//                newRoom2 = new MapArea(mapData, currArea.RowStart + newBiggerCount1, currArea.ColStart, newBiggerCount2, smaller);
//            }
//            unfinishedSet.Add(newRoom1);
//            unfinishedSet.Add(newRoom2);
//        }
//    }
//}

