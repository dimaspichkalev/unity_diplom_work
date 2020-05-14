using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValue
{
    public static int xSize = 100;
    public static int zSize = 100;

    public static string portType = "Речной";

    public static Color wallColor { get; set; } = Random.ColorHSV(0f, .25f, 0.4f, 1f);

    public static void RefreshWallColor()
    {
        wallColor = Random.ColorHSV(0f, .25f, 0.4f, 1f);
    }

}
