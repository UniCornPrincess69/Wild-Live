using UnityEngine;

public class PreySleepState : BaseState
{
    #region Variables
    private FSM_Prey fsm = null;
    #endregion
    
    public PreySleepState(FSM_Prey fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        fsm.Prey.Agent.enabled = false;
        fsm.Prey.Herd.IsStationary = true;
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
