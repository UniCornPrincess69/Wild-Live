using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Animal is inherited by Prey and Hunter to give them the NavMeshAgent
[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [field: SerializeField]
    public NavMeshAgent Agent { get; set; }

}
