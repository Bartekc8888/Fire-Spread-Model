using Simulation;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimulationInstance))]
public class ReloadMap : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Regenerate"))
        {
            SimulationInstance simulationInstance = (SimulationInstance) target;
            simulationInstance.RebuildSimulation();
        }
    }
}
