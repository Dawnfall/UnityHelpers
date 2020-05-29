using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public static class HelperMath
    {
        public static float tanH(float x)
        {
            return (1.0f - Mathf.Exp(-2.0f * x)) / (1 + Mathf.Exp(-2.0f * x));
        }

        public static Vector3[] GetArkVectors(Vector3 forward, Vector3 up, float angle, int rayCount)
        {
            Vector3[] allDir = new Vector3[rayCount];

            float deltaAngle = angle / (rayCount - 1);
            for (int i = 0; i < rayCount; i++)
            {
                float currAngle = -angle / 2f + deltaAngle * i;
                allDir[i] = Quaternion.AngleAxis(currAngle, up) * forward;
            }

            return allDir;
        }
    }
}