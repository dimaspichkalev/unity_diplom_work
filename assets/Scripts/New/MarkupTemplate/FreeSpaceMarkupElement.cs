using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class FreeSpaceMarkupElement : MarkupElement
{
    public int width;

    public int Width
    {
        get => width;
        set
        {
            if (value > 0)
                width = value;
            else
                width = 20;
        }
    }
}
