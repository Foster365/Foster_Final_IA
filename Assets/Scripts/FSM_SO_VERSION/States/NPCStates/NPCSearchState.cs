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
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataSearchState(model));
        modelRef = _entitiesData[model].model;
        searchStateMaxTimer = modelRef.CharAIData.SearchStateTimer;
        //_entitiesData[model].model.View.PlayWalkAnimation(false);
        pathfindingLastPosition = Vector3.zero;
        _nextWaypoint = 0;
        waypointIndexModifier = 1;
        var pos = GenerateRandomPosition(_entitiesData[model].model) + modelRef.transform.position;
        _entitiesData[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
           modelRef.transform.position, (modelRef.transform.position + new Vector3(model.CharAIData.RandomPositionThreshold, 0, model.CharAIData.RandomPositionThreshold)*-1));
        //var finalPath = _entitiesData[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.finalPath;
        //var finalPathNode = finalPath[finalPath.Count - 1];
        _entitiesData[model].model.gameObject.GetComponent<Leader>().target = _entitiesData[model].model.gameObject.transform;//_entitiesData[model].grid.GetNodeFromWorldPoint(_entitiesData[model].controller.CharAIController.AStarPathFinding.finalPath.Count-1);


    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC SEARCH ENTER EXECUTE");
        searchStateTimer += Time.deltaTime;
        var aiController = modelRef.GetComponent<CharacterController>().CharAIController;
        var finalPath = aiController.AStarPathFinding.finalPath;

        _entitiesData[model].controller.CharAIController.LineOfSight();

        Search(_entitiesData[model].model, finalPath);
        CheckPathRegeneration(_entitiesData[model].model, _entitiesData[model].model);

        if (searchStateTimer >= searchStateMaxTimer)
        {
            IdleSearch(_entitiesData[model].model);
        }
        else if (_entitiesData[model].controller.CharAIController.IsTargetInSight)
        {
            _entitiesData[model].model.IsSearching = false;
            _entitiesData[model].model.IsSeek = true;
        }

    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
    void CheckPathRegeneration(CharacterModel model, EntityModel _model)
    {
        if (availableToRegeneratePath)
        {
            _entitiesData[model].model.GetComponent<CharacterController>().CharAIController.AStarPathFinding.FindPath(
               modelRef.transform.position, (modelRef.transform.position + new Vector3(model.CharAIData.RandomPositionThreshold, 0, model.CharAIData.RandomPositionThreshold))*-1);
            availableToRegeneratePath = false;
        }
    }
    Vector3 GenerateRandomPosition(EntityModel model)
    {
        return new Vector3(Random.Range(0, model.CharAIData.RandomPositionThreshold), 0, Random.Range(0, model.CharAIData.RandomPositionThreshold));
    }

    public void Search(CharacterModel model, List<Node> _finalPath)
    {
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
        Vector3 _patrollingLastPos = _entitiesData[model].model.transform.position;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < _finalPath.Count; i++)
        {
            if (Vector3.Distance(_finalPath[i].worldPosition, _patrollingLastPos) > .5f)
            {
                _patrollingLastPos = _finalPath[i].worldPosition;
                _waypoints = _finalPath;
            }
            else return;
            Run(_entitiesData[model].model, _waypoints);
        }
    }

    public void IdleSearch(CharacterModel model)
    {
        _entitiesData[model].model.View.CharacterMoveAnimation(false);
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
            _entitiesData[model].model.dirToMove = dir;
            model.LookDir(dir.normalized + _entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
            model.Move(dir.normalized + _entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
        }
        else availableToRegeneratePath = true;
    }

}