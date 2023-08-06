using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveTerrainheights : MonoBehaviour
{
    [SerializeField] private Terrain terrain = null;
    [SerializeField] private Texture2D texture = null;
    [SerializeField] private string fileName = null;

    [ContextMenu("Extract heights")]
    public void ExtractHeights()
    {
        var size = terrain.terrainData.heightmapResolution;
        Debug.Log(size);
        var heights = terrain.terrainData.GetHeights(0, 0, size, size);

        var min = 1f;
        var max = 0f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var height = heights[x, y];
                if (height < min) min = height;
                else if (height > max) max = height;

                texture.SetPixel(x, y, new Color(height, height, height, 1));
                //texture.SetPixel(x, y, Color.red);
            }
        }

        Debug.Log($"min: {min}; max: {max}");

        texture.Apply();


        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/Sprites/Heightmaps/";
        Debug.Log(dirPath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + fileName + ".png", bytes);
    }
}