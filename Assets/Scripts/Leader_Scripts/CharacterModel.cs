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
    [SerializeField] List<Transform> patrollingNodes;
    [SerializeField] List<Transform> wp; //Patrol simple waypoints
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

    void SetCharacterTag()
    {
        if (gameObject.tag == TagManager.LEADER_TAG) characterHealthController.IsLeader = true;
        else if (gameObject.tag == TagManager.NPC_TAG) characterHealthController.IsNPC = true;
    }

    public void Patrol(Pathfinding _pathfinding)
    {
        Vector3 _patrollingLastPos = Vector3.zero;
        List<Node> _waypoints = new List<Node>();
        for (int i = 0; i < patrollingNodes.Count; i++)
        {
            if (Vector3.Distance(patrollingNodes[i].position, _patrollingLastPos) > 1)
            {
                //Debug.Log("Ok to find path");
                //Debug.Log("Patrolling node: " + patrollingNodes[i].name);
                _patrollingLastPos = patrollingNodes[i].position;
                //_pathfinding.FindPath(transform.position, _patrollingLastPos, IsSatisfies);
                _waypoints = _pathfinding.finalPath;
                Run(_waypoints);
            }
            else return;
        }
    }
    bool IsSatisfies(Vector3 nodeCurrPos) //Era Node curr
    {
        return Vector3.Distance(nodeCurrPos, transform.position) < 5; //Era curr.worldposition
    }
    bool IsSatisfies(Node curr, Node _target)
    {
        return curr == _target;
    }
    public void PatrolSimple()
    {
        Run(wp);
    }
    private void Update()
    {
        HandleAnimationProps();
    }

    void HandleAnimationProps()
    {
        if (readyToMove) charAnimController.CharacterMoveAnimation(true);
        else charAnimController.CharacterMoveAnimation(false);
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = rb.velocity.y;
        transform.forward = Vector3.Lerp(transform.forward, dir, rotationSpeed);
        rb.velocity = dir;
    }
    public void SetWayPoints(List<Transform> newPoints) //<Node>
    {
        _nextWaypoint = 0;
        if (newPoints.Count == 0) return;
        //_anim.Play("CIA_Idle");
        //waypoints = newPoints;
        waypoints = wp;
        var pos = waypoints[_nextWaypoint].position;
        //var pos = waypoints[_nextPoint].worldPosition;
        pos.y = transform.position.y;
        transform.position = pos;
        readyToMove = true;
        //Debug.Log("Ok");
    }
    public void Run(List<Transform> _waypoints)
    {
        var waypoint = _waypoints[_nextWaypoint];
        var waypointPosition = waypoint.position;
        waypointPosition.y = transform.position.y;
        Vector3 dir = waypointPosition - transform.position;
        //Coroutine lookAtCoroutine = StartCoroutine(LookAtWp(waypointPosition));
        if (dir.magnitude < 3)
        {
            if (_nextWaypoint + waypointIndexModifier >= _waypoints.Count || _nextWaypoint + waypointIndexModifier < 0)
                waypointIndexModifier *= -1;
            _nextWaypoint += waypointIndexModifier;
            readyToMove = true;
        }
        Move(dir.normalized);
        //Move(charAIController.CharacterObstacleAvoidance.GetDir(dir.normalized));
        //else if (readyToMove) Move(charAIController.CharacterObstacleAvoidance.GetDir(dir.normalized));
        // return dir;

        //Debug.Log("Ok to run");
        //var point = _waypoints[_nextPoint];
        //var posPoint = point.position;
        ////var posPoint = point.worldPosition;
        //posPoint.y = transform.position.y;
        //Vector3 dir = posPoint - transform.position;
        //if (dir.magnitude < 0.2f)
        //{
        //    if (_nextPoint + 1 < _waypoints.Count)
        //        _nextPoint++;
        //    else
        //    {
        //        readyToMove = false;
        //        //_anim.SetTrigger("Finish");
        //        //_anim.SetFloat("Vel", 0);
        //        return;
        //    }
        //}
        //Move(dir.normalized);
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

    public IEnumerator LookAtTarget(Vector3 _targetPos)
    {
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - _targetPos);
        float timer = 0;
        while (timer < .5f)
        {
            Quaternion.Slerp(transform.rotation, lookRotation, timer);

            timer += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }

    public void Attack1()
    {
        charAnimController.CharacterAttack1Animation();
    }

    public void Attack2()
    {
        //float timer = 0;
        //if (gameObject.name == TagManager.LEADER_A_NAME_TAG)
        //{
        //Debug.Log("ES DIABLILLO!");
        //Tomo distancia para atacar
        //charAIController.CharacterPathfinding.FindPath(transform.position, new Vector3(transform.position.x + 5, transform.position.y, transform.position.z));
        //readyToMove = true;
        //Run(charAIController.CharacterPathfinding.finalPath);
        //readyToMove = false;

        ////Ataco
        //charAnimController.CharacterAttack2Animation();
        //timer += Time.deltaTime;
        //charAIController.LeaderFSM.Transition(TagManager.LEADER_FSM_IDLE_STATE_TAG);
        //if (timer > 1)
        //{
        ////    Debug.Log("PASÓ A sacar path original");
        //Vuelvo a sacar el path original
        //charAIController.CharacterPathfinding.FindPath(transform.position, charAIController.CharacterLineOfSight.Target.position);

        //}
        //readyToMove = false;
        //timer = 0;
        //}
        //else 
        charAnimController.CharacterAttack2Animation();
    }
    public void Attack3()
    {
        charAnimController.CharacterAttack3Animation();
    }

    public void Block1()
    {
        isBlocking = true;
        charAnimController.CharacterBlock1Animation();
    }

    public void Block2()
    {
        isBlocking = true;
        charAnimController.CharacterBlock2Animation();
    }

    private void OnDrawGizmos()
    {
        if (isBlocking)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}
