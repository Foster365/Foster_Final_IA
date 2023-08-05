using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Death State", menuName = "Custom SO/FSM States/Leader States/Leader Death State", order = 0)]
public class DeathState : State
{
    float attackMaxCooldown = 1.5f, attackCooldown;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {


    }

    public override void ExecuteState(EntityModel model)
    {
    }

    public override void ExitState(EntityModel model)
    {
    }
}