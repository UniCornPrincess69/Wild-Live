using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering.UI;
using Codice.CM.Client.Differences.Graphic;

public class CustomWorldEditor : EditorWindow
{
    #region Variables
    private static readonly Vector2 _minSize = new Vector2(500f, 500f);
    private static readonly Vector2 _maxSize = new Vector2(800f, 800f);
    private static Rect _windowDefault = new(100, 50, 500, 500);

    private readonly string[] _toolbar = { "Terrain Settings", "Noise Settings", "About" };
    private readonly string[] _settings = { "Map Size", "Resolution", "Terrain Position", "Include noise" };
    private readonly string[] _noiseSettings = { "Noise Intensity", "Height Map" };
    private int _toolbarIndex = 0;
    private float _yPosFactor = 0.05f;
    private float _fieldOffset = 0f;
    private bool _useCheck = false;
    private TerrainData _terrainData;
    private Material _terrainMaterial;
    private GameObject _generator = null;

    #endregion


    #region Rect-Variables
    private float _fieldWidth = 0f;
    private float _fieldHeight = 0f;
    private float _sliderWidth = 0f;
    private Rect _slider = default;
    private Rect _field = default;
    private Rect _label = default;
    #endregion


    private void OnEnable()
    {
        _terrainData = (TerrainData)EditorGUIUtility.Load("Assets/Resources/DefaultTerrain.asset");
        _terrainMaterial = (Material)EditorGUIUtility.Load("Assets/Resources/DefaultTerrainMat.mat");

        if (FindObjectOfType<TerrainGenerator>())
        {
            if (EditorUtility.DisplayDialog("Keep Terrain?", "Do you wish to keep the previously generated Terrain?", "Okily", "Nopes"))
            {
                _generator = FindObjectOfType<TerrainGenerator>().gameObject;
            }
            else DestroyImmediate(FindObjectOfType<TerrainGenerator>().gameObject);
        }

    }

    [MenuItem("Tools/World Editor")]
    public static void DrawWindow()
    {
        var window = EditorWindow.GetWindowWithRect(typeof(CustomWorldEditor), _windowDefault);
        window.minSize = _minSize;
        window.maxSize = _maxSize;
        window.titleContent = new GUIContent("World Editor");
        //var list = FindObjectsOfType<TerrainGenerator>();
        //if (list.Length > 0) for (int i = list.Length - 1; i >= 0; i--) DestroyImmediate(list[i].gameObject);
        //if (FindObjectOfType<TerrainGenerator>(/*"Editor Terrain Generator"*/))
        //{
        //}


    }

    private void Awake()
    {
        if (_generator != null)
        {
            Debug.Log("Awake");
            if (EditorUtility.DisplayDialog("Keep Terrain?", "Do you wish to keep the previously generated Terrain?", "Yes"))
            {
                _generator = FindObjectOfType<TerrainGenerator>().gameObject;
            }
            else DestroyImmediate(_generator);
        }
        else return;
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
                var noiseMessage = new Rect(windowRect.width * .35f, windowRect.height * .35f, windowRect.width, windowRect.height * .35f);
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

    private void DrawNoiseSettings(Rect windowRect)
    {
        //.2f so the field only takes 20% of the window
        _fieldWidth = windowRect.width * 0.2f;
        //.6f so the field only takes 60% of the window
        _sliderWidth = windowRect.width * 0.6f;
        _fieldHeight = EditorGUIUtility.singleLineHeight;
        windowRect = GetFieldRects(windowRect);

        // Noise Intensity Slider
        GUILayout.BeginHorizontal();
        GUI.Label(_label, _noiseSettings[0]);
        _terrainData.NoiseIntensity = (int)EditorGUI.FloatField(_field, _terrainData.NoiseIntensity);
        _terrainData.NoiseIntensity = (int)GUI.HorizontalSlider(_slider, _terrainData.NoiseIntensity, 0f, 50f);
        GUILayout.EndHorizontal();

        _fieldOffset += _fieldHeight;
        windowRect = GetFieldRects(windowRect);

        _fieldOffset = 0f;
    }

    /// <summary>
    /// Drawing of the TerrainSettings window
    /// </summary>
    /// <param name="windowRect"></param>
    private void DrawTerrainSettings(Rect windowRect)
    {
        EditorGUI.BeginChangeCheck();

        //.2f so the field only takes 20% of the window
        _fieldWidth = windowRect.width * 0.2f;
        //.6f so the field only takes 60% of the window
        _sliderWidth = windowRect.width * 0.6f;
        _fieldHeight = EditorGUIUtility.singleLineHeight;
        windowRect = GetFieldRects(windowRect);

        _terrainData.MapSize = (int)StartField<float>(_settings[0], _terrainData.MapSize, ref windowRect, 100f, 5000f);
        _terrainData.Resolution = (int)StartField<float>(_settings[1], _terrainData.Resolution, ref windowRect, 100f, 255f);
        _terrainData.TerrainPosition = StartField<Vector3>(_settings[2], _terrainData.TerrainPosition, ref windowRect);

        // Map Size Slider
        //GUILayout.BeginHorizontal();
        //GUI.Label(_label, _settings[0]);
        //_terrainData.MapSize = (int)EditorGUI.FloatField(_field, _terrainData.MapSize);
        //_terrainData.MapSize = (int)GUI.HorizontalSlider(_slider, _terrainData.MapSize, 100f, 5000f);
        //GUILayout.EndHorizontal();

        //_fieldOffset += _fieldHeight;
        //windowRect = GetFieldRects(windowRect);

        //Resolution Slider
        //GUILayout.BeginHorizontal();
        //GUI.Label(_label, _settings[1]);
        //_terrainData.Resolution = (int)EditorGUI.FloatField(_field, _terrainData.Resolution);
        //_terrainData.Resolution = (int)GUI.HorizontalSlider(_slider, _terrainData.Resolution, 100f, 255f);
        //GUILayout.EndHorizontal();

        //_fieldOffset += _fieldHeight;
        //windowRect = GetFieldRects(windowRect);

        // Position for Terrain
        //GUILayout.BeginHorizontal();
        //var vectorRect = new Rect(windowRect.width * 0.01f, windowRect.height * _yPosFactor + _fieldOffset,
        //    windowRect.width * 0.8f, _fieldHeight);
        //_terrainData.TerrainPosition = EditorGUI.Vector3Field(vectorRect, _settings[2], _terrainData.TerrainPosition);

        //GUILayout.EndHorizontal();

        //_fieldOffset += _fieldHeight * 2;
        //windowRect = GetFieldRects(windowRect);

        //Noise Settings Toggle
        GUILayout.BeginHorizontal();
        var boolRect = new Rect(windowRect.width * 0.17f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f),
            windowRect.width * 0.05f, _fieldHeight);
        GUI.Label(_label, _settings[3]);
        _terrainData.IncludeNoise = EditorGUI.Toggle(boolRect, _terrainData.IncludeNoise);
        GUILayout.EndHorizontal();

        //t = EditorGUILayout.ObjectField(t, typeof(Transform), false) as Transform;



        if (GUI.Button(new Rect(windowRect.width * 0.05f, windowRect.height * 0.95f, windowRect.width * 0.9f, _fieldHeight), "Generate Terrain") || EditorGUI.EndChangeCheck())
        {
            if (Selection.count == 1 && Selection.activeGameObject.TryGetComponent(out TerrainGenerator gen))
            {
                gen.EditorWorldGeneration(_terrainData, _terrainMaterial);
            }
            else
            {
                if (_generator != null) DestroyImmediate(_generator);
                _generator = new GameObject("Editor Terrain Generator");
                _generator.transform.position = _terrainData.TerrainPosition;
                _generator.GetOrAddComponent<TerrainGenerator>().EditorWorldGeneration(_terrainData, _terrainMaterial);
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
        var labelPosX = windowRect.width - _fieldWidth;
        var fieldPosX = windowRect.width + _label.width + _fieldWidth;
        var sliderPosX = windowRect.width + _label.width + _field.width;
        var windowYPos = windowRect.height * _yPosFactor;

        //Magic numbers are all tested out factors so the coords fit into the window
        _label = new Rect(labelPosX * 0.01f, windowYPos + _fieldOffset, _fieldWidth, _fieldHeight);
        _field = new Rect(fieldPosX * 0.1f, windowYPos + _fieldOffset, _fieldWidth, _fieldHeight);
        _slider = new Rect(sliderPosX * 0.25f, windowYPos + _fieldOffset, _sliderWidth, _fieldHeight);
        return windowRect;
    }

    private T StartField<T>(string label, object value, ref Rect windowRect, params object[] parameters)
    {
        int lineCount = 1;

        GUILayout.BeginHorizontal();

        if(value.GetType() == typeof(int))
        {
            GUI.Label(_label, label);
            value = EditorGUI.FloatField(_field, (int)value);
            if (parameters.Length == 2) value = GUI.HorizontalSlider(_slider, (float)value, (float)parameters[0], (float)parameters[1]);
        }
        else if(value.GetType() == typeof(Vector3))
        {
            var vectorRect = new Rect(windowRect.width * 0.01f, windowRect.height * _yPosFactor + _fieldOffset,
                windowRect.width * 0.8f, _fieldHeight);
            value = EditorGUI.Vector3Field(vectorRect, label, (Vector3)value);

            lineCount = 2;
        }
        else if(value.GetType() == typeof(bool))
        {
            var boolRect = new Rect(windowRect.width * 0.17f, windowRect.height * _yPosFactor + (_fieldOffset + 0.6f),
            windowRect.width * 0.05f, _fieldHeight);
            GUI.Label(_label, label);
            value = EditorGUI.Toggle(boolRect, label, (bool)value);
        }

        GUILayout.EndHorizontal();

        _fieldOffset += _fieldHeight * lineCount;
        windowRect = GetFieldRects(windowRect);

        return (T)value;
    }
}
