using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Can Patrol?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Can Patrol?")]
public class CanPatrol : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsPatrolling;
    }
}