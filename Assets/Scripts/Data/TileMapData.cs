namespace Data
{
    public class TileMapData
    {
        private TileData[][] _tilesData;

        public TileMapData(int sizeX, int sizeY)
        {
            _tilesData = new TileData[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                _tilesData[i] = new TileData[sizeY];
            }
        }
        
        public void SetTileData(int posX, int posY, TileData tileData)
        {
            _tilesData[posX][posY] = tileData;
        }

        public TileData GetTileData(int posX, int posY)
        {
            return _tilesData[posX][posY];
        }
    }
}