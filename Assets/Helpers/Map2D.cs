namespace Dawnfall.Helper
{
    [System.Serializable]
    public class Map2D
    {
        public float[] m_noiseMap;
        public int m_width;
        public int m_height;

        public Map2D(float[] noiseMap, int width, int height)
        {
            m_noiseMap = noiseMap;
            m_width = width;
            m_height = height;
        }
    }
}