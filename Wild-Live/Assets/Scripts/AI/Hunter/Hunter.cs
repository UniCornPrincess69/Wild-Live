using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent), typeof(FSM_Hunter))]
public class Hunter : Animal
{
    #region Variables

    [field: SerializeField]
    public GameObject AttackTarget { get; private set; } = null;

    [field: SerializeField]
    public FSM_Hunter HunterFSM { get; set; } = null;

    [field: SerializeField]
    public Vector3 SearchPosition { get; private set; } = Vector3.zero;

    [SerializeField]
    private int roamLocations = 10;

    private float attackDistance = default;
    #endregion

    private void Awake()
    {
        HunterFSM = GetComponent<FSM_Hunter>();
        if (Agent == null) Agent = GetComponent<NavMeshAgent>();
        Agent.enabled = true;
        HunterFSM.Initialize(this);

    }


    //Checking if the Herd is still in attacking distance, otherwise returning to hunt state.
    //Mainly checking the environment and sending info to the FSM.
    void Update()
    {
        if (AttackTarget != null)
        {
            attackDistance = Vector3.Distance(transform.position, AttackTarget.transform.position);
            if (attackDistance > 10f)
            {
                HunterFSM.ChangeState(FSM_Hunter.States.Hunt);
                AttackTarget = null;
            }
        }
    }

    //Getting information of the Scent or the Herd itself.
    //The info of the scent is the location of the next scent.
    //The info of the herd is the transfrom of the herd, to keep following it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scent"))
        {
            Debug.Log("Entering Scent");
            SearchPosition = other.gameObject.GetComponent<Scent>().NextScentPos.position;
            HunterFSM.ChangeState(FSM_Hunter.States.Hunt);
        }

        if (other.CompareTag("Herd"))
        {
            Debug.Log("Herd detected");
            AttackTarget = other.gameObject;
            HunterFSM.ChangeState(FSM_Hunter.States.Attack);
        }
    }

}
