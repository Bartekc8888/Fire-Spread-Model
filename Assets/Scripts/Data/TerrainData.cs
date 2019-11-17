namespace Data
{
    public class TerrainData
    {
        public TerrainType Type { get; }
        public float Height { get; }

        public TerrainData(TerrainType type, float height)
        {
            Type = type;
            Height = height;
        }
    }
}