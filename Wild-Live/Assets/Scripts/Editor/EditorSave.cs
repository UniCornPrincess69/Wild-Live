using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EditorSave : ISaveable
{
    private string editorPath = string.Empty;
    private string defaultPath = string.Empty;


    //TODO: Only loading something if a save data is present
    public TerrainData Load<TerrainData>()
    {
        TerrainData data = default;
        
        if (!File.Exists(defaultPath))
        {
            Debug.Log("No file present");
        }

        return data;
    }

    public void Save<TerrainData>(TerrainData terrainData)
    {
        editorPath = Path.Combine(Application.dataPath, "Scripts", "Editor");
        defaultPath = Path.Combine(editorPath, "Editor presets");

        if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);

        string path = EditorUtility.SaveFilePanel("Save Settings", defaultPath, "preset", "json");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        string data = JsonUtility.ToJson(terrainData, true);

        File.WriteAllText(path, data);

        Debug.Log(path);
        Debug.Log("Save successful");
    }
}
