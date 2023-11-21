using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Seek State", menuName = "Custom SO/FSM States/NPC States/NPC Seek State", order = 0)]
public class NPCSeekState : State
{
    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //

    //Waypoints
    bool readyToMove = false;
    int _nextWaypoint, waypointIndexModifier;
    //

    private Dictionary<EntityModel, DataMovementState> _entitiesData = new Dictionary<EntityModel, DataMovementState>();
    List<Node> finalPath = new List<Node>();
    CharacterController charController;

    private class DataMovementState
    {
        public CharacterModel model;
        public CharacterController controller;
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC Seek ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataMovementState(model));
        if (_entitiesData[model].controller.CharAIController.Target != null)
            _entitiesData[model].controller.CharAIController.AStarPathFinding.FindPath(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
        finalPath = _entitiesData[model].controller.CharAIController.AStarPathFinding.finalPath;
        _nextWaypoint = 0;
        waypointIndexModifier = 1;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Seek EXECUTE " + _entitiesData[model].model.gameObject.name);
        var enemyPrevPosition = Vector3.zero;

        if (finalPath.Count > 0)
        {
            Seek(_entitiesData[model].model, finalPath);
            var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
            //Debug.Log("Dist " + dist);
            if (dist < _entitiesData[model].model.Data.AttackRange)
            {
                Debug.Log("Pasa a attack");
                _entitiesData[model].model.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //_entitiesData[model].model.IsAttacking = true;
                //_entitiesData[model].model.View.CharacterMoveAnimation(false);
            }

        }
    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader Seek EXIT");
        _entitiesData.Remove(model);
    }
    public void Seek(CharacterModel model, List<Node> _finalPath)
    {
        Vector3 _seekLastPos = model.transform.position;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < _finalPath.Count; i++)
        {
            if (Vector3.Distance(_finalPath[i].worldPosition, _seekLastPos) > .5f)
            {
                _seekLastPos = _finalPath[i].worldPosition;
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
            //Debug.Log("Next wp entro a run");
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
                readyToMove = true;
            }
            model.Move(dir.normalized);
        }

    }
}