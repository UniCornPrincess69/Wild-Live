using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Dynamic;

#if UNITY_EDITOR
public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null) Initialize();
            return _instance;
        }
    }


    private static void Initialize()
    {
        _instance = Resources.Load<T>($"Assets/Scripts/Editor/{typeof(T)}.asset");
        _instance = CreateInstance<T>();
    }
}
#endif
