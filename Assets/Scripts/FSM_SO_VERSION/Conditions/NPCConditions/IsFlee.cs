using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "IsFlee?", menuName = "Custom SO/FSM Conditions/NPC Conditions/Is Flee?")]
public class IsFlee : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsFlee;
    }
}