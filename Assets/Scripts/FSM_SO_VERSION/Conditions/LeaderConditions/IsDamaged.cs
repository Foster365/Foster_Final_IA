using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Damaged?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is Damaged?")]
public class IsDamaged : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        CharacterModel charModel = model as CharacterModel;

        return charModel.HealthController.IsDamaged;
    }
}