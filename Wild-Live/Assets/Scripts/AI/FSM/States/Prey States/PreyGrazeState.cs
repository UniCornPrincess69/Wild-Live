using UnityEngine;

public class PreyGrazeState : BaseState
{
    #region Variables
    private FSM_Prey fsm = null;
    private Transform grass = null;
    private float grazingCounter = 0f;
    #endregion

    public PreyGrazeState(FSM_Prey fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        grass = fsm.Prey.Grass;
        fsm.Prey.Agent.destination = grass.position;
    }

    public override void ExitState()
    {
        grass = null;
        fsm.Prey.Herd.IsStationary = false;
    }

    public override void UpdateState()
    {
        if (fsm.Prey.Herd.IsStationary)
        {
            grazingCounter += Time.deltaTime;
            if (grazingCounter >= 5f)
            {
                grazingCounter = 0f;
                fsm.ChangeState(FSM_Prey.State.Roam);
            }
        }
        else if (fsm.Prey.Agent.remainingDistance < 1f)
        {
            fsm.Prey.Herd.IsStationary = true;
        }
    }
}
