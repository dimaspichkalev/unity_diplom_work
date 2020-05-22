using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProcBuilding))]
public class ProcBuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProcBuilding proGen = (ProcBuilding)target;

        if (GUILayout.Button("Generate"))
        {
            ColorStorage.RefreshWallColor();
            proGen.Generate();
        }
    }
}
