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

    float patrolStateTimer = 0, patrolStateMaxTimer;
    private Dictionary<EntityModel, DataMovementState> _movementDatas = new Dictionary<EntityModel, DataMovementState>();
    CharacterModel modelRef;

    private class DataMovementState
    {
        public CharacterModel model;
        public int patrolCount = 0;
        public bool travelBackwards;
        public List<Node> nodesToWaypoint;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
             grid = model.MapGrid;
            travelBackwards = false;
            nodesToWaypoint = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        //Debug.Log("FSM Leader Patrol ENTER");
        if (!_movementDatas.ContainsKey(model)) _movementDatas.Add(model, new DataMovementState(model));
        modelRef = _movementDatas[model].model;
        patrolStateMaxTimer = modelRef.CharAIData.PatrolTimer;
        //_movementDatas[model].model.View.PlayWalkAnimation(false);
        var pos = GenerateRandomPosition(_movementDatas[model].model) + modelRef.transform.position;
            pathfindingLastPosition = Vector3.zero;
            _nextWaypoint = 0;
            waypointIndexModifier = 1;
            _movementDatas[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
               modelRef.transform.position, pos);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Patrol EXECUTE " + _movementDatas[model].model.gameObject.name);
        patrolStateTimer += Time.deltaTime;
        var aiController = modelRef.GetComponent<CharacterController>().CharAIController;
        var finalPath = aiController.AStarPathFinding.finalPath;
        if(patrolStateTimer < patrolStateMaxTimer)
        {
            aiController.LineOfSight();
            if (finalPath.Count > 0)
            {
                Patrol(_movementDatas[model].model, finalPath); 
                if (Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _movementDatas[model].model.transform.position) < 1)
                {
                    pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
                    aiController.AStarPathFinding.FindPath(
                        pathfindingLastPosition, GenerateRandomPosition(model) - modelRef.transform.position);
                    Patrol(_movementDatas[model].model, finalPath);
                }
                //Debug.Log("Patrol dist: " + Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _movementDatas[model].model.transform.position));
                //HandlePathRegeneration(_movementDatas[model].model, finalPath);
            }
            else if (aiController.IsTargetInSight) modelRef.IsChasing = true;
        }
        else
        {
            modelRef.IsPatrolling = false;
            patrolStateTimer = 0;
        }
    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader Patrol EXIT");
        _movementDatas.Remove(model);
    }

    Vector3 GenerateRandomPosition(EntityModel model)
    {
        return new Vector3(Random.Range(0, model.CharAIData.RandomPositionThreshold), 0, Random.Range(0, model.CharAIData.RandomPositionThreshold)) ;
    }

    void HandlePathRegeneration(CharacterModel model, List<Node> finalPath)
    {
        if (Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, model.transform.position) <= 1)
        {
            pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
            modelRef.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
                pathfindingLastPosition, GenerateRandomPosition(model) + modelRef.transform.position);
            Patrol(model, finalPath);
        }
    }

    public void Patrol(CharacterModel model, List<Node> _finalPath)
    {
        Vector3 _patrollingLastPos = model.transform.position;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < _finalPath.Count; i++)
        {
            if (Vector3.Distance(_finalPath[i].worldPosition, _patrollingLastPos) > 1)
            {
                _patrollingLastPos = _finalPath[i].worldPosition;
                _waypoints = _finalPath;
            }
            else return;
            Run(model, _waypoints);
        }
    }
    public void Run(CharacterModel model, List<Node> _waypoints)
    {
        //Debug.Log("next wp " + _nextWaypoint + "nodes count " + (_waypoints.Count - 1));
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
