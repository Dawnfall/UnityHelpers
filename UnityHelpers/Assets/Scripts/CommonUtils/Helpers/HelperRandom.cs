﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mesh;

namespace helper
{
    public static class HelperRandom
    {
        public static Vector2Int GetRandomDiscreteCoordInSquare(Vector2Int lowerLeft, Vector2Int topRight)
        {
            return new Vector2Int(Random.Range(lowerLeft.x, topRight.x + 1), Random.Range(lowerLeft.y, topRight.y + 1));
        }

        public static Vector2 GetRandomPositionInSquare(Vector2 lowerLeft, Vector2 topRight)
        {
            return new Vector2(Random.Range(lowerLeft.x, topRight.x), Random.Range(lowerLeft.y, topRight.y));
        }

        //this changes the ingoing list
        public static List<T> getRandomFromList<T>(IList<T> list, int amount, bool doAllowMultiple)
        {
            List<T> randomList = new List<T>();

            if (doAllowMultiple)
                for (int i = 0; i < amount; i++)
                {
                    randomList.Add(list[Random.Range(0, list.Count)]);
                }
            else
                for (int i = 0; i < amount; i++)
                {
                    int index = Random.Range(0, list.Count - i);
                    randomList.Add(list[index]);
                    list[index] = list[list.Count - i - 1];
                }

            return randomList;
        }

        public static T getRandomFromList<T>(IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static NoiseMap2D Generate2DPerlinNoise(int width, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, System.Random randomGen)
        {
            if (scale <= 0)
                scale = 0.0001f;

            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = randomGen.Next(-1000, 1000) + offset.x;
                float offsetY = randomGen.Next(-1000, 1000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2.0f;
            float halfHeight = height / 2.0f;

            float[] noiseMap = new float[width * height];
            for (int row = 0; row < height; row++)
                for (int col = 0; col < width; col++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0.0f;
                    for (int oct = 0; oct < octaves; oct++)
                    {
                        float sampleCol = (col - halfWidth) / scale * frequency + octaveOffsets[oct].x;
                        float sampleRow = (row - halfHeight) / scale * frequency + octaveOffsets[oct].y;

                        float perlinValue = Mathf.PerlinNoise(sampleCol, sampleRow) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;
                    noiseMap[row * width + col] = noiseHeight;
                }

            for (int row = 0; row < height; row++)
                for (int col = 0; col < width; col++)
                {
                    noiseMap[row * width + col] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[row * width + col]);
                }

            return new NoiseMap2D(noiseMap, width, height);
        }

    }
}
