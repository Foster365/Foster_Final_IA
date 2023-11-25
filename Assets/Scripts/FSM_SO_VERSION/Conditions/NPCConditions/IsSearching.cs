using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Is Searching For Target?", menuName = "Custom SO/FSM Conditions/NPC Conditions/Is Searching For Target?")]
public class IsSearching : StateCondition
{
    public override bool CompleteCondition(EntityModel model)
    {
        return model.IsSearching;
    }
}