using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "IsSeek?", menuName = "Custom SO/FSM Conditions/NPC Conditions/Is Seek?")]
public class IsSeek : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsSeek;
    }
}