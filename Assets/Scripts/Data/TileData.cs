﻿namespace Data
{
    public class TileData
    {
        public int PositionX { get; }
        public int PositionY { get; }

        public TerrainData TerrainData { get; }

        public bool IsBurning { get; set; }

        public TileData(int positionX, int positionY, TerrainData terrainData)
        {
            PositionX = positionX;
            PositionY = positionY;
            TerrainData = terrainData;
            IsBurning = false;
        }


    }
}