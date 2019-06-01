using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helper
{
    public static class HelperOther
    {
        public static float tanH(float x)
        {
            return (1.0f - Mathf.Exp(-2.0f * x)) / (1 + Mathf.Exp(-2.0f * x));
        }
        public static float linearFun(float x, float k, float n0)
        {
            return k * x + n0;
        }
        public static float temperatureTanHDecrease(float temperature, float distance)
        {
            return temperature * (1.0f - tanH(-distance));
        }
        public static float temperatureLinDecrease(float temperature, float distance, float coeficient)
        {
            return Mathf.Clamp(linearFun(distance, coeficient, temperature), 0.0f, temperature);
        }



    }
}