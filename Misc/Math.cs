namespace TNNUtils.Misc
{
    public static class Math
    {
        public static float Lerp(float from, float to, float weight)
        {
            return (to - from) * weight + from;
        }

        public static float EaseNormal(float value)
        {
            return value * value * value * (value * (value * 6 - 15) + 10);
        } 
    }
}