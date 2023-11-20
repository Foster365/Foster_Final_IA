using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "IsAttack?", menuName = "Custom SO/FSM Conditions/NPC Conditions/Is Attack?")]
public class IsAttack : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsAttack;
    }
}