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

        //list must not be empty
        public static Vector3 GetClosestPoint(IList<Vector3> list, Vector3 point)
        {
            if (list == null || list.Count == 0)
                throw new System.Exception("list empty!");

            float minDistSqr = Mathf.Infinity;
            Vector3 closest = new Vector3();

            foreach (var item in list)
            {
                float newDistSqr = (point - item).sqrMagnitude;
                if (newDistSqr < minDistSqr)
                {
                    minDistSqr = newDistSqr;
                    closest = item;
                }
            }

            return closest;
        }

        public static Vector3 PerturbHor(Vector3 position, Texture2D perturbTexture, float noiseScale, float strength)
        {
            Vector4 noise = perturbTexture.GetPixelBilinear(position.x * noiseScale, position.z * noiseScale);
            position.x += (noise.x * 2f - 1) * strength;
            position.z += (noise.z * 2f - 1) * strength;
            return position;
        }

    }
}