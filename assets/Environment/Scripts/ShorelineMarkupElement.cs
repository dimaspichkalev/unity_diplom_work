using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShorelineMarkupElement : MarkupElement
{
    public int length;
    public bool isEmpty;
    public int numItemsToSpawn = 10;

    public List<GameObject> objectsToPlacement;
}
