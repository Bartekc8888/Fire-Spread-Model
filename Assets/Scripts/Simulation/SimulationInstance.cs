using Data;
using Graphics;
using UnityEngine;

namespace Simulation
{
    public class SimulationInstance : MonoBehaviour
    {
        public int sizeX = 100;
        public int sizeY = 100;

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
            MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY);
            _tileMapData = mapDataGenerator.GenerateMapData();
        }
    }
}