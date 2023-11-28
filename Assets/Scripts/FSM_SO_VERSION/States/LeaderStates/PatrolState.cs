using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Patrol State", menuName = "Custom SO/FSM States/Leader States/Leader Patrol State", order = 0)]
public class PatrolState : State
{

    //Waypoints
    bool availableToRegeneratePath = false;
    int _nextWaypoint, waypointIndexModifier;
    //

    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //

    float patrolStateTimer = 0, patrolStateMaxTimer;
    float pathfindingWPIndexModifier = 0;
    private Dictionary<EntityModel, DataMovementState> _entitiesData = new Dictionary<EntityModel, DataMovementState>();
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
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataMovementState(model));
        modelRef = _entitiesData[model].model;
        patrolStateMaxTimer = modelRef.CharAIData.PatrolTimer;
        //_entitiesData[model].model.View.PlayWalkAnimation(false);
        var pos = GenerateRandomPosition(_entitiesData[model].model) + (modelRef.transform.position * -1);
        pathfindingLastPosition = Vector3.zero;
        _nextWaypoint = 0;
        waypointIndexModifier = 1;
        _entitiesData[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
           modelRef.transform.position, pos);
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Patrol EXECUTE " + _entitiesData[model].model.gameObject.name);

        patrolStateTimer += Time.deltaTime;
        var aiController = modelRef.GetComponent<CharacterController>().CharAIController;
        var finalPath = aiController.AStarPathFinding.finalPath;

        Patrol(_entitiesData[model].model, finalPath);
        CheckPathRegeneration(aiController, _entitiesData[model].model);
        
        if (patrolStateTimer >= patrolStateMaxTimer)
        {
            modelRef.IsPatrolling = false;
            patrolStateTimer = 0;
            _entitiesData[model].model.View.CharacterMoveAnimation(false);
        }
    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader Patrol EXIT");
        _entitiesData.Remove(model);
    }
    void CheckPathRegeneration(CharacterAIController _aiController, EntityModel _model)
    {
        if (availableToRegeneratePath)
        {
            _aiController.AStarPathFinding.FindPath(
                pathfindingLastPosition, GenerateRandomPosition(_model) + (modelRef.transform.position * -1));
            availableToRegeneratePath = false;
        }
    }
    Vector3 GenerateRandomPosition(EntityModel model)
    {
        return new Vector3(Random.Range(0, model.CharAIData.RandomPositionThreshold), 0, Random.Range(0, model.CharAIData.RandomPositionThreshold)) ;
    }

    public void Patrol(CharacterModel model, List<Node> _finalPath)
    {
        Vector3 _patrollingLastPos = model.transform.position;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < _finalPath.Count; i++)
        {
            if (Vector3.Distance(_finalPath[i].worldPosition, _patrollingLastPos) > .5f)
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
        if (_nextWaypoint <= _waypoints.Count - 1)
        {
            pathfindingLastPosition = modelRef.GetComponent<CharacterController>().CharAIController.AStarPathFinding.finalPath[modelRef.GetComponent<CharacterController>().CharAIController.AStarPathFinding.finalPath.Count - 1].worldPosition;

            var waypointPosition = _waypoints[_nextWaypoint].worldPosition;
            waypointPosition.y = _entitiesData[model].model.transform.position.y;
            Vector3 dir = waypointPosition - _entitiesData[model].model.transform.position;
            if (dir.magnitude < 1)
            {
                if (_nextWaypoint + waypointIndexModifier >= _waypoints.Count || _nextWaypoint + waypointIndexModifier < 0)
                {
                    waypointIndexModifier *= 1;
                }
                _nextWaypoint += waypointIndexModifier;
            }
            model.Move(dir.normalized + _entitiesData[model].model.gameObject.GetComponent<CharacterController>().CharAIController.SbObstacleAvoidance.GetDir());
        }
        else availableToRegeneratePath = true;
    }

    #region old execute method

    //public override void ExecuteState(EntityModel model)
    //{
    //Debug.Log("FSM Leader Patrol EXECUTE " + _entitiesData[model].model.gameObject.name);
    //patrolStateTimer += Time.deltaTime;
    //var aiController = modelRef.GetComponent<CharacterController>().CharAIController;
    //var finalPath = aiController.AStarPathFinding.finalPath;
    //Debug.Log("Wp pathfinding modifier " + pathfindingWPIndexModifier + "final path count " + finalPath.Count);
    //Patrol(_entitiesData[model].model, finalPath);
    //if (pathfindingWPIndexModifier > finalPath.Count)
    //{
    //    pathfindingWPIndexModifier = 0;
    //    aiController.AStarPathFinding.FindPath(
    //        pathfindingLastPosition, GenerateRandomPosition(model) - modelRef.transform.position);
    //    Patrol(_entitiesData[model].model, finalPath);
    //}
    //if (patrolStateTimer < patrolStateMaxTimer)
    //{
    //    Patrol(_entitiesData[model].model, finalPath);

    //    //modelRef.IsPatrolling = false;
    //    //patrolStateTimer = 0;
    //}
    //else
    //{ //aiController.LineOfSight();
    //    if (finalPath.Count > 0)
    //    {
    //        pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
    //        Patrol(_entitiesData[model].model, finalPath);
    //        //if (pathfindingWPIndexModifier > finalPath.Count)
    //        //{
    //        //    pathfindingWPIndexModifier = 0;
    //        //    aiController.AStarPathFinding.FindPath(
    //        //        pathfindingLastPosition, GenerateRandomPosition(model) - modelRef.transform.position);
    //        //    Patrol(_entitiesData[model].model, finalPath);
    //        //}
    //        //if (Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _entitiesData[model].model.transform.position) < 1)
    //        //{
    //        //    pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
    //        //    aiController.AStarPathFinding.FindPath(
    //        //        pathfindingLastPosition, GenerateRandomPosition(model) - modelRef.transform.position);
    //        //    Patrol(_entitiesData[model].model, finalPath);
    //        //}
    //        //Debug.Log("Patrol dist: " + Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _entitiesData[model].model.transform.position));
    //        //HandlePathRegeneration(_entitiesData[model].model, finalPath);
    //    }
    //    //else if (aiController.IsTargetInSight) modelRef.IsChasing = true;
    //}
    //}
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

    #endregion
}
