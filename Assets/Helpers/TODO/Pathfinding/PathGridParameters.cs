using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PathfindingParameters/PathGrid")]
public class PathGridParameters : ScriptableObject
{
    public Vector3 areaSize;
    public float nodeRadius;
    public float aditionalBorder;
    public LayerMask unwalkableMask;
}
