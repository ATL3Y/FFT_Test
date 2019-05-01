using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsHelper
{
    // Converting pure hue to RGB
    public static Vector3 HUEtoRGB ( float H )
    {
        float R = Mathf.Abs(H * 6 - 3) - 1;
        float G = 2 - Mathf.Abs(H * 6 - 2);
        float B = 2 - Mathf.Abs(H * 6 - 4);
        return new Vector3 ( Mathf.Clamp01 ( R ), Mathf.Clamp01 ( G ), Mathf.Clamp01 ( B ) );
    }

    //Converting HSV to RGB
    public static Vector3 HSVtoRGB ( Vector3 HSV )
    {
        Vector3 RGB = HUEtoRGB(HSV.x);
        Vector3 temp = new Vector3( RGB.x - 1.0f, RGB.y - 1.0f, RGB.z - 1.0f ) * HSV.y;
        temp = new Vector3 ( temp.x + 1.0f, temp.y + 1.0f, temp.z + 1.0f );
        return temp * HSV.z;
    }
}
