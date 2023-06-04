using UnityEngine;

public class HunterHuntState : BaseState
{
    #region Variables
    private FSM_Hunter fsm = null;
    #endregion
    
    public HunterHuntState(FSM_Hunter fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        Debug.Log("Enter hunt state");
        if (fsm.hunter.SearchPosition == Vector3.zero) fsm.ChangeState(FSM_Hunter.States.Roam);
    }

    public override void ExitState()
    {
        Debug.Log("Exit hunt state");
    }

    public override void UpdateState()
    {
    }
}
