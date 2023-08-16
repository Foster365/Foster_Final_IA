using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : EntityModel
{
    //Components variables
    Rigidbody rb;
    Grid mapGrid;
    CharacterController controller;
    HealthController healthController;
    //LeaderAIController charAIController;
    //

    //Waypoints
    List<Transform> patrollingNodes; //TODO ver con patrol state en FSM SO
    List<Transform> wp; //Patrol simple waypoints  //TODO ver con patrol state en FSM SO
    List<Node> targetSeekNodes;
    bool readyToMove = false;
    int _nextWaypoint = 0, waypointIndexModifier = 1;
    //

    //Pathfinding variables
    Vector3 pathfindingLastPosition;
    //

    Transform target;
    #region Encapsulated Variables

    public Vector3 PathfindingLastPosition { get => pathfindingLastPosition; set => pathfindingLastPosition = value; }
    public Transform Target { get => target; set => target = value; }
    public Grid MapGrid { get => mapGrid; set => mapGrid = value; }
    public bool ReadyToMove { get => readyToMove; set => readyToMove = value; }
    public List<Node> TargetSeekNodes { get => targetSeekNodes; set => targetSeekNodes = value; }
    public CharacterController Controller { get => controller; set => controller = value; }
    public HealthController HealthController { get => healthController; set => healthController = value; }
    #endregion

    #region Unity Engine Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        View = GetComponent<EntityView>();
        HealthController = new HealthController(Data.MaxHealth);
    }

    private void Start()
    {
        pathfindingLastPosition = Vector3.zero;
        //charAIController = new LeaderAIController();
        new List<Node>();
        SetCharacterTag();
    }

    #endregion

    void SetCharacterTag()
    {
        if (gameObject.tag == TagManager.LEADER_TAG) HealthController.IsLeader = true;
        else if (gameObject.tag == TagManager.NPC_TAG) HealthController.IsNPC = true;
    }

    public override StateData[] GetStates() => CharAIData.FsmStates;

    public override EntityData GetData() => Data;
    public override Vector3 GetFoward() => transform.forward;

    public override float GetSpeed() => rb.velocity.magnitude;
    public override void Move(Vector3 direction)
    {
        direction.y = 0;
        //direction += _obstacleAvoidance.GetDir() * multiplier;
        rb.velocity = direction * (Data.MovementSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, direction, Data.RotationSpeed * Time.deltaTime);
        //View.PlayWalkAnimation(true);
    }

    public override void LookDir(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        direction.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * Data.RotationSpeed);
    }

    public void Patrol(List<Node> _finalPath)
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
                Run(_waypoints);
            }
            else return;
        }
    }
    public void Run(List<Node> _waypoints)
    {
        if (_nextWaypoint <= _waypoints.Count - 1)
        {
            var waypoint = _waypoints[_nextWaypoint];
            var waypointPosition = waypoint.worldPosition;
            waypointPosition.y = transform.position.y;
            Vector3 dir = waypointPosition - transform.position;
            if (dir.magnitude < 1)
            {
                if (_nextWaypoint + waypointIndexModifier >= _waypoints.Count || _nextWaypoint + waypointIndexModifier < 0)
                    waypointIndexModifier *= -1;
                _nextWaypoint += waypointIndexModifier;
                readyToMove = true;
            }
            Move(dir.normalized);
        }

    }
    public override Rigidbody GetRigidbody() => rb;

    public override EntityModel GetModel() => this;

    public EntityModel GetTarget()
    {
        return null;
    }
}
