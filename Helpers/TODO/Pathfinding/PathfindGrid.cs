using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper.Pathfinding
{
    public class PathfindGrid //: AMonoSingleton<PathfindGrid>
    {
        Vector3 WorldBottomLeft;
        float nodeRadius;

        Vector2Int Size = Vector2Int.zero;
        public int NodeCount
        {
            get
            {
                return Size.x * Size.y;
            }
        }
        public float[] Data;

        public List<Vector2Int> Get4Neighbours(Vector2Int coords)
        {
            List<Vector2Int> neigbours = new List<Vector2Int>();
            for (int row = -1; row <= 1; row++)
                for (int col = -1; col <= 1; col++)
                {
                    if (row == col)
                        continue;
                    Vector2Int ne = new Vector2Int(coords.x + row, coords.y + col);
                    if (IsValidCoords(ne))
                        neigbours.Add(ne);
                }
            return neigbours;
        }
        public List<Vector2Int> Get8Neighbours(Vector2Int coords)
        {
            List<Vector2Int> neigbours = new List<Vector2Int>();
            for (int row = -1; row <= 1; row++)
                for (int col = -1; col <= 1; col++)
                {
                    if (row == 0 && col == 0)
                        continue;

                    Vector2Int ne = new Vector2Int(coords.x + row, coords.y + col);
                    if (IsValidCoords(ne))
                        neigbours.Add(ne);
                }
            return neigbours;
        }
        public bool IsWalkable(Vector2Int coords)
        {
            if (IsValidCoords(coords))
            {
                if (Data[coords.x * Size.y + coords.y] == 1)
                    return true;//TODO:....
            }
            return false;
        }

        public Vector3 CoordsToWorld(Vector2Int coords)
        {
            return WorldBottomLeft + new Vector3((coords.x + 0.5f) * nodeRadius, 0, (coords.x + 0.5f) * nodeRadius);
        }
        public Vector2Int WorldToCoords(Vector3 point)
        {
            float deltaX = point.x - WorldBottomLeft.x;
            float deltaZ = point.z - WorldBottomLeft.z;

            int col = (int)(deltaX / (nodeRadius * 2.0f));
            int row = (int)(deltaZ / (nodeRadius * 2.0f));

            return new Vector2Int(col, row); //TODO;
        }

        public bool IsValidCoords(Vector2Int coords)
        {
            return coords.x >= 0 && coords.y >= 0 && coords.x < Size.x && coords.y < Size.y;
        }
        public int GetDistance(Vector2Int a, Vector2Int b)
        {
            int deltaRow = Mathf.Abs(a.x - b.x);
            int deltaCol = Mathf.Abs(a.y - b.y);

            int bigger = Mathf.Max(deltaRow, deltaCol);
            int smaller = Mathf.Min(deltaRow, deltaCol);

            return smaller * 14 + (bigger - smaller) * 10;
        }
        private void OnDrawGizmos()
        {
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                {
                    Vector2Int coords = new Vector2Int(x, y);

                    if (IsWalkable(coords))
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;

                    Gizmos.DrawCube(new Vector3(x + 0.5f, y + 0.5f, 0), Vector3.one * 0.9f);
                }

            Gizmos.color = Color.white;
        }
    }
}
//public void InitGrid()
//{
//    //LevelManager levelManager = LevelManager.Instance;

//    //Size = levelManager.Size;

//    //Data = new float[levelManager.TileCount];
//    //for (int x = 0; x < levelManager.Size.x; x++)
//    //    for (int y = 0; y < levelManager.Size.y; y++)
//    //    {
//    //        int currIndex = x * Size.y + y;
//    //        Data[currIndex] = (levelManager.GridController.WallTilemap.IsWall(new Vector3Int(x, y, 0))) ? 0f : 1f;
//    //    }
//}

//public void UpdateGrid()
//{
//    //TODO: some smart way of updating only part of grid
//}
