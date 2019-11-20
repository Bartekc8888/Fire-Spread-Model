using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalData
{
    public float MoistureContent { get; }
    public float WindVelocity { get; }

    public EnvironmentalData(float moistureContent, float windVelocity)
    {
        MoistureContent = moistureContent;
        WindVelocity = windVelocity;
    }
}
