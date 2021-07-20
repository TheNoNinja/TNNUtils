using UnityEngine;

namespace TNNUtils.Random
{
    public static class Perlin
    {
        private static float Interpolate(float from, float to, float weight)
        {
            if (0f > weight) return from;
            if (1f < weight) return to;
            
            return (to - from) * ((weight * (weight * 6f - 15f) + 10f) * weight * weight * weight) + from;
        }

        private static Vector2 RandomGradient(int x, int y)
        {
            var random = 2920f * Mathf.Sin(x * 21942f + y * 171324f + 8912f) * Mathf.Cos(x * 23157 * y * 217832 + 9758);

            return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        }

        private static float DotGridGradient(int x, int y, float xOffset, float yOffset)
        {
            var gradient = RandomGradient(x, y);

            var xDistance = xOffset - x;
            var yDistance = yOffset - y;

            return xDistance * gradient.x + yDistance * gradient.y;
        }

        public static float PerlinNoise(float x, float y)
        {
            var x0 = (int)x;
            var x1 = x0 + 1;
            var y0 = (int)y;
            var y1 = y0 + 1;

            var xWeight = x - x0;
            var yWeight = y - y0;
            
            var xValue00 = DotGridGradient(x0, y0, x, y);
            var xValue01 = DotGridGradient(x1, y0, x, y);
            var xValue0 = Interpolate(xValue00, xValue01, xWeight);

            var xValue10 = DotGridGradient(x0, y1, x, y);
            var xValue11 = DotGridGradient(x1, y1, x, y);
            var xValue1 = Interpolate(xValue10, xValue11, xWeight);

            return (Interpolate(xValue0, xValue1, yWeight) + 1) / 2;
        }
    }
}