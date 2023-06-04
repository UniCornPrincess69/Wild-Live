using UnityEngine;

//BaseState is inherited by every other state to give them the needed methods
public abstract class BaseState
{
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
