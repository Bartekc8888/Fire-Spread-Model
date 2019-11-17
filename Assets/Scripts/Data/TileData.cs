namespace Data
{
    public class TileData
    {
        private int _positionX;
        private int _positionY;
        public TerrainData TerrainData { get; }

        public TileData(int positionX, int positionY, TerrainData terrainData)
        {
            _positionX = positionX;
            _positionY = positionY;
            TerrainData = terrainData;
        }
    }
}