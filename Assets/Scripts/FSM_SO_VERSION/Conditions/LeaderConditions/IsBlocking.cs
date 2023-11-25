using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Blocking", menuName = "Custom SO/FSM Conditions/Leader Conditions/Is Blocking")]
public class IsBlocking : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsBlocking;
    }
}