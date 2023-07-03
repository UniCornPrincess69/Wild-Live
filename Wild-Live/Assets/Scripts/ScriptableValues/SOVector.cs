using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOVector", menuName = "ScriptableObjects/Vector")]
public class SOVector : ScriptableValue
{
    [field: SerializeField]
    public Vector3 Value { get; set; } = Vector3.zero;
}
