using UnityEngine;

public class HunterAttackState : BaseState
{
    #region Variables
    private FSM_Hunter fsm = null;
    private Prey target = null;
    #endregion

    public HunterAttackState(FSM_Hunter fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        Debug.Log("Attacking prey!");
        var attacktarget = fsm.hunter.AttackTarget.GetComponentInParent<Herd>();
        float smallestDistance = float.MaxValue;
        foreach (var prey in attacktarget.Preys)
        {
            var dist = Vector3.Distance(prey.transform.position, fsm.hunter.transform.position);
            if (dist < smallestDistance)
            {
                target = prey;
                smallestDistance = dist;
            }
        }
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (target != null)
        {
            Debug.Log("Attack target acquired");
            fsm.hunter.Agent.destination = target.transform.position;
        }
    }

}
