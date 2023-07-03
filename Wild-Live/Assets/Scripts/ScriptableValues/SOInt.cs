using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInt", menuName = "ScriptableObjects/Int")]
public class SOInt : ScriptableValue
{
    [field: SerializeField]
    public int Value { get; set; } = 0;

}
