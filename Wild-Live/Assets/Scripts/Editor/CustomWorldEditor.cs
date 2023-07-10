using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class CustomWorldEditor : EditorWindow
{
    #region Variables
    private static readonly Vector2 _minSize = new Vector2(500f, 500f);
    private static readonly Vector2 _maxSize = new Vector2(800f, 800f);
    private static Rect _windowDefault = new(100, 50, 500, 500);

    private readonly string[] _toolbar = { "Terrain Settings", "Noise Settings" };
    private readonly string[] _settings = { "Map Size", "Resolution", "Terrain Position", "Include noise" };
    private int _toolbarIndex = 0;
    private float _yPosFactor = 0.05f;
    private float _fieldOffset = 0f;
    private TerrainData _terrainData;
    private Material _terrainMaterial;
    private GameObject _generator = null;

    private EditorManager _editorManager;
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
        _editorManager = EditorManager.Instance;
        _editorManager.WorldEditor = this;
        _terrainData = (TerrainData)EditorGUIUtility.Load("Assets/Resources/DefaultTerrain.asset");
        _terrainMaterial = (Material)EditorGUIUtility.Load("Assets/Resources/DefaultTerrainMat.mat");
    }

    [MenuItem("Tools/World Editor")]
    public static void DrawWindow()
    {
        var window = EditorWindow.GetWindowWithRect(typeof(CustomWorldEditor), _windowDefault);
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
        else
        {
            GUILayout.Label("TEST");
        }
        //GUILayout.Label("Terrain Settings", EditorStyles.boldLabel);

    }


    private void DrawTerrainSettings(Rect windowRect)
    {
        //.2f so the field only takes 20% of the window
        _fieldWidth = windowRect.width * 0.2f;
        //.6f so the field only takes 60% of the window
        _sliderWidth = windowRect.width * 0.6f;
        _fieldHeight = EditorGUIUtility.singleLineHeight;
        windowRect = GetFieldRects(windowRect);

        GUILayout.BeginHorizontal();
        GUI.Label(_label, _settings[0]);
        _terrainData.MapSize = (int)EditorGUI.FloatField(_field, _terrainData.MapSize);
        _terrainData.MapSize = (int)GUI.HorizontalSlider(_slider, _terrainData.MapSize, 100f, 1000f);
        GUILayout.EndHorizontal();
        _fieldOffset += _fieldHeight;
        windowRect = GetFieldRects(windowRect);
        GUILayout.BeginHorizontal();
        GUI.Label(_label, _settings[1]);
        _terrainData.Resolution = (int)EditorGUI.FloatField(_field, _terrainData.Resolution);
        _terrainData.Resolution = (int)GUI.HorizontalSlider(_slider, _terrainData.Resolution, 100f, 255f);
        GUILayout.EndHorizontal();

        //GUILayout.Toggle

        if (GUI.Button(new Rect(windowRect.width * 0.05f, windowRect.height * 0.95f, windowRect.width * 0.9f, _fieldHeight), "Generate Terrain"))
        {
            if (_generator != null) DestroyImmediate(_generator);
            var clone = Instantiate(_generator = new GameObject("Editor Terrain Generator"), _terrainData.TerrainPosition, Quaternion.identity);
            DestroyImmediate(clone);
            _generator.GetOrAddComponent<TerrainGenerator>().EditorWorldGeneration(_terrainData, _terrainMaterial);
        }

        //field offset needs to be reset, otherwise the field would move constantly downwards
        _fieldOffset = 0f;
    }

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
}
