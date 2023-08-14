using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EditorManager
{
    public CustomWorldEditor WorldEditor { get; set; } = null;

    //private Dictionary<string, ScriptableValue> _terrainValues = null;


    //public int MapSize { get; set; } = 0;
    //public int Resolution { get; set; } = 0;
    //public Vector3 Position { get; set; } = Vector3.zero;
}
#endif
