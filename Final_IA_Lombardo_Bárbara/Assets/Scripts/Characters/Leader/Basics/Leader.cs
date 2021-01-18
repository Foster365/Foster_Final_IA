using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.Steering;

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
    public List<Transform> Waypoints;
    public float distance;
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

    Roulette _roulette;
    Dictionary<Node, int> _rouletteNodes = new Dictionary<Node, int>();
    Node _initNode;

    Transform _transform;

    //Pathfinding
    public Entity characterAStar;
    public LayerMask mask;
    public float distanceMax;
    public float radius;
    public Vector3 offset;
    public GameObject finit;

    AgentAStar _agentAstar;
    //

    Seek seekBehaviour;
    ObstacleAvoidance obsAvoidanceBehaviour;
    Flee fleeBehaviour;
    LineOfSight lineOfSight;

    public Seek SeekBehaviour { get => seekBehaviour; set => seekBehaviour = gameObject.GetComponent<Seek>(); }
    public ObstacleAvoidance ObsAvoidanceBehaviour { get => obsAvoidanceBehaviour; set => obsAvoidanceBehaviour = gameObject.GetComponent<ObstacleAvoidance>(); }
    public Flee FleeBehaviour { get => fleeBehaviour; set => fleeBehaviour = value; }
    public LineOfSight Line_Of_Sight { get => lineOfSight; }
    public LayerMask Layer { get => layer; set => layer = value; }

    //Seek Behaviour
    float _idleTimer;
    [SerializeField]
    float _idleCountdown;
    //

    void Start()
    {

        _transform = GetComponent<Transform>();

        _leaderAnimations = gameObject.GetComponent<LeaderAnimations>();

        _target = GameObject.FindWithTag(CharacterTags.FOLLOWER_TAG).transform;//Estaba como Leader

        _agentAstar = new AgentAStar(characterAStar, mask, distanceMax, radius, offset, seekBehaviour, finit);

        //seekBehaviour = GetComponent<Seek>();
        //obsAvoidanceBehaviour = GetComponent<ObstacleAvoidance>();
        //fleeBehaviour = GetComponent<Flee>();
        //lineOfSight = GetComponent<LineOfSight>();

        currentAttackTime = defaultAttackTime;

        //RouletteWheel();

        _idleTimer = 0;

        //_attackCollider = GetComponent<AttackColliders>();

    }

    public void Move(Vector3 dir)
    {

        dir.y = 0;
        rb.velocity = dir * movementSpeed;
        transform.forward = Vector3.Lerp(transform.forward, dir, 0.2f);
        _isMoving = true;

    }

    //public void Attack()
    //{
    //    currentAttackTime += Time.deltaTime;

    //if (currentAttackTime >= defaultAttackTime)
    //    RouletteAction();

    //    currentAttackTime = 0;

    //    //Damage();
    //    Debug.Log("Punch Anim");
    //}

    //public void RouletteWheel()
    //{
    //    _roulette = new Roulette();

    //    //ActionNode aPunch = new ActionNode(APunch);
    //    //ActionNode bPunch = new ActionNode(BPunch);
    //    //ActionNode kick = new ActionNode(Kick);

    //    //_rouletteNodes.Add(aPunch, 30);
    //    //_rouletteNodes.Add(bPunch, 35);
    //    //_rouletteNodes.Add(kick, 50);

    //    ActionNode rouletteAction = new ActionNode(RouletteAction);
    //}

    //public void RouletteAction()
    //{
    //    Debug.Log("Entered in roulette");
    //    Node nodeRoulette = _roulette.Run(_rouletteNodes);
    //    nodeRoulette.Execute();
    //}

    public void GoToWaypoint()
    {

        var waypoint = Waypoints[_nextWp];
        var waypointPosition = waypoint.position;
        waypointPosition.y = transform.position.y;
        Vector3 dir = waypointPosition - transform.position;
        if (dir.magnitude < distance)
        {
            if (_nextWp + _indexModifier >= Waypoints.Count || _nextWp + _indexModifier < 0)
                _indexModifier *= -1;
            _nextWp += _indexModifier;
        }
        Move(dir.normalized);
        //Move(obsAvoidanceBehaviour.GetDir().normalized);//Testear este movimiento
        // return dir;

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
        _agentAstar.PathFindingAStarVector();
        //GoToWaypoint();
        _leaderAnimations.MoveAnimation(true);

    }

    public void Seek()
    {
        if (lineOfSight.Target != null)
        {
            seekBehaviour.move = true;

            if (Vector3.Distance(transform.position, lineOfSight.Target.position) > attackRange)
            {
                seekBehaviour.move = true;
                _leaderAnimations.SeekAnimation();
                Debug.Log("Seek Animation");
            }
            else
            {
                seekBehaviour.move = false;
            }
            //combat.attack = true;
        }
    }

    public void Flee()
    {

        if (CurrentHealth < minimumLife)
        {
            seekBehaviour.move = false;
            fleeBehaviour.Move();
            _leaderAnimations.SeekAnimation();
            Debug.Log("Flee Animation");
        }
        else
            return;

    }

    public void Punch()
    {
        _leaderAnimations.PunchAnimation();
    }

    public void Kick()
    {
        _leaderAnimations.KickAnimation();
    }

    public void Block()
    {
        _leaderAnimations.BlockAnimation();
    }

    public void GetDamage()
    {
        _leaderAnimations.DamageAnimation();
    }

    public void Idle()
    {
        _leaderAnimations.IdleAnimation();
    }

    public void Died()
    {
        _leaderAnimations.DeathAnimation();
    }

}
