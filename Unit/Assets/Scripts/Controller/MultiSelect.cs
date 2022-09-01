using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiSelect
{
    private static Texture2D _whiteTexture;

    public static Texture2D whiteTexture
    {
        get
        {
            if(_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }
            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, whiteTexture);
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        //top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        //botom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        //leftside
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        //rightside
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    public static Rect GetScreenRect(Vector3 screenPos_1, Vector3 screenPos_2)
    {
        //go from the bottom right to the top left
        screenPos_1.y = Screen.height - screenPos_1.y;
        screenPos_2.y = Screen.height - screenPos_2.y;

        //corners
        Vector3 botomRight = Vector3.Max(screenPos_1, screenPos_2);
        Vector3 topLeft = Vector3.Min(screenPos_1, screenPos_2);

        //create the rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, botomRight.x, botomRight.y);

    }

    public static Bounds GetViewPointBounds(Camera camera, Vector3 screenPosition_1, Vector3 screenPosition_2)
    {
        Vector3 pos_1 = camera.ScreenToViewportPoint(screenPosition_1);
        Vector3 pos_2 = camera.ScreenToViewportPoint(screenPosition_2);
         
        Vector3 min = Vector3.Min(pos_1, pos_2);
        Vector3 max = Vector3.Max(pos_1, pos_2);

        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }
}
