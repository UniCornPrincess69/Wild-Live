using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HunterRoamState : BaseState
{
    #region Variables
    private readonly FSM_Hunter fsm = null;
    private readonly float maxPos = 29;
    private readonly float minPos = -29;
    private float waitCounter = 0f;
    #endregion

    public HunterRoamState(FSM_Hunter fsm)
    {
        this.fsm = fsm;
    }

    public override void EnterState()
    {
        var posX = Random.Range(minPos, maxPos);
        var posY = Random.Range(minPos, maxPos);
        fsm.hunter.Agent.enabled = true;
        fsm.hunter.Agent.destination = new Vector3(posX, 0f, posY);
    }

    public override void ExitState()
    {
        Debug.Log("Exit roam state");
    }

    public override void UpdateState()
    {
        if (fsm.hunter.Agent.remainingDistance <= 0.5f)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= 6f)
            {
                var posX = Random.Range(minPos, maxPos);
                var posY = Random.Range(minPos, maxPos);
                fsm.hunter.Agent.destination = new Vector3(posX, 0f, posY);
                waitCounter = 0f;
            }
        }
    }
}
