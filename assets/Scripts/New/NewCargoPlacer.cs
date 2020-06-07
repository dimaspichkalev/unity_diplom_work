using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCargoPlacer : MonoBehaviour
{
    public List<GameObject> containersList;
    public List<GameObject> tubesList;

    public int height;
    public int breakHeight;
    void Start()
    {
        Vector3[] zone = new Vector3[]
        {
            new Vector3(100, 0, 0),
            new Vector3(200, 0, 0),
            new Vector3(200, 0, 200),
            new Vector3(100, 0, 200),
        };
        //PlaceCargoInZone(zone);   
        PlacePyramidTubes(tubesList[Random.Range(0, tubesList.Count - 1)], height, breakHeight);
        //PlaceContainers(containersList[3], height, breakHeight);
    }

    void PlaceCargoInZone(Vector3[] points)
    {
       
    }

    void PlacePyramidTubes(GameObject tubePrefab, int levels, int breakLevel)
    {
        Renderer prefabRenderer = (tubePrefab.GetComponent<Renderer>() != null) ? tubePrefab.GetComponent<Renderer>() : tubePrefab.GetComponentInChildren<Renderer>();
        Vector3 cargo_space = prefabRenderer.bounds.size;
        for (int i = 0; i < levels; i++)
        {
            var nextPosition = cargo_space.z / 2f * i;
            for (int k = 0; k < (levels - i); k++)
            {
                Instantiate(tubePrefab, new Vector3(nextPosition, i * cargo_space.y / 2f, 0), Quaternion.identity);
                nextPosition += cargo_space.z;
            }
            if (i == breakLevel - 1)
                break;
        }
    }

    void PlaceContainers(GameObject prefab, int levels, int breakLevel)
    {
        Renderer prefabRenderer = (prefab.GetComponent<Renderer>() != null) ? prefab.GetComponent<Renderer>() : prefab.GetComponentInChildren<Renderer>();
        Vector3 cargo_space = prefabRenderer.bounds.size;
        Material material;
        Debug.Log(cargo_space.ToString());
        for (int y = 0; y < levels; y++)
        {
            for (int x = 0; x < levels; x++)
            {
                for (int z = 0; z < levels; z++)
                {
                    Instantiate(prefab, new Vector3(cargo_space.x * x, y * cargo_space.y, z * cargo_space.z), Quaternion.identity);
                    ColorStorage.RefreshWallColor();
                    material = RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
                    prefabRenderer.sharedMaterial = material;
                }

            }
            if (y == breakLevel - 1)
                break;
        }
    }
}

public class CargoList
{
    CargoType cargoType;
    //List<GameObject> cargoList;
}

public enum CargoType { Tube, Container };
