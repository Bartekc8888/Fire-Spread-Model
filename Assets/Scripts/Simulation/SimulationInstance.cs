using Data;
using Graphics;
using UnityEngine;

namespace Simulation
{
    public class SimulationInstance : MonoBehaviour
    {
        public int sizeX = 100;
        public int sizeY = 100;
        public float maxHeight = 2.0f;
        public int generatorFrequency = 10;

        public float windSpeed = 20.0f;

        public TileGuiInfo tileGuiInfo;

        private TileMap _tileMap;
        private TileMapData _tileMapData;
        
        void Start()
        {
            RebuildSimulation();
        }

        public void RebuildSimulation()
        {
            GenerateData();
            _tileMap = gameObject.GetComponent<TileMap>();
            _tileMap.InitTileMap(sizeX, sizeY, _tileMapData);
        }

        private void GenerateData()
        {
            MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY, maxHeight, generatorFrequency, windSpeed);
            _tileMapData = mapDataGenerator.GenerateMapData();
        }

        public void StartFire()
        {
            TileData selectedTile = tileGuiInfo.CurrentlySelectedTile;
            
        }
    }
}