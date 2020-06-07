using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHousePlacer : MonoBehaviour
{
    public NewRoadGenerator roadGen;
    public ProcBuilding proc;

    public void PlaceHouses()
    {
        foreach (Vector3[] zone in roadGen.freeZonesList)
        {
            proc.PlaceObjectsInZone(zone);
        }
    }
}
