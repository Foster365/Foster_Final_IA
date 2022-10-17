using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Entity, IMove
{

    public bool dead;
    public float movementSpeed;
    bool _isMoving;

    LeaderAnimations _leaderAnimations;

    Transform _target;

    ////
    //[SerializeField]
    //float sbR;

    //[SerializeField]
    //float angle;

    public float attackRange;
    float currentAttackTime = 0;
    public float defaultAttackTime = 1.2f;

    bool attackTarget;

    //Waypoint System(Patrol) variables Esto cambia por malla de nodos
    public List<NodePathfinding> aStarWaypoints;
    public bool readyToMove;
    public float waypointDistance;
    int _nextWp = 0;
    int _indexModifier = 1;

    [SerializeField]
    LayerMask layer;

    Transform leaderTarget;
    //

    //SafePoints variables
    [SerializeField]
    LayerMask safePointLayer;
    Vector3 sPoint;
    //


    //Behaviour Tree Variables
    Roulette _roulette;
    Dictionary<BehaviourTreeNode, int> _rouletteNodes = new Dictionary<BehaviourTreeNode, int>();
    BehaviourTreeNode _initNode;
    //

    Transform _transform;

    LeaderSteering leaderSteering;

    //Pathfinding
    //public LayerMask aStarNodeMask;
    //public float distanceMax;
    //public float radius;
    //public Vector3 offset;
    //public GameObject finit;

    //AgentAStar _agentAstar;
    //

    //Seek seekBehaviour;
    //ObstacleAvoidance obsAvoidanceBehaviour;
    //Flee fleeBehaviour;
    LineOfSight lineOfSight;
    AgentAStar agentAStar;

    //public Seek SeekBehaviour { get => seekBehaviour; set => seekBehaviour = gameObject.GetComponent<Seek>(); }
    //public ObstacleAvoidance ObsAvoidanceBehaviour { get => obsAvoidanceBehaviour; set => obsAvoidanceBehaviour = gameObject.GetComponent<ObstacleAvoidance>(); }
    //public Flee FleeBehaviour { get => fleeBehaviour; set => fleeBehaviour = value; }
    public LineOfSight Line_Of_Sight { get => lineOfSight; set => lineOfSight = value; }
    public LayerMask Layer { get => layer; set => layer = value; }
    public float IdleCountdown { get => _idleCountdown; }
    //public AgentAStar AgentAStar { get => agentAStar; set => agentAStar = value; }

    //Seek Behaviour
    float _idleTimer;
    [SerializeField]
    float _idleCountdown;
    //

    void Start()
    {

        _transform = GetComponent<Transform>();

        _leaderAnimations = gameObject.GetComponent<LeaderAnimations>();

        leaderSteering = GetComponent<LeaderSteering>();

        //_target = GameObject.FindWithTag(CharacterTags.FOLLOWER_TAG).transform;//Estaba como Leader


        agentAStar = GetComponent<AgentAStar>();

        //seekBehaviour = GetComponent<Seek>();
        //obsAvoidanceBehaviour = GetComponent<ObstacleAvoidance>();
        //fleeBehaviour = GetComponent<Flee>();
        lineOfSight = GetComponent<LineOfSight>();

        currentAttackTime = defaultAttackTime;

        //agentAStar = new AgentAStar(transform, aStarNodeMask, distanceMax, radius, offset, SeekBehaviour, finit);

        //RouletteWheel();

        _idleTimer = 0;

        //_attackCollider = GetComponent<AttackColliders>();

    }
    public void Move(Vector3 dir)
    {

        dir.y = 0;
        transform.position += Time.deltaTime * dir * movementSpeed;

        //transform.forward = Vector3.Lerp(transform.forward, dir, movementSpeed * Time.deltaTime);
    }

    public void SetWayPoints(List<NodePathfinding> newPoints)
    {

        _nextWp = 0;

        if (newPoints.Count == 0) return;

        aStarWaypoints = newPoints;

        var pos = aStarWaypoints[_nextWp].transform.position;

        pos.y = transform.position.y;
        transform.position = pos;
        readyToMove = true;
    }

    public void Run()
    {

        var point = aStarWaypoints[_nextWp];//Var

        var posPoint = point.transform.position;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;

        if (dir.magnitude < waypointDistance/*0.2f*/)
        {

            if (_nextWp + _indexModifier < aStarWaypoints.Count-1)
            {

                _nextWp++;
                _leaderAnimations.MoveAnimation(true);

            }

            else
            {
                readyToMove = false;

                _leaderAnimations.MoveAnimation(false);
                return;
            }

        }

        Move(dir.normalized);

    }

    public Vector3 CalculateSafePoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, lineOfSight.ViewDistance, safePointLayer);
        if (colliders.Length > 1)
        {
            sPoint = colliders[0].transform.position;
        }
        return sPoint;
    }

    public Vector3 Target()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, lineOfSight.ViewDistance, Layer);
        if (targets.Length >= 1)
            leaderTarget = targets[0].transform;
        return leaderTarget.position;
    }

    public void Patrolling()
    {
        //_agentAstar.PathFindingAStarVector();
        //GoToWaypoint();
        _leaderAnimations.MoveAnimation(true);

    }

    public void Seek()
    {
        if (lineOfSight.Target != null)
        {
            leaderSteering.SbSeek.move = true;

            if (Vector3.Distance(transform.position, lineOfSight.Target.position) > attackRange)
            {
                leaderSteering.SbSeek.move = true;
                _leaderAnimations.SeekAnimation();
                Debug.Log("Seek Animation");
            }
            else
            {
                leaderSteering.SbSeek.move = false;
            }
            //combat.attack = true;
        }
    }

    public void Flee()
    {

        if (CurrentHealth < minimumLife)
        {
            leaderSteering.SbSeek.move = false;
            leaderSteering.SbFlee.GetDir();
            _leaderAnimations.SeekAnimation();
            Debug.Log("Flee Animation");
        }
        else
            return;

    }

}
