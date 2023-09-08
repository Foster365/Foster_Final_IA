using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Can Attack?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Can Attack?")]
public class CanAttack : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        var thisModel = (CharacterModel)model;

        return model.IsAttacking;
    }
}