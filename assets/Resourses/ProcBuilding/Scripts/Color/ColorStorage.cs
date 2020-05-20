using UnityEngine;
using System.Collections;

public class ColorStorage
{
    public static Color wallColor { get; set; } = Random.ColorHSV(0f, .25f, 0.4f, 1f);

    public static void RefreshWallColor()
    {
        wallColor = Random.ColorHSV(0f, .25f, 0.4f, 1f);
    }

}
