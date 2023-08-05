using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Health Below Regular Attack Threshold?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is Health Below Regular Attack Threshold?")]
public class IsBelowRegularAttackThreshold : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        CharacterModel charModel = model as CharacterModel;

        return charModel.HealthController.CurrentHealth < charModel.EnhancedAttackHealthThreshold;
    }
}