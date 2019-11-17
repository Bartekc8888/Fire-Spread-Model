using System;

namespace Data
{
    public class MaterialPropertiesFactory
    {
        public static MaterialProperties GetProperties(TerrainType type, float windVelocity)
        {
            switch (type)
            {
                case TerrainType.Grass:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 
                        512.576f, 114.8293f, 0.1659f, 0.3048f,
                        0.12f, 0.7f, windVelocity); // Short grass
                case TerrainType.Trees:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 
                        512.576f, 65.6167f, 1.8157f, 0.1219f,
                        0.25f, 0.7f, windVelocity); // Large downed logs
                case TerrainType.Bushes:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 
                        512.576f, 52.4934f, 0.67251f, 0.3048f,
                        0.40f, 0.7f, windVelocity); // Moderate load, humid climate shrub 
                case TerrainType.HighGrass:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 
                        512.576f, 49.2125f, 0.67251f, 0.762f,
                        0.25f, 0.7f, windVelocity); // Tall grass
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}