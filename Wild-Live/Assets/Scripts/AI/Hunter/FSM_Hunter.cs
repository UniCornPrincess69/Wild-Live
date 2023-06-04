using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Hunter : FSM
{
    #region Variables
    private HunterAttackState attackState = null;
    private HunterEatState eatState = null;
    private HunterHuntState huntState = null;
    private HunterRestState restState = null;
    private HunterRoamState hunterRoamState = null;

    public Hunter hunter = null;
    #endregion

    public enum States
    {
        Attack,
        Eat,
        Hunt,
        Rest,
        Roam
    }

    // Initialization and Assignment of the hunter to it's state machine and state
    public void Initialize(Hunter hunter)
    {
        this.hunter = hunter;

        attackState = new HunterAttackState(this);
        eatState = new HunterEatState(this);
        huntState = new HunterHuntState(this);
        restState = new HunterRestState(this);
        hunterRoamState = new HunterRoamState(this);
    }

    /// <summary>
    /// Method to change the states of the AI and making sure that the ExitState method was run.
    /// </summary>
    /// <param name="states">State to be changed into</param>
    public void ChangeState(States states)
    {
        if (CurrentState != null) { CurrentState.ExitState(); }
        switch (states)
        {
            case States.Attack:
                CurrentState = attackState;
                CurrentState.EnterState();
                break;
            case States.Eat:
                CurrentState = eatState;
                CurrentState.EnterState();
                break;
            case States.Roam:
                CurrentState = hunterRoamState;
                CurrentState.EnterState();
                break;
            case States.Hunt:
                CurrentState = huntState;
                CurrentState.EnterState();
                break;
            case States.Rest:
                CurrentState = restState;
                CurrentState.EnterState();
                break;
        }
    }

    //Check if the current state is null, and setting it to the roam state.
    private void Start()
    {
        if (CurrentState == null)
        {
            CurrentState = hunterRoamState;
        }
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
