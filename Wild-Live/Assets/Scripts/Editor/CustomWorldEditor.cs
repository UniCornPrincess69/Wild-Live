using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using Codice.CM.SEIDInfo;
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
    }

    [MenuItem("Tools/World Editor")]
    public static void DrawWindow()
    {
        var window = EditorWindow.GetWindowWithRect(typeof(CustomWorldEditor), _windowDefault);
        window.minSize = _minSize;
        window.maxSize = _maxSize;
        window.titleContent = new GUIContent("World Editor");
        
        TestLoader.Init();
    }

    private void OnGUI()
    {
            
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
        var windowYPos = windowRect.height * _yPosFactor;

        //Magic numbers are all tested out factors so the coords fit into the window
        _label = new Rect(labelPosX * 0.01f, windowYPos, _fieldWidth, _fieldHeight);
        _field = new Rect(fieldPosX * 0.1f, windowYPos, _fieldWidth, _fieldHeight);
        _slider = new Rect(sliderPosX * 0.25f, windowYPos, _sliderWidth, _fieldHeight);

        

        GUILayout.BeginHorizontal();
        GUI.Label(_label, _settings[0]);
        //TODO: To implement a for loop i need to exchange the variable! Maybe Dict, or something else?!
        //manager.MapSize = TestLoader.List["Map Size"];
        _editorManager.MapSize = (int)EditorGUI.FloatField(_field, _editorManager.MapSize);
        _editorManager.MapSize = (int)GUI.HorizontalSlider(_slider, _editorManager.MapSize, 100f, 1000f);
        GUILayout.EndHorizontal();

        //GUILayout.Toggle

    }
}
