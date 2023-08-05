using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is enemy on sight?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is enemy on sight?")]
public class IsEnemyOnSight : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        CharacterModel charModel = model as CharacterModel;
        return charModel.Controller.CharAIController.LineOfSight();
    }
}