using System.Collections;
using System.Collections.Generic;
using UADE.IA.Flocking.States;
using UnityEngine;

public class NPCCharacterModel : MonoBehaviour
{
    HealthController characterHealthController;
    //NPCAIController npcAIController;
    EntityView characterAnimController;

    Rigidbody rb;

    Grid mapGrid;

    [SerializeField] float movementSpeed;
    Vector3 pathfindingLastPosition;

    #region Waypoints_Variables

    //Waypoints
    [SerializeField] List<Transform> patrollingNodes;
    [SerializeField] List<Transform> wp; //Patrol simple waypoints
    public List<Transform> waypoints;
    List<Node> targetSeekNodes;
    public float rotationSpeed = 1;
    bool readyToMove = false;
    int _nextWaypoint = 0, waypointIndexModifier = 1;
    //

    #endregion

    #region Encapsulated_Variables

    public HealthController CharacterHealthController { get => characterHealthController; }
    public Vector3 PathfindingLastPosition { get => pathfindingLastPosition; set => pathfindingLastPosition = value; }
    public Grid MapGrid { get => mapGrid; set => mapGrid = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }
    public bool ReadyToMove { get => readyToMove; set => readyToMove = value; }
    public EntityView CharacterAnimController { get => characterAnimController; set => characterAnimController = value; }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterHealthController = GetComponent<HealthController>();
        //npcAIController = GetComponent<NPCAIController>();
        characterAnimController = GetComponent<EntityView>();
    }

    private void Start()
    {
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        pathfindingLastPosition = Vector3.zero;
        new List<Node>();
        //SetCharacterTag();
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
            else if (readyToMove) Move(dir.normalized);
        }

    }
    public void MoveWithFlocking()
    {

        //npcAIController.CharacterPathfinding.FindPath(transform.position, npcAIController.FlockingGoalPos);


        //if (npcAIController.CharacterPathfinding.finalPath.Count > 1)
        //{
        //    if (_nextWaypoint <= npcAIController.CharacterPathfinding.finalPath.Count - 1)
        //    {
        //        var waypoint = npcAIController.CharacterPathfinding.finalPath[_nextWaypoint];
        //        var waypointPosition = waypoint.worldPosition;
        //        waypointPosition.y = transform.position.y;
        //        Vector3 dir = waypointPosition - transform.position;
        //        //Vector3 dir = npcAIController.FlockBehaviour(waypointPosition - transform.position);
        //        if (dir.magnitude < 1)
        //        {
        //            if (_nextWaypoint + waypointIndexModifier >= npcAIController.CharacterPathfinding.finalPath.Count || _nextWaypoint + waypointIndexModifier < 0)
        //                waypointIndexModifier *= -1;
        //            _nextWaypoint += waypointIndexModifier;
        //            readyToMove = true;
        //        }
        //        else if (readyToMove) Move(dir.normalized);
        //    }
            //npcAIController.FlockBehaviour();
            //Run(npcAIController.CharacterPathfinding.finalPath);// transform.Translate(0, 0, Time.deltaTime * flockingSpeed);
        //}

    }

    public void Move(Vector3 _dir)
    {
        this.transform.position += _dir * movementSpeed * Time.deltaTime;
    }
    public void Move(Vector3 _dir, float _flockSpeed)
    {
        _dir *= _flockSpeed;
        _dir.y = rb.velocity.y;
        transform.forward = Vector3.Lerp(transform.forward, _dir, rotationSpeed);
        rb.velocity = _dir;
    }
    public Vector3 GetRandomPosition()
    {
        float width = mapGrid.gridWorldSize.x;
        float height = mapGrid.gridWorldSize.y;

        float x = (Random.value * width) - (width / 2);
        float z = (Random.value * height) - (height / 2);

        return new Vector3(x, 0, z);
    }

    public void SeekBehaviour()
    {
        //if (npcAIController.CharacterLineOfSight.Target != null)
        //{
        //    npcAIController.CharacterPathfinding.FindPath(transform.position, npcAIController.CharacterLineOfSight.Target.position);
        //    RunSeek(npcAIController.CharacterPathfinding.finalPath);
        //}
    }
    void RunSeek(List<Node> _waypoints)
    {
        //if (_nextWaypoint <= _waypoints.Count - 1)
        //{
        //    var waypoint = _waypoints[_nextWaypoint];
        //    var waypointPosition = waypoint.worldPosition;
        //    waypointPosition.y = transform.position.y;
        //    Vector3 dir = waypointPosition - transform.position;
        //    if (dir.magnitude < 1)
        //    {
        //        if (_nextWaypoint + waypointIndexModifier >= _waypoints.Count || _nextWaypoint + waypointIndexModifier < 0)
        //            waypointIndexModifier *= -1;
        //        _nextWaypoint += waypointIndexModifier;
        //        readyToMove = true;
        //    }
        //    Rigidbody targetRB = npcAIController.CharacterLineOfSight.Target.gameObject.GetComponent<Rigidbody>();
        //    transform.position += Time.deltaTime * targetRB.velocity * movementSpeed; ;
        //    transform.forward = Vector3.Lerp(transform.forward, targetRB.velocity, rotationSpeed * Time.deltaTime);
        //    //npcAIController.NpcPursuitSteeringBehaviour.GetDir(dir);
        //}
    }

    public void Attack1()
    {
        characterAnimController.CharacterAttack1Animation();
    }
    public void Attack2()
    {
        characterAnimController.CharacterAttack2Animation();
    }
    public void Attack3()
    {
        characterAnimController.CharacterAttack3Animation();
    }
    public void Block1()
    {
        characterAnimController.CharacterBlock1Animation();
    }
}

