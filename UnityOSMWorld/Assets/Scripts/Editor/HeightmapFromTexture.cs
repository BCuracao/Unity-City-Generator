using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Heightmap maker
/// </summary>
public static class HeightmapFromTexture
{
    [MenuItem("Terrain/Heightmap From Texture")]
    static void ApplyHeightmap()
    {
        //string heightmapPath = EditorUtility.OpenFilePanel("Texture", GetFolderPath(SpecialFolder.Desktop), ".png");
        Texture2D heightmap = Selection.activeObject as Texture2D;
        if (heightmap == null)
        {
            EditorUtility.DisplayDialog("No texture selected", "Please select a texture.", "Cancel");
            return;
        }
        var terrain = Terrain.activeTerrain.terrainData;
        int w = heightmap.width;
        int h = heightmap.height;
        int w2 = terrain.heightmapWidth;
        float[,] heightmapData = terrain.GetHeights(0, 0, w2, w2);
        Color[] mapColors = heightmap.GetPixels();
        Color[] map = new Color[w2 * w2];
        if (w2 != w || h != w)
        {
            // Resize using nearest-neighbor scaling if texture has no filtering
            if (heightmap.filterMode == FilterMode.Point)
            {
                float dx = (float)w / (float)w2;
                float dy = (float)h / (float)w2;
                for (int y = 0; y < w2; y++)
                {
                    if (y % 20 == 0)
                    {
                        EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
                    }
                    int thisY = Mathf.FloorToInt(dy * y) * w;
                    int yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        map[yw + x] = mapColors[Mathf.FloorToInt(thisY + dx * x)];
                    }
                }
            }
            // Otherwise resize using bilinear filtering
            else
            {
                float ratioX = (1.0f / ((float)w2 / (w - 1)));
                float ratioY = (1.0f / ((float)w2 / (h - 1)));
                for (int y = 0; y < w2; y++)
                {
                    if (y % 20 == 0)
                    {
                        EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
                    }
                    int yy = Mathf.FloorToInt(y * ratioY);
                    int y1 = yy * w;
                    int y2 = (yy + 1) * w;
                    int yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        int xx = Mathf.FloorToInt(x * ratioX);
                        Color bl = mapColors[y1 + xx];
                        Color br = mapColors[y1 + xx + 1];
                        Color tl = mapColors[y2 + xx];
                        Color tr = mapColors[y2 + xx + 1];
                        float xLerp = x * ratioX - xx;
                        map[yw + x] = Color.Lerp(Color.Lerp(bl, br, xLerp), Color.Lerp(tl, tr, xLerp), y * ratioY - (float)yy);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
        else
        {
            // Use original if no resize is needed
            map = mapColors;
        }
        // Assign texture data to heightmap
        for (int y = 0; y < w2; y++)
        {
            for (int x = 0; x < w2; x++)
            {
                heightmapData[y, x] = map[y * w2 + x].grayscale;
            }
        }
        terrain.SetHeights(0, 0, heightmapData);
    }
}