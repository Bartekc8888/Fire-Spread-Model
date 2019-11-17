using System;
using Data;

namespace Graphics
{
    public class MapDataGenerator
    {
        private readonly int _sizeX;
        private readonly int _sizeY;

        public MapDataGenerator(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
        }
        
        public TileMapData GenerateMapData()
        {
            TileMapData tileMapData = new TileMapData(_sizeX, _sizeY);
            InitTiles(tileMapData);

            return tileMapData;
        }

        private void InitTiles(TileMapData tileMapData)
        {
            Array terrainTypes = Enum.GetValues(typeof(TerrainType));
            Random random = new Random();

            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    TerrainType terrainType = (TerrainType)terrainTypes.GetValue(random.Next(terrainTypes.Length));
                    TerrainData terrainData = new TerrainData(terrainType);
                    tileMapData.SetTileData(x, y, new TileData(x, y, terrainData));
                }
            }
        }
    }
}