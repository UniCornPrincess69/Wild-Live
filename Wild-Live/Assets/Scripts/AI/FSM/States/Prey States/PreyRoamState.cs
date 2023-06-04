using UnityEngine;


public class PreyRoamState : BaseState
{
    #region Variables
    private FSM_Prey fsm = null;
    private float maxPos = 29;
    private float minPos = -29;
    private float waitCounter = 0f;
    #endregion


    public PreyRoamState(FSM_Prey fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        var posX = Random.Range(minPos, maxPos);
        var posY = Random.Range(minPos, maxPos);
        fsm.Prey.Agent.enabled = true;
        fsm.Prey.Agent.destination = new Vector3(posX, 0f, posY);
    }

    public override void ExitState()
    {
        Debug.Log("Exiting roaming");
    }

    public override void UpdateState()
    {
        if (fsm.Prey.Agent.remainingDistance <= 0.5f)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= 5f)
            {
                var posX = Random.Range(minPos, maxPos);
                var posY = Random.Range(minPos, maxPos);
                fsm.Prey.Agent.destination = new Vector3(posX, 0f, posY);
                waitCounter = 0f;
            }
        }
    }
}
