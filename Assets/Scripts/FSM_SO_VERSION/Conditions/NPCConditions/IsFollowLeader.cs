using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "IsFollowLeader?", menuName = "Custom SO/FSM Conditions/NPC Conditions/Is Follow Leader?")]
public class IsFollowLeader : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsFollowLeader;
    }
}