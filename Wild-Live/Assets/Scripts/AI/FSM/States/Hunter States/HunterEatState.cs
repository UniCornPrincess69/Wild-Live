using UnityEngine;

public class HunterEatState : BaseState
{
    #region Variables
    private FSM_Hunter fsm = null;
    #endregion

    public HunterEatState(FSM_Hunter fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
