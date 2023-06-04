using UnityEngine;

public class HunterRestState : BaseState
{
    #region Variables
    private FSM_Hunter fsm = null;
    private float restCounter = 0f; 
    #endregion
    
    public HunterRestState(FSM_Hunter fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        Debug.Log("Enter rest state");
        restCounter = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("Exit rest state!");
        fsm.ChangeState(FSM_Hunter.States.Roam);
        restCounter = 0f;
    }

    public override void UpdateState()
    {
        if (restCounter >= 8f)
        {
            ExitState();
        }
        restCounter += Time.deltaTime;
    }
}
