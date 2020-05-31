using UnityEngine;
using System.Collections;

public class BigBorder : MonoBehaviour
{
    public int countBlocks = 14;
    public GameObject borderPrefab;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < countBlocks*2; i+=2)
        {
            Instantiate(borderPrefab, new Vector3(i, 0.2f, 0), Quaternion.identity, transform);
        }
    }

}
