using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection.Emit;

public class CustomWorldEditor : EditorWindow
{
    private static readonly Vector2 _minSize = new Vector2(500f, 500f);
    private static readonly Vector2 _maxSize = new Vector2(800f, 800f);

    private readonly string[] _toolbar = { "Terrain Settings", "Noise Settings" };
    private int _toolbarIndex = 0;
    private int _mapSize = 100;
    private int _resolution = 20;


    [MenuItem("Tools/World Editor")]
    public static void DrawWindow()
    {
        var window = EditorWindow.GetWindowWithRect(typeof(CustomWorldEditor), new Rect(100, 50, 500, 500));
        window.minSize = _minSize;
        window.maxSize = _maxSize;
        window.titleContent = new GUIContent("World Editor");
    }

    private void OnGUI()
    {
        _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbar);
        if (_toolbarIndex == 0)
        {
            var sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
            sliderStyle.stretchWidth = true;

            var thumb = new GUIStyle(GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            GUILayout.Label("MapSize");
            _mapSize = (int)GUI.HorizontalSlider(new(140, 25, 300, 20), _mapSize, 100f, 1000f, sliderStyle, thumb) ;
            _mapSize = (int)EditorGUI.FloatField(new(85, 25, 45, 20), _mapSize);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Resolution");
            _resolution = (int)GUI.HorizontalSlider(new(140, 45, 300, 20), _resolution, 20f, 255f);
            _resolution = (int)EditorGUI.FloatField(new(85, 45, 45, 20), _resolution);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("TEST");
        }
        //GUILayout.Label("Terrain Settings", EditorStyles.boldLabel);

    }

}
