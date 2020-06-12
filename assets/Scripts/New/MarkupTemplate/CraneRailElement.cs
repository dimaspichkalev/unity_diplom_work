using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class CraneRailElement : MarkupElement
{
    public const int defaultWidth = 15;

    public bool deadEndLeft;
    public bool deadEndRight;
    
    [Range (0, 2)]
    public int howManyRails;
}