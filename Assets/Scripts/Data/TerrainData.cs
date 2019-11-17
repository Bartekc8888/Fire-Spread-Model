namespace Data
{
    public class TerrainData
    {
        public TerrainType Type { get; }
        public MaterialProperties MaterialProperties { get; }
        public float Height { get; }

        public TerrainData(TerrainType type, float height, MaterialProperties materialProperties)
        {
            Type = type;
            Height = height;
            MaterialProperties = materialProperties;
        }
    }
}