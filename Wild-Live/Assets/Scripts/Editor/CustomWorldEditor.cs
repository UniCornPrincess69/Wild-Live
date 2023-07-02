using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class CustomWorldEditor : EditorWindow
{
    #region Variables
    private static readonly Vector2 _minSize = new Vector2(500f, 500f);
    private static readonly Vector2 _maxSize = new Vector2(800f, 800f);
    private static Rect _windowDefault = new(100, 50, 500, 500);

    private readonly string[] _toolbar = { "Terrain Settings", "Noise Settings" };
    private readonly string[] _settings = { "Map Size", "Resolution", "Terrain Position", "Include noise" };
    private int _toolbarIndex = 0;
    private int _mapSize = 100;
    private int _resolution = 20;
    private float _yPosFactor = 0.05f;
    private Vector3 _position = Vector3.zero;
    private bool _useNoise = false;

    private object[] values = null;
    #endregion



    #region Rect-Variables
    private float _fieldWidth = 0f;
    private float _fieldHeight = 0f;
    private float _sliderWidth = 0f;
    private Rect _slider = default;
    private Rect _field = default;
    private Rect _label = default;
    #endregion


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
        if (values == null) values = new object[] { _mapSize, _resolution, _position, _useNoise };
            
        var windowRect = position;
        
        _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbar);
        if (_toolbarIndex == 0)
        {
            DrawTerrainSettings(windowRect);

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Resolution");
            //_resolution = (int)GUI.HorizontalSlider(new(140, 45, 300, 20), _resolution, 20f, 255f);
            //_resolution = (int)EditorGUI.FloatField(new(85, 45, 45, 20), _resolution);
            //GUILayout.EndHorizontal();
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

        var labelPosX = windowRect.width - _fieldWidth;
        var fieldPosX = windowRect.width + _label.width + _fieldWidth;
        var sliderPosX = windowRect.width +_label.width + _field.width;

        //Magic numbers are all tested out factors so the coords fit into the window
        _label = new Rect(labelPosX * 0.01f, windowRect.height * _yPosFactor, _fieldWidth, _fieldHeight);
        _field = new Rect(fieldPosX * 0.1f, windowRect.height * _yPosFactor, _fieldWidth, _fieldHeight);
        _slider = new Rect(sliderPosX * 0.25f, windowRect.height * _yPosFactor, _sliderWidth, _fieldHeight);


        GUILayout.BeginHorizontal();
        GUI.Label(_label, _settings[0]);
        //TODO: To implement a for loop i need to exchange the variable! Maybe Dict, or something else?!
        values[0] = (int)EditorGUI.FloatField(_field, (float)values[0]);
        values[0] = (int)GUI.HorizontalSlider(_slider, (float)values[0], 100f, 1000f);
        GUILayout.EndHorizontal();

    }
}
