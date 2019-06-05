using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helper
{
    [System.Serializable]
    public class SerVector
    {
        public float x, y, z, w;
        public int xi, yi, zi;

        public SerVector(Vector2 vec)
        {
            x = vec.x;
            y = vec.y;
        }
        public SerVector(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
        public SerVector(Vector4 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
            w = vec.w;

        }
        public SerVector(Vector2Int vec)
        {
            xi = vec.x;
            yi = vec.y;
        }
        public SerVector(Vector3Int vec)
        {
            xi = vec.x;
            yi = vec.y;
            zi = vec.z;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
        public Vector4 ToVector4()
        {
            return new Vector4(x, y, z, w);
        }
        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(xi, yi);
        }
        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(xi, yi, zi);
        }
    }

    [System.Serializable]
    public class SerQuaternion
    {
        public float x, y, z, w;

        public SerQuaternion(Quaternion quat)
        {
            x = quat.x;
            y = quat.y;
            z = quat.z;
            w = quat.w;
        }
        
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}