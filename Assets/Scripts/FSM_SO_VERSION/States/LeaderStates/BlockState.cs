using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Block State", menuName = "Custom SO/FSM States/Leader States/Leader Block State", order = 0)]
public class BlockState : State
{
    float blockStateMaxTimer, blockStateTimer = 0;
    private Dictionary<EntityModel, DataBlockState> _entitiesData = new Dictionary<EntityModel, DataBlockState>();
    private class DataBlockState
    { 
        public CharacterModel model;
        public CharacterController controller;

        public DataBlockState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();

        }
    }

    public override void EnterState(EntityModel model)
    {
        //Debug.Log("FSM Leader Block ENTER");
        _entitiesData.Add(model, new DataBlockState(model));
        blockStateMaxTimer = _entitiesData[model].model.CharAIData.BlockStateTimer;
        _entitiesData[model].model.View.CharacterMoveAnimation(false);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Block EXECUTE " + _entitiesData[model].model.gameObject.name);
        blockStateTimer += Time.deltaTime;
        if (blockStateTimer < blockStateMaxTimer)
        {
            _entitiesData[model].model.HealthController.CanReceiveDamage = false;
            //_entitiesData[model].model.View.CharacterBlockAnimation();

        }
        else
        {
                           blockStateTimer = 0;
            _entitiesData[model].model.HealthController.CanReceiveDamage = true;
            _entitiesData[model].model.IsBlocking = false;
            _entitiesData[model].model.IsAttacking = true;
            //_entitiesData[model].model.IsAttackDone = false;
        }

    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader Block EXIT");
        _entitiesData.Remove(model);
    }
}