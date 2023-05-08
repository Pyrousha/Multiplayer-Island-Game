using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TileSpawner))]
public class TilleSpawnerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TileSpawner tileSpawner = (TileSpawner)target;

        if (GUILayout.Button("Generate Map"))
        {
            tileSpawner.SpawnTiles();
        }
    }
}
#endif