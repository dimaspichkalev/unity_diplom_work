using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    public GameObject itemToSpread;
    public int numItemsToSpawn = 10;

    public float xSpread = 10;
    public float ySpread = 0;
    public float zSpread = 10;

    void Start()
    {
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            SpreadItem();
        }
        
    }

    void SpreadItem()
    {
        Vector3 randPosition = new Vector3(Random.Range(-xSpread, xSpread), Random.Range(-ySpread, ySpread), Random.Range(-zSpread, zSpread));
        GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
    }

}
