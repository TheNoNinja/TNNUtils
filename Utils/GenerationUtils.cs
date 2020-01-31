using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationUtils
{
    public static float[,] GeneratePerlin(int width, int height, float scale = 1.1f)
    {
        int originX = Random.Range(0, 99999);
        int originY = Random.Range(0, 99999);

        return GeneratePerlin(width, height, originX, originY, scale);
    }

    public static float[,] GeneratePerlin(float width, float height, float originX, float originY, float scale = 1.1f)
    {
        float[,] values = new float[(int)width, (int)height];

        scale = Mathf.Floor(scale) + 0.1f;

        for (float x = 0f; x < width; x++)
        {
            for (float y = 0f; y < height; y++)
            {
                values[(int)x, (int)y] = Mathf.PerlinNoise(originX + x / width * scale,
                                                           originY + y / height * scale);
            }
        }
        return values;
    }

    public static float[,] Fbm(float width, float height, int octaves, float scale = 1.1f)
    {
        int originX = Random.Range(0, 99999);
        int originY = Random.Range(0, 99999);

        return Fbm(width, height, octaves, scale, originX, originY);
    }

    public static float[,] Fbm(float width, float height,  int octaves, float scale, float originX, float originY)
    {
        float[,] values = new float[(int)width, (int)height];

        scale = Mathf.Floor(scale) + 0.1f;

        float l = 2.0f;
        float g = 0.5f;
        float a = 0.5f;

        for (float i = 0; i < octaves; i++)
        {
            for (float x = 0; x < width; x++)
            {
                for (float y = 0; y < height; y++)
                {
                    values[(int)x, (int)y] += a * Mathf.PerlinNoise(originX + x / width * scale,
                                                                    originY + y / height * scale);
                }
            }
            scale *= l;
            a *= g;
        }

        return values;
    }
}
