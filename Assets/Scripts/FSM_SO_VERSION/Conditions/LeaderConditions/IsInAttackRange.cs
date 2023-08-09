using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is In Attack Range?", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is In Attack Range?")]
public class IsInAttackRange : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        var thisModel = (CharacterModel)model;

        return Vector3.Distance(thisModel.transform.position, thisModel.GetTarget().transform.position) < thisModel.GetData().AttackRange;
    }
}