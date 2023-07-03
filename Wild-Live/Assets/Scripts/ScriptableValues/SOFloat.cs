using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "ScriptableObjects/Float")]
public class SOFloat : ScriptableValue
{
    [field: SerializeField]
    public float Value { get; set; } = 0f;
}
