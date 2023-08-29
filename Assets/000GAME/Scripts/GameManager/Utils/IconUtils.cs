using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IconUtils{

    static public Texture2D AssetPreviewTransparent(Texture2D diffuse, int resize)
    {
        int realSize;
        if (resize > diffuse.width)
            realSize = resize;
        else
            realSize = Mathf.Max(diffuse.width, diffuse.height);
        while (realSize % 4 != 0)
            realSize++;

        Texture2D result = new Texture2D(realSize, realSize, TextureFormat.ARGB32, true);

        int padxStart = (realSize - diffuse.width) / 2;
        int padyStart = (realSize - diffuse.height) / 2;
        int padxEnd = padxStart + diffuse.width;
        int padyEnd = padyStart + diffuse.height;
        Color refer = diffuse.GetPixel(0, 0);

        int c2 = realSize / 4;
        float rgb = 0f;

        for (int x = 0; x < realSize; ++x)
        {
            for (int y = 0; y < realSize; ++y)
            {
                int odd = 0;
                odd += x / c2;
                odd += y / c2;
                if (odd % 2 == 1)
                    rgb = 200f / 255f;
                //rgb=71f/255f;
                else
                    rgb = 1f;
                //rgb=102f/255f;

                if (x <= padxStart || y <= padyStart || x > padxEnd || y > padyEnd)
                    result.SetPixel(x, y, new Color(rgb, rgb, rgb, 0));
                else
                {
                    Color diffuseColor = diffuse.GetPixel(x - padxStart, y - padyStart);
                    if (diffuseColor == refer)
                        result.SetPixel(x, y, new Color(rgb, rgb, rgb, 0));
                    else
                        result.SetPixel(x, y, diffuseColor);
                }
            }
        }
        //result.Resize (128, 128);
        result.Apply();
        return result;
    }

    static Texture2D CreateUsingMaskAlpha(Texture2D diffuse, Texture2D mask)
    {
        Texture2D result = new Texture2D(diffuse.width, diffuse.height, TextureFormat.ARGB32, false);
        Color[] diffuseColors = diffuse.GetPixels();
        Color[] maskColors = mask.GetPixels();

        for (int i = 0; i < diffuseColors.Length; i++)
        {
            diffuseColors[i].a = maskColors[i].a;
        }

        result.SetPixels(diffuseColors);
        result.Apply();

        return result;
    }
}
