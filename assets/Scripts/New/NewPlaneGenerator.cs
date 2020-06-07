using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewPlaneGenerator : MonoBehaviour
{
    public int xSize;
    public int zSize;
    public float height;
    public enum PortType
    {
        Любой, Речной, Морской
    }
    public PortType portType;
    public bool active = false;

    public Material ground;
    public Material underground;

    public GameObject waterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateShape();
        active = true;
        gameObject.GetComponent<NewRoadGenerator>().GenerateRoads();
        gameObject.GetComponent<NewBorderGenerator>().GenerateBorder();
        gameObject.GetComponent<NewBorderGenerator>().GenerateTires();
        //GameObject.Find("HouseGenerator").GetComponent<NewHousePlacer>().PlaceHouses();
        GameObject.Find("CargoGenerator").GetComponent<NewCargoPlacer>().PlaceCargo();
        SpawnWater();
    }


    void CreateShape()
    {
        if (portType == PortType.Любой)
        {
            portType = (PortType)Random.Range(1, 2);
        }

        if (portType == PortType.Речной)
        {
            Vector3[] points = new Vector3[]
            {
                    new Vector3(0, 0, 0),
                    new Vector3(xSize, 0, 0),
                    new Vector3(xSize, 0, zSize),
                    new Vector3(0, 0, zSize),
            };
            ProBuilder.Examples.CreatePolyShape.CreatePortPlane(points, ground, underground, height);
        }
        else if (portType == PortType.Морской)
        {
            Vector3[] points = new Vector3[]
            {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 75),
                    new Vector3(150, 0, 75),
                    new Vector3(150, 0, 175),
                    new Vector3(0, 0, 175),
                    new Vector3(0, 0, 250),
                    new Vector3(200, 0, 250),
                    new Vector3(200, 0, 0)
            };
            ProBuilder.Examples.CreatePolyShape.CreatePortPlane(points, ground, underground, height);

        }
    }

    void SpawnWater()
    {
        GameObject waterHolder = new GameObject("Water");
        waterHolder.transform.position = new Vector3(xSize / 2f, 0, zSize / 2f);
        waterHolder.transform.localScale = new Vector3(30, 0, 30);
        Instantiate(waterPrefab, waterHolder.transform);
    }

}
