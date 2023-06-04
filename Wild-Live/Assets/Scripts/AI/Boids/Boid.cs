using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{
    #region Variables
    [field: SerializeField]
    public Flock Flock { get; set; } = null;

    private Vector2 direction = Vector2.zero; 
    #endregion


    private void Awake()
    {
        if (Flock == null)
        {
            Flock = GetComponentInParent<Flock>();
        }
    }

    private void Update()
    {
        transform.Translate(direction * (Flock.Speed * Time.deltaTime));
    }

    private void LateUpdate()
    {
        CenterMove();
    }

    private void CenterMove()
    {
        Vector2 faceDirection = (Flock.AveragePosition - (Vector2)transform.position).normalized;

        float deltaTimeStrength = Flock.CenterStrength * Time.deltaTime;
        direction = (Vector2)Flock.transform.position + direction + Time.deltaTime*faceDirection/(deltaTimeStrength + 1);
        direction = direction.normalized;
    }

    private void AvoidOthers()
    {

    }

    private void AlignWithOthers()
    {

    }
}
