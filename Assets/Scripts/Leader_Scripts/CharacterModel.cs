using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterModel : MonoBehaviour
{
    //Components variables
    Rigidbody rb;
    Grid mapGrid;
    CharacterAnimationsController charAnimController;
    LeaderAIController charAIController;
    HealthController characterHealthController;

    //

    //Waypoints
    [SerializeField] List<Transform> patrollingNodes; //TODO ver con patrol state en FSM SO
    [SerializeField] List<Transform> wp; //Patrol simple waypoints  //TODO ver con patrol state en FSM SO
    public List<Transform> waypoints;
    List<Node> targetSeekNodes;
    public float rotationSpeed = 1;
    bool readyToMove = false;
    int _nextWaypoint = 0, waypointIndexModifier = 1;
    //

    //Pathfinding variables
    Vector3 pathfindingLastPosition;

    //

    //Character Variables
    [Header("Basic character props")]
    public float speed = .1f;

    Transform target;
    bool isBlocking = false;
    #region Encapsulated Variables

    public Vector3 PathfindingLastPosition { get => pathfindingLastPosition; set => pathfindingLastPosition = value; }
    public Transform Target { get => target; set => target = value; }
    public Grid MapGrid { get => mapGrid; set => mapGrid = value; }
    public bool ReadyToMove { get => readyToMove; set => readyToMove = value; }
    public List<Node> TargetSeekNodes { get => targetSeekNodes; set => targetSeekNodes = value; }
    public HealthController CharacterHealthController { get => characterHealthController; set => characterHealthController = value; }
    public CharacterAnimationsController CharAnimController { get => charAnimController; set => charAnimController = value; }

    #endregion

    #region Unity Engine Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        charAnimController = GetComponent<CharacterAnimationsController>();
        charAIController = GetComponent<LeaderAIController>();
        characterHealthController = GetComponent<HealthController>();
    }

    private void Start()
    {
        pathfindingLastPosition = Vector3.zero;
        new List<Node>();
        SetCharacterTag();
    }

    #endregion

    void SetCharacterTag()
    {
        if (gameObject.tag == TagManager.LEADER_TAG) characterHealthController.IsLeader = true;
        else if (gameObject.tag == TagManager.NPC_TAG) characterHealthController.IsNPC = true;
    }

}
