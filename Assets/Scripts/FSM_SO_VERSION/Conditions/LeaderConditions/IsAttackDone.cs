using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Attack Done?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is Attack Done?")]
public class IsAttackDone : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        //TODO manejar este boolean desde afuera, con un triggerenter en el mapa.
        CharacterModel charModel = model as CharacterModel;
        return charModel.IsAttackDone;

    }
}
