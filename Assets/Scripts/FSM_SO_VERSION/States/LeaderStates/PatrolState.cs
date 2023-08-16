using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Patrol State", menuName = "Custom SO/FSM States/Leader States/Leader Patrol State", order = 0)]
public class PatrolState : State
{
    private Dictionary<EntityModel, DataMovementState> _movementDatas = new Dictionary<EntityModel, DataMovementState>();
    CharacterModel modelRef;

    private class DataMovementState
    {
        public float timer, patrolTimer, restPatrolTime;
        public CharacterModel model;
        public int patrolCount;
        public bool travelBackwards;
        public List<Node> nodesToWaypoint;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            timer = model.CharAIData.PatrolTimer;
            patrolCount = 0;
            patrolTimer = 0;
            restPatrolTime = 0;
             grid = model.MapGrid;
            travelBackwards = false;
            nodesToWaypoint = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        if (!_movementDatas.ContainsKey(model)) _movementDatas.Add(model, new DataMovementState(model));
        modelRef = _movementDatas[model].model;
        //_movementDatas[model].model.View.PlayWalkAnimation(false);
        var pos = GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position;
        Debug.Log("Random Patrol position" + pos);
        _movementDatas[model].model.Controller.CharAIController.AStarPathFinding.FindPath(pos);
        Debug.Log("Path: " + _movementDatas[model].model.Controller.CharAIController.AStarPathFinding.finalPath);
        model.IsPatrolling = true;
    }

    public override void ExecuteState(EntityModel model)
    {
        var timer = _movementDatas[model].model.CharAIData.PatrolTimer;
        Debug.Log("Leader patrol state" + timer);
        //Debug.Log("patrol timer: " + _movementDatas[model].patrolTimer);
        if (_movementDatas[model].patrolTimer <= timer)
        {
            _movementDatas[model].patrolTimer += Time.deltaTime;
            if (_movementDatas[model].model.Controller.CharAIController.AStarPathFinding.finalPath.Count > 0)
            {
                var finalPath = modelRef.Controller.CharAIController.AStarPathFinding.finalPath;
                //Acá corro Astar.
                //var patrolPoints = _movementDatas[model].model.GetPatrolPoints();
                modelRef.Patrol(finalPath);
                if(_movementDatas[model].model.transform.position == finalPath[finalPath.Count-1].worldPosition)
                {
                    modelRef.Controller.CharAIController.AStarPathFinding.FindPath(GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position);
                }
            }
            //    var distToNextPoint = Vector3.Distance(finalPath[_movementDatas[model].patrolCount].worldPosition, model.transform.position);
            //    if (distToNextPoint == 0)
            //    {
            //        _movementDatas[model].model.Move(Vector3.zero);
            //    }
            //    //Si estoy lejos del punto, me muevo hacia el
            //    if (distToNextPoint > 1f)
            //    {
            //        var dirToNextPoint = (finalPath[_movementDatas[model].patrolCount].worldPosition - _movementDatas[model].model.transform.position).normalized;

            //        _movementDatas[model].model.Move(dirToNextPoint);
            //    }
            //    else
            //    {
            //        //Si ya estoy cerca, me quedo quiero un tiempo X

            //        _movementDatas[model].model.GetRigidbody().velocity = Vector3.zero;
            //        _movementDatas[model].timer -= Time.deltaTime;

            //        //Si ese tiempo X ya paso, chequeo si tengo que seguir el mismo recorrido
            //        //O tengo que cambiar el sentido de la patrulla
            //        if (_movementDatas[model].timer <= 0 && !_movementDatas[model].travelBackwards)
            //        {
            //            _movementDatas[model].patrolCount++;

            //            _movementDatas[model].timer = _movementDatas[model].restPatrolTime;
            //        }
            //        else if (_movementDatas[model].timer <= 0 && _movementDatas[model].travelBackwards)
            //        {

            //            _movementDatas[model].patrolCount--;

            //            _movementDatas[model].timer = _movementDatas[model].restPatrolTime;
            //        }
            //    }

            //    //Chequeo si tengo que cambiar el sentido de la patrulla en base al count actual
            //    //si ya llegue al final, cambio el sentido
            //    if (_movementDatas[model].patrolCount >= finalPath.Count - 1)
            //    {
            //        _movementDatas[model].travelBackwards = true;

            //    }
            //    else if (_movementDatas[model].patrolCount <= 0)
            //    {
            //        _movementDatas[model].travelBackwards = false;
            //    }
            //}

            else
            {
                //_movementDatas[model].model.IsPatrolling = false;
            }
        }
    }

    public override void ExitState(EntityModel model)
    {
        _movementDatas[model].model.IsPatrolling = false;

        _movementDatas.Remove(model);
    }

    Vector3 GenerateRandomPosition(EntityModel model)
    {
        return new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
    }
}
