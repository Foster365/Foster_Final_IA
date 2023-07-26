using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : EntityModel
{
    //Components variables
    Rigidbody rb;
    Grid mapGrid;
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

    #endregion

    #region Unity Engine Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        View = GetComponent<EntityView>();
    }

    private void Start()
    {
        pathfindingLastPosition = Vector3.zero;
        HealthController = new HealthController(this);
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

    public override StateData[] GetStates() => Data.FsmStates;
    public override Vector3 GetFoward() => transform.forward;

    public override float GetSpeed() => rb.velocity.magnitude;
    public override void Move(Vector3 direction)
    {
        direction.y = 0;
        //direction += _obstacleAvoidance.GetDir() * multiplier;
        rb.velocity = direction.normalized * (Data.MovementSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, direction, Data.RotationSpeed * Time.deltaTime);
        //View.PlayWalkAnimation(true);
    }

    public override void LookDir(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        direction.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * Data.RotationSpeed);
    }

    public override Rigidbody GetRigidbody() => rb;

    public override EntityModel GetModel() => this;
}
