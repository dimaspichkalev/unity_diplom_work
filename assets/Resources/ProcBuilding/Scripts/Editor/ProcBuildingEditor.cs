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
            proGen.Clear();
            ColorStorage.RefreshWallColor();
            var holder = new GameObject("PartHouse Holder" + proGen.rows.ToString() + " " + proGen.columns.ToString() + " " + proGen.floors.ToString());
            proGen.Generate(proGen.rows, proGen.columns, proGen.floors, holder);
        }

        if (GUILayout.Button("Generate 5 random houses"))
        {
            proGen.Clear();
            for (int i = 0; i < 5; i++)
            {
                proGen.Generate(Random.Range(5, 50), Random.Range(5, 50), Random.Range(5, 15), proGen.transform.gameObject);
            }
        }

        if (GUILayout.Button("Place random houses"))
        {
            proGen.Clear();
            Vector3[] zonePoints = new Vector3[]
            {
                new Vector3(100,0,20),
                new Vector3(200,0,20),
                new Vector3(200,0,40),
                new Vector3(100,0,40)
            };
            proGen.PlaceObjectsInZone(zonePoints);
        }


    }
}
