using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour
{

    #region Variables
    [field: SerializeField, Range(1f, 20f)]
    public float TimeSpeed { get; set; } = 0f;

    private Transform _light = null;
    private float _lightRot = 50f;
    #endregion
    void Awake()
    {
        GameWorldManager.Instance.WorldTime = this;
        _light = GetComponent<Transform>();
    }

    void Update()
    {
        _lightRot += TimeSpeed * Time.deltaTime;
        SetRotation( _lightRot );
    }

    /// <summary>
    /// Setting the rotation of the main light source (sun)
    /// </summary>
    /// <param name="xRot">Float for the x axis rotation</param>
    public void SetRotation(float xRot) 
    {
        _light.rotation = Quaternion.Euler(xRot, _light.rotation.y, _light.rotation.z);
    }
}
