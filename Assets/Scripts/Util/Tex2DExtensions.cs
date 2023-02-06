using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.Util
{
    public static class Tex2DExtension
    {
        public static Texture2D DrawCircle(this Texture2D tex, Color color, int x, int y, int radius = 3)
        {
            float rSquared = radius * radius;

            for (int u = x - radius; u < x + radius + 1; u++)
                for (int v = y - radius; v < y + radius + 1; v++)
                    if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                        tex.SetPixel(u, v, color);

            return tex;
        }

        public static Texture2D FillTexture(this Texture2D tex, Color color)
        {
            var fillColorArray = tex.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = color;
            }

            tex.SetPixels(fillColorArray);

            return tex;
        }
    }
}