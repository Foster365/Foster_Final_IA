using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Patrol State", menuName = "Custom SO/FSM States/Leader States/Leader Patrol State", order = 0)]
public class PatrolState : State
{

    //Waypoints
    bool readyToMove = false;
    int _nextWaypoint, waypointIndexModifier;
    //

    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //


    float patrolStateTimer = 0, maxTimer;
    private Dictionary<EntityModel, DataMovementState> _movementDatas = new Dictionary<EntityModel, DataMovementState>();
    CharacterModel modelRef;

    private class DataMovementState
    {
        public CharacterModel model;
        public int patrolCount;
        public bool travelBackwards;
        public List<Node> nodesToWaypoint;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            patrolCount = 0;
             grid = model.MapGrid;
            travelBackwards = false;
            nodesToWaypoint = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        if (!_movementDatas.ContainsKey(model)) _movementDatas.Add(model, new DataMovementState(model));
        modelRef = _movementDatas[model].model;
        maxTimer = _movementDatas[model].model.CharAIData.PatrolTimer;
        //_movementDatas[model].model.View.PlayWalkAnimation(false);
        var pos = GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position;
        if (_movementDatas[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.finalPath == null)
        {
            pathfindingLastPosition = Vector3.zero;
            _nextWaypoint = 0;
            waypointIndexModifier = 1;
            _movementDatas[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
               modelRef.transform.position, GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position);
        }
       
    }

    public override void ExecuteState(EntityModel model) 
    {
        Debug.Log("Leader patrol state execute");
        Debug.Log("Patrol timer: " + patrolStateTimer);
        //Debug.Log("patrol timer: " + patrolStateTimer);
        patrolStateTimer += Time.deltaTime;
        var finalPath = modelRef.GetComponent<CharacterController>().CharAIController.AStarPathFinding.finalPath;
        //if (finalPath.Count > 0)
        //{
        //    Patrol(_movementDatas[model].model, finalPath);
        //    if (Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _movementDatas[model].model.transform.position) <= 1)
        //    {
        //        pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
        //        _movementDatas[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
        //            pathfindingLastPosition, GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position);
        //        Patrol(_movementDatas[model].model, finalPath);
        //    }
        //}
        //else
        //{
        //    modelRef.IsPatrolling = false;
        //    patrolStateTimer = 0;
        //}
        if (patrolStateTimer >= maxTimer)
        {
            patrolStateTimer = 0;
            modelRef.IsPatrolling = false;
        }
    }

    public override void ExitState(EntityModel model)
    {
        _movementDatas.Remove(model);
    }

    Vector3 GenerateRandomPosition(EntityModel model)
    {
        return new Vector3(Random.Range(0, 100), 0, Random.Range(0, 100));
    }

    public void Patrol(CharacterModel model, List<Node> _finalPath)
    {
        Vector3 _patrollingLastPos = Vector3.zero;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < _finalPath.Count; i++)
        {
            if (Vector3.Distance(_finalPath[i].worldPosition, _patrollingLastPos) > 1)
            {
                //Debug.Log("Ok to find path");
                //Debug.Log("Patrolling node: " + patrollingNodes[i].name);
                _patrollingLastPos = _finalPath[i].worldPosition;
                //_pathfinding.FindPath(transform.position, _patrollingLastPos, IsSatisfies);
                _waypoints = _finalPath;
                Run(model, _waypoints);
            }
            else return;
        }
    }
    public void Run(CharacterModel model, List<Node> _waypoints)
    {
        if (_nextWaypoint <= _waypoints.Count-1)
        {
            var waypointPosition = _waypoints[_nextWaypoint].worldPosition;
            waypointPosition.y = _movementDatas[model].model.transform.position.y;
            Vector3 dir = waypointPosition - _movementDatas[model].model.transform.position;
            if (dir.magnitude < 1)
            {
                if (_nextWaypoint + waypointIndexModifier >= _waypoints.Count || _nextWaypoint + waypointIndexModifier < 0)
                {
                    waypointIndexModifier *= 1;
                }
                _nextWaypoint += waypointIndexModifier;
                readyToMove = true;
            }
            model.Move(dir.normalized);
        }

    }
}
