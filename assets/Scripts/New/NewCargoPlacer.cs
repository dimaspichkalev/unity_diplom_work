using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCargoPlacer : MonoBehaviour
{
    public List<GameObject> containersList;
    public List<GameObject> tubesList;

    public int height;
    public int breakHeight;
    public int tileX = 30;
    public int tileZ = 35;

    public NewRoadGenerator roadGen;

    float portHeight = 2;

    public void PlaceCargo()
    {
        portHeight = GameObject.Find("Generator").GetComponent<NewPlaneGenerator>().height;
        foreach (Vector3[] zone in roadGen.freeZonesList)
        {
            PlaceCargoInZone(zone);
        }
    }

    //void Start()
    //{
    //    Vector3[] zone = new Vector3[]
    //    {
    //        new Vector3(0, 0, 0),
    //        new Vector3(100, 0, 0),
    //        new Vector3(100, 0, 100),
    //        new Vector3(0, 0, 100),
    //    };
    //    PlaceCargoInZone(zone);
    //}

    void PlaceCargoInZone(Vector3[] zonePoints)
    {
        Vector3 minVector = Vector3.positiveInfinity;
        Vector3 maxVector = Vector3.zero;

        for (int i = 0; i < zonePoints.Length; i++)
        {
            minVector = (zonePoints[i].magnitude < minVector.magnitude) ? zonePoints[i] : minVector;
            maxVector = (zonePoints[i].magnitude > maxVector.magnitude) ? zonePoints[i] : maxVector;
        }

        
        for (float x = minVector.x; x < maxVector.x - tileX; x += tileX)
        {
            bool containers = Random.value > 0.5f;
            if (containers)
                PlaceContainers(containersList[Random.Range(0, containersList.Count - 1)], new Vector3(x, 0, minVector.z), new Vector3(x + tileX, 0, minVector.z));
            else
                PlacePyramidTubes(tubesList[Random.Range(0, tubesList.Count - 1)], new Vector3(x, 0, minVector.z), new Vector3(x + tileX, 0, minVector.z));
        }
        



    }

    void PlacePyramidTubes(GameObject tubePrefab, Vector3 minVectorZone, Vector3 maxVectorZone)
    {
        Renderer prefabRenderer = (tubePrefab.GetComponent<Renderer>() != null) ? tubePrefab.GetComponent<Renderer>() : tubePrefab.GetComponentInChildren<Renderer>();
        Vector3 cargo_space = prefabRenderer.bounds.size;
        Vector3 offset = new Vector3(2.5f, portHeight + cargo_space.y / 3f, 2.5f);
        int levels = (int)(tileX / cargo_space.y);
        int breakLevel = Random.Range(1, levels) + 1000;
        GameObject holder = new GameObject("Cargo Holder");
        for (int i = 0; i < levels; i++)
        {
            var nextPosition = cargo_space.y / 2f * i;
            for (int k = 0; k < (levels - i); k++)
            {
                Instantiate(tubePrefab, new Vector3(nextPosition, i * cargo_space.y / 2f, cargo_space.x / 2f) + minVectorZone + offset, Quaternion.identity, holder.transform);
                nextPosition += cargo_space.y;
            }
            if (i == breakLevel - 1)
                break;
        }
    }

    void PlaceContainers(GameObject prefab, Vector3 minVectorZone, Vector3 maxVectorZone)
    {
        Vector3 offset = new Vector3(2.5f, portHeight, 2.5f);
        Renderer prefabRenderer = (prefab.GetComponent<Renderer>() != null) ? prefab.GetComponent<Renderer>() : prefab.GetComponentInChildren<Renderer>();
        Vector3 cargo_space = prefabRenderer.bounds.size;
        Debug.Log(cargo_space.ToString());
        GameObject holder = new GameObject("Cargo Container Holder");
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < (int)(tileX / (cargo_space.x + offset.x)); x++)
            {
                for (int z = 0; z < (int)(tileZ / (cargo_space.z + offset.z)); z++)
                {
                    Instantiate(prefab, new Vector3(cargo_space.x * (x + 1 / 2f), cargo_space.y * (y + 1 / 2f), cargo_space.z * (z + 1 / 2f)) + minVectorZone + offset, Quaternion.identity, holder.transform);
                }
            }
        }
    }
}

public class CargoList
{
    CargoType cargoType;
    //List<GameObject> cargoList;
}

public enum CargoType { Tube, Container };
