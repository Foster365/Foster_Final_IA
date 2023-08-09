using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Patrol State", menuName = "Custom SO/FSM States/Leader States/Leader Patrol State", order = 0)]
public class PatrolState : State
{
    private Dictionary<EntityModel, DataMovementState> _movementDatas = new Dictionary<EntityModel, DataMovementState>();
    private class DataMovementState
    {
        public float timer;
        public CharacterModel model;
        public float patrolCount;
        public bool travelBackwards;
        public List<Node> nodesToWaypoint;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            timer = model.CharAIData.PatrolTimer;
            patrolCount = 0;
            grid = model.MapGrid;
            travelBackwards = false;
            nodesToWaypoint = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {

        if (!_movementDatas.ContainsKey(model)) _movementDatas.Add(model, new DataMovementState(model));
        //_movementDatas[model].model.View.PlayWalkAnimation(false);
        _movementDatas[model].model.Controller.CharAIController.AStarPathFinding.FindPath();
        model.IsPatrolling = true;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("Leader patrol state");
        if (_movementDatas[model].patrolCount <= _movementDatas[model].model.CharAIData.PatrolTimer)
        {
            _movementDatas[model].patrolCount += Time.deltaTime;
        }
        else
        {
            //_movementDatas[model].model.IsPatrolling = false;
        }
        //Ac� corro Astar.
        //var patrolPoints = _movementDatas[model].model.GetPatrolPoints();

        //var distToNextPoint = Vector3.Distance(patrolPoints[_movementDatas[model].patrolCount].transform.position, model.transform.position);
        //if (distToNextPoint == 0)
        //{
        //    model.Move(Vector3.zero);
        //}
        ////Si estoy lejos del punto, me muevo hacia el
        //if (distToNextPoint > 1f)
        //{
        //    var dirToNextPoint = (patrolPoints[_movementDatas[model].patrolCount].transform.position - model.transform.position).normalized;

        //    model.Move(dirToNextPoint);
        //}
        //else
        //{
        //    //Si ya estoy cerca, me quedo quiero un tiempo X

        //    model.GetRigidbody().velocity = Vector3.zero;
        //    _movementDatas[model].timer -= Time.deltaTime;

        //    //Si ese tiempo X ya paso, chequeo si tengo que seguir el mismo recorrido
        //    //O tengo que cambiar el sentido de la patrulla
        //    if (_movementDatas[model].timer <= 0 && !_movementDatas[model].travelBackwards)
        //    {
        //        _movementDatas[model].patrolCount++;

        //        _movementDatas[model].timer = _movementDatas[model].model.GetData().RestPatrolTime;
        //    }
        //    else if (_movementDatas[model].timer <= 0 && _movementDatas[model].travelBackwards)
        //    {

        //        _movementDatas[model].patrolCount--;

        //        _movementDatas[model].timer = _movementDatas[model].model.GetData().RestPatrolTime;
        //    }
        //}

        ////Chequeo si tengo que cambiar el sentido de la patrulla en base al count actual
        ////si ya llegue al final, cambio el sentido
        //if (_movementDatas[model].patrolCount >= patrolPoints.Length - 1)
        //{
        //    _movementDatas[model].travelBackwards = true;

        //}
        //else if (_movementDatas[model].patrolCount <= 0)
        //{
        //    _movementDatas[model].travelBackwards = false;
        //}
    }

    public override void ExitState(EntityModel model)
    {
        _movementDatas[model].model.IsPatrolling = false;

        _movementDatas.Remove(model);
    }
}
