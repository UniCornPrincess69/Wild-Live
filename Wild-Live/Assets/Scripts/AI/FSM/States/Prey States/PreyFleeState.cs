using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PreyFleeState : BaseState
{
    #region Variables
    private FSM_Prey fsm = null;
    private Vector3 fleePosition = Vector3.zero;
    private Transform hunter = null;
    #endregion

    public PreyFleeState(FSM_Prey fsm)
    {
        this.fsm = fsm;
    }

    private Vector3 FleeDirection(Transform transform)
    {
        var direction = fsm.Prey.transform.position + (fsm.Prey.transform.position - transform.position);
        return direction;
    }

    public override void EnterState()
    {
        Debug.Log("Fleeing!!!!!!");
        hunter = fsm.Prey.Hunter;
    }

    public override void ExitState()
    {
        Debug.Log("End of fleeing!");
    }

    public override void UpdateState()
    {
        if (hunter == null) ExitState();
        fleePosition = FleeDirection(hunter);
        fsm.Prey.Agent.destination = fleePosition;
    }
}
