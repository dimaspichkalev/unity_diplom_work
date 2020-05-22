using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ProcBuilding : MonoBehaviour
{
    public int rows = 3;
    public int columns = 3;
    public int floors = 3;

    Material material;
    ProBuilderMesh body;
    ProBuilderMesh roof;
    ProBuilderMesh door;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Clear();
        ColorStorage.RefreshWallColor();
        material = RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);

        body = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(rows, floors, columns));
        body.GetComponent<MeshRenderer>().sharedMaterial = material;
        body.transform.name = "House BODY";
        body.transform.parent = transform;
        body.transform.position = transform.position;

        roof = ShapeGenerator.GeneratePrism(PivotLocation.Center, new Vector3(rows, Mathf.CeilToInt(columns / 5.0f), columns));
        roof.GetComponent<MeshRenderer>().sharedMaterial = material;// RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
        roof.transform.position = body.transform.position + new Vector3(0, 0.5f * (floors + Mathf.CeilToInt(columns / 5.0f)) , 0);
        roof.transform.name = "House ROOF";
        roof.transform.parent = transform;

        //int doorwidth = (int)(0.4f * columns);
        //int door_delta = doorwidth / 2;
        //door = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(doorwidth, 3, 0.14f));
        //door.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load("Materials/Brown", typeof(Material)) as Material;
        //door.transform.name = "House DOOR";
        //door.transform.parent = transform;
        //door.transform.position = body.transform.position;
        ////column <= columns / 2 + door_delta && column >= columns / 2 - door_delta
    }

    // Update is called once per frame
    void Clear()
    {
        if (body != null)
        {
            DestroyImmediate(body.transform.gameObject);
        }
        if (roof != null)
        {
            DestroyImmediate(roof.transform.gameObject);
        }
        if (door != null)
        {
            DestroyImmediate(door.transform.gameObject);
        }
    }
}
