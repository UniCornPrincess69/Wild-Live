using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EditorManager : ScriptableSingleton<EditorManager>
{
    public CustomWorldEditor WorldEditor { get; set; } = null;

    public int MapSize { get; set; } = 0;
    public int Resolution { get; set; } = 0;
    public Vector3 Position { get; set; } = Vector3.zero;
}
#endif
