using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProGen))]
public class ProGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProGen proGen = (ProGen)target;

        if (GUILayout.Button("Generate"))
        {
            StaticValue.RefreshWallColor();
            proGen.Generate();
        }

        if (GUI.changed)
        {
            proGen.Generate();
        }
    }
}
