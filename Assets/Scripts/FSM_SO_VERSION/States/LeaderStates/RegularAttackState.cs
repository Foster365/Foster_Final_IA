using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Regular Attack State", menuName = "Custom SO/FSM States/Leader States/Leader Regular Attack State", order = 0)]
public class RegularAttackState : State
{
    float attackMaxCooldown = 1.5f, attackCooldown;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, model as CharacterModel);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("Attacking enemy");
    }

    public override void ExitState(EntityModel model)
    {
    }
}