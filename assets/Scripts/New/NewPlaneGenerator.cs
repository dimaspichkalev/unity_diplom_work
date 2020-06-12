using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewPlaneGenerator : MonoBehaviour
{
    public int xSize;
    public int zSize;
    public float height;

    [HideInInspector]
    public int zLeftPartSize;
    [HideInInspector]
    public int zRightPartSize;

    public enum PortType
    {
        Любой, Речной, Морской
    }
    public PortType portType;
    public bool active = false;

    public Material ground;
    public Material underground;

    public MarkupTemplate riverMarkupTemplate;
    public MarkupTemplate seaLeftPartMarkupTemplate;
    public MarkupTemplate seaRightPartMarkupTemplate;
    public GameObject waterPrefab;

    NewRoadGenerator newRoadGenerator;
    public const int seaRectangleXsize = 50;
    public const int seaLadleZsize = 100; // Размер ковша по Z


    void Start()
    {
        CreateShape();
        active = true;
        newRoadGenerator = gameObject.GetComponent<NewRoadGenerator>();
        Debug.Log("Border porttype = " + portType.ToString());
        if (portType == PortType.Речной)
            newRoadGenerator.GenerateFromTemplate(riverMarkupTemplate);
        else if (portType == PortType.Морской)
        {
            newRoadGenerator.GenerateFromSeaTemplate(seaLeftPartMarkupTemplate, 0, zLeftPartSize / 2f);
            newRoadGenerator.GenerateFromSeaTemplate(seaRightPartMarkupTemplate, zLeftPartSize + seaLadleZsize, zLeftPartSize + seaLadleZsize + zRightPartSize / 2f);
        }

        gameObject.GetComponent<NewBorderGenerator>().GenerateBorder(portType);
        gameObject.GetComponent<NewBorderGenerator>().GenerateTires(portType);
        SpawnObjects();
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
            zSize = GetZSizeofPort(riverMarkupTemplate);
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
            zLeftPartSize = GetZSizeofPort(seaLeftPartMarkupTemplate);
            zRightPartSize = GetZSizeofPort(seaRightPartMarkupTemplate);
            Vector3[] points = new Vector3[]
            {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, zLeftPartSize),
                    new Vector3(xSize, 0, zLeftPartSize),
                    new Vector3(xSize, 0, zLeftPartSize + seaLadleZsize),
                    new Vector3(0, 0, zLeftPartSize + seaLadleZsize),
                    new Vector3(0, 0, zLeftPartSize + seaLadleZsize + zRightPartSize),
                    new Vector3(xSize + seaRectangleXsize, 0, zLeftPartSize + seaLadleZsize + zRightPartSize),
                    new Vector3(xSize + seaRectangleXsize, 0, 0)
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

    void SpawnObjects()
    {
        foreach (Vector3[] zone in newRoadGenerator.freeZonesList)
        {
            bool houses = Random.value > 0.5f;
            if (houses)
                GameObject.Find("HouseGenerator").GetComponent<NewHousePlacer>().PlaceHouses(zone);
            else
                GameObject.Find("CargoGenerator").GetComponent<NewCargoPlacer>().PlaceCargo(zone, height);
        }
        
    }

    int GetZSizeofPort(MarkupTemplate markupTemplate)
    {
        int zsize = 10;
        foreach (MarkupElement element in markupTemplate.roadsList)
        {
            if (element is AutoMarkupElement)
                zsize += AutoMarkupElement.defaultWidth;
            else if (element is CraneRailElement)
                zsize += CraneRailElement.defaultWidth;
            else if (element is FreeSpaceMarkupElement)
            {
                FreeSpaceMarkupElement elem = (FreeSpaceMarkupElement)element;
                zsize += elem.width;
            }
        }
        return zsize;
    }

}
