using Data;
using System;
using UnityEngine;

namespace Simulation
{
    public class SimulationVariablesCalculator
    {
        private MaterialProperties materialProperties;

        public float reactionIntensity;
        public float optimumReactionVelocity;
        public float maximumReactionVelocity;
        public float optimumPackingRatio;
        public float packingRatio;
        public float ovenDryBulkDensity;
        public float netFuelLoad;

        public float moistureDampingCoefficient;
        public float mineralDampingCoefficient;

        public float propagatingFluxRatio;
        public float windFactor;
        public float slopeFactor;
        public float effectiveHeatingNumber;
        public float heatOfPreignition;

        public float RateOfSpread { get; set; }

        public SimulationVariablesCalculator(Data.TerrainData terrainData)
        {
            this.materialProperties = terrainData.MaterialProperties;
        }
        public void CalculateVariables(float slopeSteepness, float moistureContent, float windSpeed)
        {
            netFuelLoad = CalculateNetFuelLoad();
            ovenDryBulkDensity = CalculateOvenDryBulkDensity();
            packingRatio = CalculatePackingRatio();
            optimumPackingRatio = CalculateOptimumPackingRatio();
            maximumReactionVelocity = CalculateMaximumReactionVelocity();
            optimumReactionVelocity = CalculateOptimumReactionVelocity();

            moistureDampingCoefficient = CalculateMoistureDampingCoefficient(moistureContent);
            mineralDampingCoefficient = CalculateMineralDampingCoefficient();

            reactionIntensity = CalculateReactionIntensity();
            propagatingFluxRatio = CalculatePropagatingFluxRatio();
            windFactor = CalculateWindFactor(windSpeed);
            slopeFactor = CalculateSlopeFactor(slopeSteepness);
            effectiveHeatingNumber = CalculateEffectiveHeatingNumber();
            heatOfPreignition = CalculateHeatOfPreignition(moistureContent);

            RateOfSpread = CalculateRateOfSpread();
        }


        private float CalculateRateOfSpread()
        {
            return reactionIntensity * propagatingFluxRatio * (1.0f + windFactor + slopeFactor)
                /(ovenDryBulkDensity * effectiveHeatingNumber * heatOfPreignition);
        }

        private float CalculateReactionIntensity()
        {
            return optimumReactionVelocity * netFuelLoad * materialProperties.LowHeatContent 
                * moistureDampingCoefficient * mineralDampingCoefficient;
        }

        private float CalculateNetFuelLoad()
        {
            return materialProperties.OvenDryFuelLoad * (1.0f - materialProperties.TotalMineralContent);
        }

        private float CalculateOvenDryBulkDensity()
        {
            return materialProperties.OvenDryFuelLoad / materialProperties.FuelBedDepth;
        }

        private float CalculatePackingRatio()
        {
            return ovenDryBulkDensity / materialProperties.OvenDryParticleDensity;
        }

        private float CalculateOptimumPackingRatio()
        {
            return 3.348f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, -0.8189f);
        }

        private float CalculateMaximumReactionVelocity()
        {
            float ratioPower = Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 1.5f);
            return ratioPower * Mathf.Pow((495.0f + 0.0594f * ratioPower), -1.0f);
        }

        private float CalculateOptimumReactionVelocity()
        {
            float a = 133.0f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, -0.7913f);
            return maximumReactionVelocity * Mathf.Pow(packingRatio / optimumPackingRatio, a)
                * Mathf.Exp(a * (1 - packingRatio / optimumPackingRatio));
        }

        private float CalculateMoistureDampingCoefficient(float moistureContent)
        {
            float rm = moistureContent / materialProperties.DeadFuelMoistureOfExtinction;
            rm = rm > 1.0f ? 1.0f : rm;

            return 1.0f - 2.59f * rm + 5.11f * Mathf.Pow(rm, 2.0f) - 3.52f * Mathf.Pow(rm, 3.0f);
        }

        private float CalculateMineralDampingCoefficient()
        {
            float ns = 0.174f * Mathf.Pow(materialProperties.EffectiveMineralContent, -0.19f);

            return ns > 1.0f ? 1.0f : ns;
        }

        private float CalculatePropagatingFluxRatio()
        {
            return Mathf.Pow(192.0f + 0.2595f * materialProperties.SurfaceAreaToVolumeRatio, -1.0f)
                * Mathf.Exp(0.792f + 0.681f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.5f)
                * (packingRatio + 0.1f));
        }

        private float CalculateWindFactor(float windVelocity)
        {
            float c = 7.47f * Mathf.Exp(-0.133f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.55f));
            float b = 0.02526f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.54f);
            float e = 0.715f * Mathf.Exp(-3.59f * 0.0001f * materialProperties.SurfaceAreaToVolumeRatio);

            return c * Mathf.Pow(windVelocity, b) 
                * Mathf.Pow(packingRatio / optimumPackingRatio, -1.0f * e);
        }

        private float CalculateSlopeFactor(float slopeSteepness)
        {
            return 5.275f * Mathf.Pow(packingRatio, -0.3f) * Mathf.Pow(slopeSteepness, 2.0f);
        }

        private float CalculateEffectiveHeatingNumber()
        {
            return Mathf.Exp(-138.0f / materialProperties.SurfaceAreaToVolumeRatio);
        }

        private float CalculateHeatOfPreignition(float moistureContent)
        {
            return 250.0f + 1116.0f * moistureContent;
        }
    }
}
