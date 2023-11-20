using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Follow Leader State", menuName = "Custom SO/FSM States/NPC States/Follow Leader State", order = 0)]
public class NPCFollowLeaderState : State
{
    float timer = 0;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();

    GameObject boss;
    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM Follow Leader START");
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;

        if (boss == null)
        {
            boss = GameObject.Find(_entitiesData[model].Data.BossName);
        }
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Follow Leader EXECUTE");
        _entitiesData[model].LookDir(_entitiesData[model].GetComponent<FlockingManager>().RunFlockingDir());
        _entitiesData[model].Move(_entitiesData[model].GetComponent<FlockingManager>().RunFlockingDir());

    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM Follow Leader EXIT");
        _entitiesData.Remove(model);
    }
}