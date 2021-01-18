using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Entity
{
    public float movementSpeed;
    bool _isMoving;

    Transform _target;

    public float attackRange;
    float currentAttackTime = 0;
    public float defaultAttackTime = 1.2f;

    bool attackTarget;

    //public Player _player;

    //public AttackColliders _attackCollider;

    //Waypoint System (Patrol) variables
    public List<Transform> Waypoints;
    public float distance;
    int _nextWp = 0;
    int _indexModifier = 1;
    //

    //Line of Sight Variables
    [SerializeField]
    float LOSRadius, angle;

    [SerializeField]
    LayerMask layer;
    //

    Roulette _roulette;
    Dictionary<Node, int> _rouletteNodes = new Dictionary<Node, int>();
    Node _initNode;

    Transform _transform;

    private void Start()
    {

        _transform = GetComponent<Transform>();
        //_enemyAnim = GetComponent<EnemyAnimations>();


        _target = GameObject.FindWithTag(CharacterTags.FOLLOWER_TAG).transform;

        currentAttackTime = defaultAttackTime;

        RouletteWheel();
        //_player = GetComponent<Player>();

        //_attackCollider = GetComponent<AttackColliders>();
    }

    public void Move(Vector3 dir)
    {

        dir.y = 0;
        rb.velocity = dir * movementSpeed;
        transform.forward = Vector3.Lerp(transform.forward, dir, 0.2f);
        // _enemyAnimation.RunAnimation();
        _isMoving = true;

    }

    public void Attack()
    {
        currentAttackTime += Time.deltaTime;

        if (currentAttackTime >= defaultAttackTime)
        {
            RouletteAction();
            currentAttackTime = 0;
        }




        //Damage();
        // Debug.Log("Punch Anim");
    }

    //public void Damage()
    //{

    //    if (_attackCollider.isKick)
    //    {
    //        TakeDamage(kickDamage);
    //        Debug.Log(_attackCollider.isKick + "Damage applied");
    //    }

    //    if (_attackCollider.isPunch)
    //    {
    //        TakeDamage(punchDamage);
    //        Debug.Log(_attackCollider.isPunch + "Damage applied");
    //    }


    //}

    public void RouletteWheel()
    {
        _roulette = new Roulette();

        //ActionNode aPunch = new ActionNode(APunch);
        //ActionNode bPunch = new ActionNode(BPunch);
        //ActionNode kick = new ActionNode(Kick);

        //_rouletteNodes.Add(aPunch, 30);
        //_rouletteNodes.Add(bPunch, 35);
        //_rouletteNodes.Add(kick, 50);

        ActionNode rouletteAction = new ActionNode(RouletteAction);
    }

    public void RouletteAction()
    {
        // Debug.Log("Entered in roulette");
        Node nodeRoulette = _roulette.Run(_rouletteNodes);
        nodeRoulette.Execute();
    }

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
        // return dir;

    }

    //public bool LineOfSight(Transform target)
    //{

    //    Vector3 diff = transform.position - target.transform.position;
    //    float distance = diff.magnitude;
    //    if (distance > LOSRadius) return false;
    //    float angleToTarget = Vector3.Angle(transform.position, diff.normalized);
    //    if (angleToTarget > angle / 2) return false;
    //    if (Physics.Raycast(transform.position, diff, distance, layer)) return true;

    //    return true;

    //}

}
