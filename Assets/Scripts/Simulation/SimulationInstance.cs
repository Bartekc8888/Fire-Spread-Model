using System;
using System.Collections;
using System.Collections.Generic;
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
        public float moistureContent = 0.7f;

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
            MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY, maxHeight, generatorFrequency);
            _tileMapData = mapDataGenerator.GenerateMapData();
        }

        public void StartFire()
        {
            TileData selectedTile = tileGuiInfo.CurrentlySelectedTile;
            selectedTile.IsBurning = true;

            SimulationVariablesCalculator variablesCalculator = new SimulationVariablesCalculator(selectedTile.TerrainData);

            List<float> slopeSteepnesses = CalculateSlopeSteepnesses(selectedTile);


            foreach(float steepness in slopeSteepnesses)
            {
                variablesCalculator.CalculateVariables(steepness, moistureContent, windSpeed);
                Debug.Log(variablesCalculator.RateOfSpread);
            }
        }

        private List<float> CalculateSlopeSteepnesses(TileData selectedTile)
        {
            List<float> steepnesses = new List<float>();
            for (int x = selectedTile.PositionX - 1; x <= selectedTile.PositionX + 1; x++)
            {
                if ( x < 0)
                    continue;

                for(int y = selectedTile.PositionY - 1; y <= selectedTile.PositionY + 1; y++ )
                {
                    if (y < 0)
                        continue;
                    if (x == selectedTile.PositionX && y == selectedTile.PositionY)
                        continue;

                    float slopeSteepness = (_tileMapData.GetTileData(x, y).TerrainData.Height - selectedTile.TerrainData.Height)
                        / 20.0f;

                    steepnesses.Add(slopeSteepness);
                }
            }

            return steepnesses;
        }
    }
}