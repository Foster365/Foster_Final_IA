using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Death State", menuName = "Custom SO/FSM States/Leader States/Leader Death State", order = 0)]
public class DeathState : State
{
    float attackMaxCooldown = 1.5f, attackCooldown;
    private Dictionary<EntityModel, DeathStateData> _entitiesData = new Dictionary<EntityModel, DeathStateData>();

    public class DeathStateData
    {
        public CharacterModel model;
        public CharacterController controller;

        public DeathStateData(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();

        }
    }

    public override void EnterState(EntityModel model)
    {
        Debug.Log("NPC/LEADER DEATH STATE ENTER");
        _entitiesData.Add(model, new DeathStateData(model));
        _entitiesData[model].model.DeathHandler();
    }

    public override void ExecuteState(EntityModel model)
    {
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
}