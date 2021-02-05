using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderController : MonoBehaviour
{

    Leader _leader;
    LeaderAnimations _leaderAnimations;


    //FSM Variables
    FSM<string> _fsm;
    Rigidbody _rigidbody;
    //

    //LineOfSight variables
    //

    private void Awake()
    {
        _leader = GetComponent<Leader>();
        _leaderAnimations = GetComponent<LeaderAnimations>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {

        _fsm = new FSM<string>();

        LeaderIdleState<string> leaderIdleState = new LeaderIdleState<string>(_leader, _leaderAnimations, _fsm, "LeaderPatrolState", "LeaderDamagedState", "LeaderDeathState", "LeaderFleeState", _leader.IdleCountdown);
        LeaderPatrolState<string> leaderPatrolState = new LeaderPatrolState<string>(_leader, _leaderAnimations, _fsm, "LeaderIdleState", "LeaderSeekState");
        LeaderSeekState<string> leaderSeekState = new LeaderSeekState<string>(_leader, _leaderAnimations, _fsm, "LeaderPatrolState", "LeaderAttackState");
        LeaderAttackState<string> leaderAttackState = new LeaderAttackState<string>(_leader, _leaderAnimations, _fsm, "LeaderSeekState", "LeaderFleeState", "LeaderDamagedState", "LeaderDeathState");
        LeaderDamagedState<string> leaderDamagedState = new LeaderDamagedState<string>(_leader, _leaderAnimations, _fsm, "LeaderAttackState", "LeaderIdleState", "LeaderFleeState");
        LeaderFleeState<string> leaderFleeState = new LeaderFleeState<string>(_leader, _leaderAnimations, _fsm, "LeaderSeekState");
        LeaderDeathState<string> leaderDeathState = new LeaderDeathState<string>(_leaderAnimations);

        leaderIdleState.AddTransition("LeaderPatrolState", leaderPatrolState);
        leaderIdleState.AddTransition("LeaderHitState", leaderDamagedState);
        leaderIdleState.AddTransition("LeaderDeathState", leaderDeathState);
        leaderPatrolState.AddTransition("LeaderIdleState", leaderIdleState);
        leaderPatrolState.AddTransition("LeaderSeekState", leaderSeekState);
        leaderSeekState.AddTransition("LeaderPatrolState", leaderPatrolState);
        leaderSeekState.AddTransition("LeaderAttackState", leaderAttackState);
        leaderAttackState.AddTransition("LeaderSeekState", leaderSeekState);
        leaderAttackState.AddTransition("LeaderDamagedState", leaderDamagedState);
        leaderAttackState.AddTransition("LeaderDeathState", leaderDeathState);
        leaderDamagedState.AddTransition("LeaderDamagedState", leaderDamagedState);
        leaderDamagedState.AddTransition("LeaderIdleState", leaderIdleState);
        leaderFleeState.AddTransition("LeaderSeekState", leaderSeekState);
        _fsm.SetInit(leaderIdleState);

    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();
    }

    //public Transform GetTarget()
    //{
    //    Transform target = null;

    //    Collider[] colliders = Physics.OverlapSphere(transform.position, _leader.attackRange, _leader.Layer);
    //    if (colliders.Length > 0)
    //        target = colliders[0].transform;

    //    //Vector3 targetDir = (target.position - transform.position).normalized;

    //    Vector3 targetDir = (target.position - transform.position).normalized;
    //    Vector3 currTargetDir;

    //    for (var i = 0; i < colliders.Length; i++)
    //    {
    //        if(targetDir)
    //    }

    //}
}
