using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper.Pathfinding
{
    public interface IPathable<T> //may not be needed
    {
        Vector3 WorldPosition { get; }
        bool IsWalkable { get; }
    }

    public interface INavigatable<T>
    {
        int NodeCount { get; }
        List<T> GetNeighbours(T node);
        int GetDistance(T nodeA, T nodeB);

        Vector3 GetPosition(T node);
        bool IsWalkable(T node);
    }

    public interface IHeuristical<T>
    {
        float Hcost { get; set; }
        float Gcost { get; set; }
        float Fcost { get; }
        T Parent { get; set; }
    }

    public interface IGridable<T>
    {
        int Row { get; }
        int Col { get; }
    }
}