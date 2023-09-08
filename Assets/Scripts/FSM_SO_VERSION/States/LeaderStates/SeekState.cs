using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Seek State", menuName = "Custom SO/FSM States/Leader States/Leader Seek State", order = 0)]
public class SeekState : State
{

    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //

    //Waypoints
    bool readyToMove = false;
    int _nextWaypoint, waypointIndexModifier;
    //

    float patrolStateTimer = 0, maxTimer;
    float attackMaxCooldown = 1.5f, attackCooldown;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    CharacterController charController;
    public override void EnterState(EntityModel model)
    {
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, model as CharacterModel);
        charController = _entitiesData[model].GetComponent<CharacterController>();
        charController.CharAIController.AStarPathFinding.FindPath(_entitiesData[model].transform.position, charController.CharAIController.Target.position);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("Seek state execute");
        var enemyPrevPosition = Vector3.zero;

        var finalPath = charController.CharAIController.AStarPathFinding.finalPath;

        if (finalPath.Count > 0)
        {
            Vector3 _patrollingLastPos = _entitiesData[model].transform.position;
            enemyPrevPosition = charController.CharAIController.Target.position;

            if (Vector3.Distance(charController.CharAIController.Target.position, _entitiesData[model].transform.position) <= _entitiesData[model].Data.AttackRange)
            {
                _entitiesData[model].IsAttacking = true;
            }

            List<Node> _waypoints = new List<Node>();
            for (int i = 0; i < finalPath.Count; i++)
            {
                if (Vector3.Distance(finalPath[i].worldPosition, _patrollingLastPos) > 1)
                {
                    _patrollingLastPos = finalPath[i].worldPosition;
                    _waypoints = finalPath;
                    Run(_entitiesData[model], _waypoints);
                }
                else return;

            }

            if (Vector3.Distance(finalPath[finalPath.Count - 1].worldPosition, _entitiesData[model].transform.position) <= 1)
            {
                pathfindingLastPosition = finalPath[finalPath.Count - 1].worldPosition;
                charController.CharAIController.AStarPathFinding.FindPath(
                    pathfindingLastPosition, charController.CharAIController.Target.position);
            }

        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    public void Run(CharacterModel model, List<Node> _waypoints)
    {
        if (_nextWaypoint <= _waypoints.Count - 1)
        {
            var waypointPosition = _waypoints[_nextWaypoint].worldPosition;
            waypointPosition.y = _entitiesData[model].transform.position.y;
            Vector3 dir = waypointPosition - _entitiesData[model].transform.position;
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