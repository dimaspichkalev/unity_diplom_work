using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHousePlacer : MonoBehaviour
{
    public ProcBuilding proc;

    public void PlaceHouses(Vector3[] zone)
    {
        proc.PlaceObjectsInZone(zone);
    }
}
