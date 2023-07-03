using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBool", menuName = "ScriptableObjects/Bool")]
public class SOBool : ScriptableValue
{
    [field: SerializeField]
    public bool Value { get; set; } = false;



}
