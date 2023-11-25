using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Search State", menuName = "Custom SO/FSM States/NPC States/NPC Search State", order = 0)]
public class NPCSearchState : State
{
    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //

    //Waypoints
    bool availableToRegeneratePath = false;
    int _nextWaypoint, waypointIndexModifier;
    //

    float searchStateTimer = 0, searchStateMaxTimer;
    float searchIdleStateTimer = 0, searchIdleStateMaxTimer;
    float pathfindingWPIndexModifier = 0;
    CharacterModel modelRef;

    private Dictionary<EntityModel, DataSearchState> _entitiesData = new Dictionary<EntityModel, DataSearchState>();
    List<Node> finalPath = new List<Node>();
    CharacterController charController;

    private class DataSearchState
    {
        public CharacterModel model;
        public CharacterController controller;
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataSearchState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }
    public override void EnterState(EntityModel model)
    {
        //Debug.Log("FSM Leader SEARCH ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataSearchState(model));
        modelRef = _entitiesData[model].model;
        searchStateMaxTimer = modelRef.CharAIData.SearchTimer;
        //_entitiesData[model].model.View.PlayWalkAnimation(false);
        pathfindingLastPosition = Vector3.zero;
        _nextWaypoint = 0;
        waypointIndexModifier = 1;
        var pos = GenerateRandomPosition(_entitiesData[model].model) + (modelRef.transform.position * -1);
        _entitiesData[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
           modelRef.transform.position, pos);
    }

    public override void ExecuteState(EntityModel model)
    {
        searchStateTimer += Time.deltaTime;
        var aiController = modelRef.GetComponent<CharacterController>().CharAIController;
        var finalPath = aiController.AStarPathFinding.finalPath;

        _entitiesData[model].controller.CharAIController.LineOfSight();

        Search(_entitiesData[model].model, finalPath);
        CheckPathRegeneration(aiController, _entitiesData[model].model);

        if (searchStateTimer >= searchStateMaxTimer)
        {
            IdleSearch(_entitiesData[model].model);
        }
        else if(_entitiesData[model].controller.CharAIController.IsTargetInSight)
        {
            _entitiesData[model].model.IsSearching = false;
            _entitiesData[model].model.IsSeek = true;
        }

    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader SEARCH EXIT");
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
        return new Vector3(Random.Range(0, model.CharAIData.RandomPositionThreshold), 0, Random.Range(0, model.CharAIData.RandomPositionThreshold));
    }

    public void Search(CharacterModel model, List<Node> _finalPath)
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

    public void IdleSearch(CharacterModel model)
    {
        _entitiesData[model].model.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        searchIdleStateTimer += Time.deltaTime;
        if (searchIdleStateTimer >= searchIdleStateMaxTimer)
        {
            searchStateTimer = 0;
            searchIdleStateTimer = 0;
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
            model.Move(dir.normalized + _entitiesData[model].model.gameObject.GetComponent<CharacterController>().CharAIController.SbObstacleAvoidance.GetDir() + _entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
        }
        else availableToRegeneratePath = true;
    }

}