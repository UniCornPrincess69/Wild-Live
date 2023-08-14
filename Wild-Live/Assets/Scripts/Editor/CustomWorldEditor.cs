using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using UnityEditor.UIElements;

public class CustomWorldEditor : EditorWindow
{
    #region Variables
    private static readonly Vector2 _minSize = new Vector2(500f, 500f);
    private static readonly Vector2 _maxSize = new Vector2(800f, 800f);
    private static Rect _windowDefault = new(100, 50, 500, 500);

    private readonly string[] _toolbar = { "Terrain Settings", "Noise Settings", "About" };
    private readonly string[] _settings = { "Map Size", "Resolution", "Spawn Position", "Include noise", "Material" };
    private readonly string[] _noiseSettings = { "Noise Intensity", "Height Map", "Use Heightmap?", "Noise Seed" };
    private int _toolbarIndex = 0;
    private float _yPosFactor = 0.05f;
    private float _fieldOffset = 0f;
    private bool _useCheck = false;
    private TerrainData _terrainData;
    private GameObject _generator = null;
    private GameObject _selected = null;
    private int _selectionCount = 0;
    private EditorSaveSystem _saveSystem = null;


    #endregion


    #region Rect-Variables
    private float _fieldWidth = 0f;
    private float _fieldHeight = 0f;
    private float _sliderWidth = 0f;
    private float _labelWidth = 0f;
    private Rect _slider = default;
    private Rect _field = default;
    private Rect _label = default;
    #endregion

    //TODO: Dropdown Menu for using heightmap values on top of noise or only heightmap

    //Loading of the default material and the scriptable object for Terrain data storage
    private void OnEnable()
    {
        _terrainData = (TerrainData)EditorGUIUtility.Load("Assets/Resources/DefaultTerrain.asset");
        _terrainData.Material = (Material)EditorGUIUtility.Load("Assets/Resources/DefaultTerrainMat.mat");
        _saveSystem = new EditorSaveSystem();
        _generator = FindObjectOfType<TerrainGenerator>()?.gameObject;
        var terrains = FindObjectsOfType<TerrainGenerator>();
        if (terrains.Length > 1)
        {
            _selected = terrains[0].gameObject;
            for (int i = 1; i < terrains.Length; i++)
            {
                DestroyImmediate(terrains[i].gameObject);
            }
        }

        if (_generator != null)
        {
            if (EditorUtility.DisplayDialog("Keep Terrain?", "Do you wish to keep the previously generated Terrain?", "Yes", "No"))
            {
                _useCheck = true;
            }
            else
            {
                _terrainData.TerrainPosition = Vector3.zero;
                _terrainData.MapSize = 1000;
                _terrainData.Resolution = 200;
                DestroyImmediate(_generator);
            }
        }
        else return;

    }

    [MenuItem("Tools/World Editor")]
    public static void DrawWindow()
    {
        var window = GetWindowWithRect(typeof(CustomWorldEditor), _windowDefault);
        window.minSize = _minSize;
        window.maxSize = _maxSize;
        window.titleContent = new GUIContent("World Editor");
    }


    private void OnGUI()
    {
        var windowRect = position;
        _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbar);
        if (_toolbarIndex == 0)
        {
            DrawTerrainSettings(windowRect);
        }
        else if (_toolbarIndex == 1)
        {
            if (!_terrainData.IncludeNoise)
            {
                var noiseMessage = new Rect(windowRect.width * .35f, windowRect.height * .35f,
                    windowRect.width, windowRect.height * .35f);
                EditorGUI.LabelField(noiseMessage, new GUIContent("Please enable the noise toggle!"));
            }
            else DrawNoiseSettings(windowRect);
        }
        else
        {
            EditorGUILayout.LabelField(new GUIContent("This tool was created for experimentation of the terrain generation"));
            EditorGUILayout.LabelField(new GUIContent("Created by Tommy Hartung"));
        }
        //GUILayout.Label("Terrain Settings", EditorStyles.boldLabel);

    }

    /// <summary>
    /// Draw the tab with the noise settings
    /// </summary>
    /// <param name="windowRect">Rect of the complete window</param>
    private void DrawNoiseSettings(Rect windowRect)
    {
        EditorGUI.BeginChangeCheck();
        //.3f so the label only takes 30% of the window
        _labelWidth = windowRect.width * 0.3f;
        //.1f so the field only takes 10% of the window
        _fieldWidth = windowRect.width * 0.1f;
        //.6f so the field only takes 60% of the window
        _sliderWidth = windowRect.width * 0.60f;
        _fieldHeight = EditorGUIUtility.singleLineHeight + 0.25f;
        windowRect = GetFieldRects(windowRect);

        _terrainData.Seed = StartField<string>(_noiseSettings[3], _terrainData.Seed, ref windowRect);

        _terrainData.NoiseIntensity = (int)StartField<float>(_noiseSettings[0], _terrainData.NoiseIntensity,
            ref windowRect, 1f, 2000f);

        if (_terrainData.HeightmapUsed = StartField<bool>(_noiseSettings[2], _terrainData.HeightmapUsed, ref windowRect))
        {
            GUI.enabled = true;
        }
        else GUI.enabled = false;

        _terrainData.Heightmap = StartField<Texture2D>(_noiseSettings[1], _terrainData.Heightmap, ref windowRect);
        GUI.enabled = true;

        

        if (_useCheck)
        {
            _selected.GetOrAddComponent<TerrainGenerator>().EditorWorldGeneration(_terrainData, _terrainData.Material);
        }
        _fieldOffset = 0f;
    }

    /// <summary>
    /// Drawing of the TerrainSettings window
    /// </summary>
    /// <param name="windowRect">Rect of the complete window</param>
    private void DrawTerrainSettings(Rect windowRect)
    {
        EditorGUI.BeginChangeCheck();

        //.2f so the label only takes 20% of the window
        _labelWidth = windowRect.width * 0.20f;
        //.2f so the field only takes 12% of the window
        _fieldWidth = windowRect.width * 0.12f;
        //.6f so the field only takes 60% of the window
        _sliderWidth = windowRect.width * 0.6f;
        _fieldHeight = EditorGUIUtility.singleLineHeight;
        windowRect = GetFieldRects(windowRect);

        _terrainData.MapSize = (int)StartField<float>(_settings[0], _terrainData.MapSize, ref windowRect, 100f, 5000f);
        _terrainData.Resolution = (int)StartField<float>(_settings[1], _terrainData.Resolution, ref windowRect, 100f, 255f);
        _terrainData.TerrainPosition = StartField<Vector3>(_settings[2], _terrainData.TerrainPosition, ref windowRect);
        _terrainData.Material = StartField<Material>(_settings[4], _terrainData.Material, ref windowRect);
        _terrainData.IncludeNoise = StartField<bool>(_settings[3], _terrainData.IncludeNoise, ref windowRect);
        _fieldOffset = 0f;

        
        if (GUI.Button(new Rect(windowRect.width * 0.05f, windowRect.height * 0.95f, windowRect.width * 0.45f, _fieldHeight), "Save"))
        {
            var saveData = new TerrainSaveData(_terrainData);
            _saveSystem.Save(saveData);
        }
        if (GUI.Button(new Rect(windowRect.width * 0.50f, windowRect.height * 0.95f, windowRect.width * 0.45f, _fieldHeight), "Load"))
        {
            //_terrainData = _saver.Load(_terrainData);
            //_saver.Load<TerrainSaveData>(ref _terrainData);

            _terrainData.SetData(_saveSystem.Load<TerrainSaveData>());
        }

        if (GUI.Button(new Rect(windowRect.width * 0.05f, windowRect.height * 0.90f, windowRect.width * 0.9f, _fieldHeight), "Generate Terrain") || _useCheck)
        {
            _useCheck = true;
            if (_selectionCount == 1 && _selected.TryGetComponent(out TerrainGenerator gen))
            {
                //_useCheck = true;
                gen.EditorWorldGeneration(_terrainData, _terrainData.Material);
            }
            else
            {
                if (_generator != null) DestroyImmediate(_generator);
                if (_selected == null) _selected = new GameObject("Editor Terrain Generator");
                _selected.transform.position = _terrainData.TerrainPosition;
                _selected.GetOrAddComponent<TerrainGenerator>().EditorWorldGeneration(_terrainData, _terrainData.Material);
            }
        }


        //field offset needs to be reset, otherwise the field would move constantly downwards
        _fieldOffset = 0f;
    }

    /// <summary>
    /// Formular for the calculation of the Rects was taken from ChatGPT. I changed about 90% of it to fit my needs
    /// </summary>
    /// <param name="windowRect"></param>
    /// <returns></returns>
    private Rect GetFieldRects(Rect windowRect)
    {
        var labelPosX = windowRect.width - _labelWidth;
        var fieldPosX = windowRect.width + _label.width + _fieldWidth;
        var sliderPosX = windowRect.width + _label.width + _field.width;
        var windowYPos = windowRect.height * _yPosFactor;

        //Magic numbers are all tested out factors so the coords fit into the window
        _label = new Rect(labelPosX * 0.01f, windowYPos + _fieldOffset, _labelWidth, _fieldHeight);
        _field = new Rect(fieldPosX * 0.15f, windowYPos + _fieldOffset, _fieldWidth, _fieldHeight);
        _slider = new Rect(sliderPosX * 0.25f, windowYPos + _fieldOffset, _sliderWidth, _fieldHeight);
        return windowRect;
    }

    /// <summary>
    /// Generic method to field needs to be instanciated. In relation to the datatype.
    /// Explained and shown by Stephan
    /// </summary>
    /// <typeparam name="T">Method kept generic to work  with different datatypes</typeparam>
    /// <param name="label">Name of the setting</param>
    /// <param name="value">Setting-Value, from Scriptable Object</param>
    /// <param name="windowRect">Rect to set the correct positions</param>
    /// <param name="parameters">Optional params, specifically for Sliders</param>
    /// <returns></returns>
    private T StartField<T>(string label, object value, ref Rect windowRect, params object[] parameters)
    {
        float lineCount = 1f;

        GUILayout.BeginHorizontal();

        if (value.GetType() == typeof(int))
        {
            GUI.Label(_label, label);
            value = EditorGUI.FloatField(_field, (int)value);
            if (parameters.Length == 2) value = GUI.HorizontalSlider(_slider, (float)value, (float)parameters[0], (float)parameters[1]);
        }
        else if (value.GetType() == typeof(Vector3))
        {
            var vectorRect = new Rect(windowRect.width * 0.01f, windowRect.height * _yPosFactor + _fieldOffset,
                windowRect.width * 0.8f, _fieldHeight);
            value = EditorGUI.Vector3Field(vectorRect, label, (Vector3)value);

            lineCount = 2.5f;
        }
        else if (value.GetType() == typeof(bool))
        {
            var boolRect = new Rect(windowRect.width * 0.21f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f),
            windowRect.width * 0.05f, _fieldHeight);
            GUI.Label(_label, label);
            value = EditorGUI.Toggle(boolRect, (bool)value);
        }
        else if (value.GetType() == typeof(float))
        {
            GUI.Label(_label, label);
            value = EditorGUI.FloatField(_field, (float)value);
            if (parameters.Length == 2) value = GUI.HorizontalSlider(_slider, (float)value, (float)parameters[0], (float)parameters[1]);
        }
        else if (value.GetType() == typeof(Texture2D))
        {
            GUI.Label(_label, label);
            var textureRect = new Rect(windowRect.width * 0.21f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f),
            windowRect.width * 0.10f, _fieldHeight * 3.5f);
            value = EditorGUI.ObjectField(textureRect, (UnityEngine.Object)value, typeof(Texture2D), true);
            lineCount = 3.5f;
        }
        else if (value.GetType() == typeof(Material))
        {
            GUI.Label(_label, label);
            var materialRect = new Rect(windowRect.width * 0.15f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f),
            windowRect.width * 0.80f, _fieldHeight);
            value = EditorGUI.ObjectField(materialRect, (UnityEngine.Object)value, typeof(Material), true);
        }
        else if (value.GetType() == typeof(string))
        {
            GUI.Label(_label, label);
            var seedRect = new Rect(windowRect.width * 0.21f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f), windowRect.width * 0.40f, _fieldHeight);
            value = EditorGUI.TextField(seedRect, (string)value);
        }

        GUILayout.EndHorizontal();

        _fieldOffset += _fieldHeight * lineCount;
        windowRect = GetFieldRects(windowRect);

        return (T)value;
    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject == null) return;
        if (Selection.activeGameObject.TryGetComponent(out TerrainGenerator gen))
        {
            _selected = Selection.activeGameObject;
        }
        
        //if(Selection.activeGameObject)_selected = Selection.activeGameObject;
        if(Selection.count > 0) _selectionCount = Selection.count;
    }
}
