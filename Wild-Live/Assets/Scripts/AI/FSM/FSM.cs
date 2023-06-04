using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class FSM : MonoBehaviour
{
    //BaseState, all other States inherit from this state
    
    protected BaseState CurrentState { get; set; } = null;

    protected BaseState PreviousState { get; set; } = null;
}
