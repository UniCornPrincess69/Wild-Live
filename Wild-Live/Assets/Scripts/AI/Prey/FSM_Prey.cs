using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FSM_Prey : FSM
{
    #region Variables
    private PreyFleeState fleeState = null;
    private PreyGrazeState grazeState = null;
    private PreyRoamState preyRoamState = null;
    private PreySleepState sleepState = null;

    public Prey Prey { get; set; } = null;
    #endregion


    public enum State
    {
        Flee,
        Graze,
        Roam,
        Sleep
    }

    // Initialization and Assignment of the prey to it's state machine and state
    public void Initialize(Prey prey)
    {
        this.Prey = prey;
        fleeState = new PreyFleeState(this);
        grazeState = new PreyGrazeState(this);
        preyRoamState = new PreyRoamState(this);
        sleepState = new PreySleepState(this);
    }

    /// <summary>
    /// Method to change the states of the AI and making sure that the ExitState method was run.
    /// </summary>
    /// <param name="states">State to be changed into</param>
    public void ChangeState(State states)
    {
        //if (CurrentState != null) { CurrentState.ExitState(); }
        CurrentState?.ExitState();

        switch (states)
        {
            case State.Flee:
                CurrentState = fleeState;
                CurrentState.EnterState();
                break;
            case State.Graze:
                CurrentState = grazeState;
                CurrentState.EnterState();
                break;
            case State.Roam:
                CurrentState = preyRoamState;
                CurrentState.EnterState();
                break;
            case State.Sleep:
                CurrentState = sleepState;
                CurrentState.EnterState();
                break;
        }
    }

    //Check if the current state is null, and setting it to the roam state.
    private void Start()
    {
        if (CurrentState == null) CurrentState = preyRoamState;
        CurrentState.EnterState();
    }

    //Regular Update of the UpdateState function of the current state
    private void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState();
        }

    }
}
