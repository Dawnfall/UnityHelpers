using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dawnfall.Helper.MapGen;
using Codice.Client.BaseCommands;
using Dawnfall.Helper;

public class Map : MonoBehaviour
{
    //***********************
    // Input Params
    //***********************

    [Range(1, 200)] public int m_rowCount = 100;
    [Range(1, 200)] public int m_colCount = 100;
    public float m_nodeSize = 1f;
    public Vector3 Offset = Vector3.zero;

    public bool DoPartion = true;
    [Range(1, 1000)] public int minPartitionSize = 25;
    [Range(0, 30)] public int borderSize = 2;
    [Range(0, 100)] public float fillChance = 0.65f;
    [Range(1, 20)] public int cellAutomataIterCount = 5;

    [SerializeField] private float m_wallSize = 0f;

    public float VerticalWallSize => m_wallSize;

    //***********************
    // Results
    //***********************

    public MapGenResults Results = null;

    //***********************
    // References
    //***********************

    [SerializeField] MeshFilter m_wallsMesh;
    public MeshFilter WallsMesh { get { return m_wallsMesh; } }

    [SerializeField] MeshFilter m_verticalWallsMesh;
    public MeshFilter VerticalWallsMesh { get { return m_verticalWallsMesh; } }

    [SerializeField] MeshFilter m_floorMesh;
    public MeshFilter FloorMesh { get { return m_floorMesh; } }

    [SerializeField] PolygonCollider2D m_polygonCollider;
    public PolygonCollider2D PolygonCollider { get { return m_polygonCollider; } }

    [SerializeField] MeshCollider m_wallCollider;
    public MeshCollider WallCollider { get { return m_wallCollider; } }

    public void Clear()
    {
        WallsMesh.mesh = null;
        VerticalWallsMesh.mesh = null;
        FloorMesh.mesh = null;
    }

    public void Generate()
    {
        Clear();

        MapGenParams newParams = new MapGenParams()
        {
            DoBinaryGridMap = true,
            BinaryGridMapRowCount = m_rowCount,
            BinaryGridMapColumnCount = m_colCount,
            BinaryGridMapFillChance = fillChance,
            BinaryGridMapBorderSize = borderSize,

            DoCellularAutomata = true,
            CellularAutomataIterCount = cellAutomataIterCount,
            CellularAutomataBorderThickness = borderSize,

            DoMarchingSquares = true,
            MarchingOffset = Offset,
            MarchingNodeSize = m_nodeSize,

            DoCreateWallMesh = true,

            DoCreateVerticalWallMesh = true,
            VerticalWallSize = VerticalWallSize,

            DoCreateFloorMesh = true,
            DoCreateRoofMesh = true,

            DoCreateEdgeColliders = true,
        };

        Results = MapGenUtils.Run(newParams);

        WallsMesh.mesh = Results.WallMesh;
        FloorMesh.mesh = Results.FloorMesh;
        VerticalWallsMesh.mesh = Results.VerticalWallMesh;

        //PolygonCollider.pathCount = 0;
    }



    #region gizmos
    //**************
    // GIZMOS
    //**************
    [SerializeField] bool m_doDrawGizmos = false;

    private void OnDrawGizmos()
    {
        if (Results == null || !m_doDrawGizmos)
            return;

        if (Results.BinaryGridMap != null)
        {
            for (int row = 0; row < Results.BinaryGridMap.RowCount; row++)
                for (int col = 0; col < Results.BinaryGridMap.ColCount; col++)
                {
                    BinaryGridNode currNode = Results.BinaryGridMap.GetNode(row, col);
                    Gizmos.color = (currNode.m_type == BinaryNodeType.FULL) ? Color.black : Color.white;
                    Gizmos.DrawCube(Results.BinaryGridMap.GetNodePos(Offset, row, col, m_nodeSize), new Vector3(m_nodeSize - 0.1f, m_nodeSize - 0.1f, 0.1f)); //TODO 3d,2d
                }
        }

        if (Results.EdgePaths != null)
        {
            Gizmos.color = Color.white;
            foreach (List<Vector3> path in Results.EdgePaths)
                for (int i = 0; i < path.Count - 1; i++)
                    Gizmos.DrawLine(path[i], path[i + 1]);

        }
    }
    #endregion
}
