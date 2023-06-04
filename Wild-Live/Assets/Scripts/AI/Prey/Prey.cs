using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FSM_Prey))]
public class Prey : Animal
{
    #region Variables
    [field: SerializeField]
    public Herd Herd { get; set; } = null;

    [field: SerializeField]
    public FSM_Prey PreyFSM { get; set; } = null;

    private bool isDead = false;

    [field: SerializeField]
    public Transform Hunter { get; set; } = null;

    [field: SerializeField]
    public Transform Grass { get; set; } = null;

    private float fleeDistance = 0;
    private Quaternion originalRot = Quaternion.identity;
    #endregion



    /// <summary>
    /// Getting hunter Transform to flee from it.
    /// Setting state to flee, uses the Transform
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter"))
        {
            Hunter = other.GetComponent<Transform>();
            PreyFSM.ChangeState(FSM_Prey.State.Flee);
        }
        if (other.CompareTag("Grass"))
        {
            Grass = other.GetComponent<Transform>();
            PreyFSM.ChangeState(FSM_Prey.State.Graze);
        }
    }


    private void Awake()
    {
        PreyFSM = GetComponent<FSM_Prey>();
        PreyFSM.Initialize(this);
        Herd = GetComponentInParent<Herd>();
        Herd.IsStationaryChanged += IsStationaryChanged;
        Agent = GetComponent<NavMeshAgent>();
    }

    //Checking if the Hunter is still in attacking distance, otherwise returning to roam state.
    //Mainly checking the environment and sending info to the FSM.
    private void Update()
    {
        if (Hunter != null) 
        {
            fleeDistance = Vector3.Distance(transform.position, Hunter.transform.position);
            if (fleeDistance > 12f)
            {
                PreyFSM.ChangeState(FSM_Prey.State.Roam);
                Hunter = null;
            }
        }
    }

    private void OnDisable()
    {
        Herd.IsStationaryChanged -= IsStationaryChanged;
    }

    /// <summary>
    /// Rotation of the prey in random direction
    /// </summary>
    /// <param name="val">Parameter acquired throuhg subscription</param>
    private void IsStationaryChanged(bool val)
    {
        if (val == true)
        {
            RandomRotation();
        }
        else 
        {
            transform.localRotation = originalRot;
        }
    }



    /// <summary>
    /// Method to rotate the animals in random directions
    /// </summary>
    public void RandomRotation()
    {
        transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
    }

    /// <summary>
    /// Method to destroy the prey upon death.
    /// </summary>
    private void OnDeath()
    {
        Herd.Preys.Remove(this);
        Destroy(gameObject);
    }
}
