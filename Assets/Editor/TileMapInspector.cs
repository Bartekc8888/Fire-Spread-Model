using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMap))]
public class ReloadMap : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Regenerate"))
        {
            TileMap tileMap = (TileMap) target;
            tileMap.BuildMap();
        }
    }
}
