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

        private bool startedFire = false;
        private SimulationVariablesCalculator simulationVariablesCalculator;
        private int _simulationSpeed = 30_000;
        
        void Start()
        {
            RebuildSimulation();
            simulationVariablesCalculator = new SimulationVariablesCalculator();
        }

        private void FixedUpdate()
        {
            if(startedFire)
            {
                List<TileData> burningTiles = GetBurningTiles();
                
                foreach (TileData tile in burningTiles)
                {
                    List<TileData> neighbours = GetUnburningNeighbours(tile);

                    BurnNeighbours(tile, neighbours);
                }
            }
        }

        public void RebuildSimulation()
        {
            GenerateData();
            _tileMap = gameObject.GetComponent<TileMap>();
            _tileMap.InitTileMap(sizeX, sizeY, _tileMapData);
        }

        public void SetSimulationSpeed(int newSimulationSpeed)
        {
            _simulationSpeed = newSimulationSpeed;
            Debug.Log("New simulation speed: " + newSimulationSpeed);
        }

        private void GenerateData()
        {
            MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY, maxHeight, generatorFrequency);
            _tileMapData = mapDataGenerator.GenerateMapData();
        }

        public void StartFire()
        {
            TileData selectedTile = tileGuiInfo.CurrentlySelectedTile;
            BurnTile(selectedTile);
            startedFire = true;

        }

        private float CalculateSlopeSteepness(TileData selectedTile, TileData anotherTile)
        {
            float slopeSteepness = (anotherTile.TerrainData.Height - selectedTile.TerrainData.Height)
                  / _tileMap.tileSize;

            return slopeSteepness;
        }

        private float CalculateWholeDistance(TileData selectedTile, TileData anotherTile)
        {
            double xDistance = Math.Pow(selectedTile.PositionX - anotherTile.PositionX, 2);
            double yDistance = Math.Pow(selectedTile.PositionY - anotherTile.PositionY, 2);
            float distance = _tileMap.tileSize * (float) Math.Sqrt(xDistance + yDistance);

            return distance;
        }

        private List<TileData> GetUnburningNeighbours(TileData selectedTile)
        {
            List<TileData> neighbours = new List<TileData>();
            for (int x = selectedTile.PositionX - 1; x <= selectedTile.PositionX + 1; x++)
            {
                if (x < 0 || x >=_tileMap._tileMapData.GetTileData().Length)
                    continue;

                for (int y = selectedTile.PositionY - 1; y <= selectedTile.PositionY + 1; y++)
                {
                    if (y < 0 || y >= _tileMap._tileMapData.GetTileData()[x].Length)
                        continue;
                    if (x == selectedTile.PositionX && y == selectedTile.PositionY)
                        continue;

                    if(!_tileMapData.GetTileData(x, y).IsBurning)
                    {
                        neighbours.Add(_tileMapData.GetTileData(x, y));
                    }
                }
            }

            return neighbours;
        }

        private List<TileData> GetBurningTiles()
        {
            List<TileData> burning = new List<TileData>();

            foreach (TileData[] x in _tileMap._tileMapData.GetTileData())
            {
                foreach (TileData y in x)
                {
                    if(y.IsBurning)
                    {
                        burning.Add(y);
                    }
                }
            }

            return burning;
        }

        private void BurnTile(TileData tile)
        {
            tile.IsBurning = true;
            tile.TerrainData.Type = TerrainType.Burning;
            _tileMap.UpdateTexture(tile);
        }

        private void BurnNeighbours(TileData tile, List<TileData> neighbours)
        {
            foreach (TileData neighbour in neighbours)
            {
                float deltaDistance = CalculateBurnedDistance(tile, neighbour);
                float wholeDistance = CalculateWholeDistance(tile, neighbour);

                string neighbourKey = neighbour.PositionX.ToString() + neighbour.PositionY.ToString();

                if (tile.FireSpreadingDistance.ContainsKey(neighbourKey))
                {
                    tile.FireSpreadingDistance[neighbourKey] += deltaDistance;

                }
                else
                {
                    tile.FireSpreadingDistance.Add(neighbourKey, deltaDistance);
                }

                if (tile.FireSpreadingDistance[neighbourKey] >= wholeDistance)
                {
                    BurnTile(neighbour);
                }
            }
        }

        private float CalculateBurnedDistance(TileData tile, TileData neighbour)
        {
            float slopeSteepness = CalculateSlopeSteepness(tile, neighbour);
            simulationVariablesCalculator.CalculateVariables(neighbour.TerrainData, slopeSteepness,
                moistureContent, windSpeed);

            float timeSinceLastFrame = Time.deltaTime / 60000.0f;
            float burningSpeed = simulationVariablesCalculator.RateOfSpread;

            float deltaDistance = timeSinceLastFrame * burningSpeed * _simulationSpeed;

            return deltaDistance;
        }
    }
}