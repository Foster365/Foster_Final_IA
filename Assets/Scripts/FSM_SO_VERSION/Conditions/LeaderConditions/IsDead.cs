using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Dead?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is Dead?")]
public class IsDead : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        CharacterModel charModel = model as CharacterModel;

        return charModel.HealthController.IsDead;
    }
}