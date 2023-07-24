using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EditorSaveSystem : EditorWindow, ISaveable
{
    private string editorPath = string.Empty;
    private string defaultPath = string.Empty;


    private void OnEnable()
    {
        editorPath = Path.Combine(Application.dataPath, "Scripts", "Editor");
        defaultPath = Path.Combine(editorPath, "Editor presets");

        if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);
    }

    //TODO: Only loading something if a save data is present
    public TerrainSaveData Load<TerrainSaveData>()
    {
        var fileCheck = Directory.GetFiles(defaultPath, "*.json");
        TerrainSaveData saveData = default;

        if (fileCheck == null || fileCheck.Length <= 0)
        {
            Debug.Log("No suitable file present");
            return saveData;
        }


        string loadPath = EditorUtility.OpenFilePanel("Load preset", defaultPath, "json");

        if (string.IsNullOrEmpty(loadPath)) return saveData;

        string loadData = File.ReadAllText(loadPath);

        saveData = (TerrainSaveData)JsonUtility.FromJson(loadData, typeof(TerrainSaveData));

        return saveData;
    }

    public void Save<TerrainSaveData>(TerrainSaveData data)
    {
        editorPath = Path.Combine(Application.dataPath, "Scripts", "Editor");
        defaultPath = Path.Combine(editorPath, "Editor presets");

        if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);

        string path = EditorUtility.SaveFilePanel("Save Settings", defaultPath, "preset", "json");

        if (string.IsNullOrEmpty(path)) return;

        string saveData = JsonUtility.ToJson(data, true);
        
        File.WriteAllText(path, saveData);

        Debug.Log("Save successful");
    }
}