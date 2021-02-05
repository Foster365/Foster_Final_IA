using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderAttackState<T> : FSMState<T>
{

    Leader _leader;
    LeaderAnimations _leaderAnimations;

    Roulette _roulette;
    Dictionary<Node, int> _rouletteNodes = new Dictionary<Node, int>();
    Node _initNode;

    FSM<T> _fsm;
    T _seekState;
    T _hitState;
    T _dieState;
    T _fleeState;

    float currentAttackTime = 0;
    public float defaultAttackTime = 2f;

    public LeaderAttackState(Leader leader, LeaderAnimations leaderAnimations, FSM<T> fsm,
    T seekState , T hitState, T dieState, T fleeState)
    {
        _leader = leader;
        _leaderAnimations = leaderAnimations;

        _fsm = fsm;
        _seekState = seekState;
        _hitState = hitState;
        _dieState = dieState;
        _fleeState = fleeState;

    }

    public override void Awake()
    {
        Debug.Log("Leader AttackState Awake");
        _leaderAnimations.MoveAnimation(false);
        _leaderAnimations.SeekAnimation();
        CreateRoulette();
    }

    public override void Execute()
    {

        Debug.Log("Leader AttackState Execute");
        currentAttackTime += Time.deltaTime;

        if (currentAttackTime >= defaultAttackTime)
        {
            RouletteAction();

            currentAttackTime = 0;
        }

        if (Vector3.Distance(_leader.transform.position, _leader.Target()) >= _leader.attackRange)
            _fsm.Transition(_seekState);
        //else if ()
        //    _fsm.Transition(_blockStateEnemy);
        //else if (_target.TakeDamage(_target.punchDamage) || _target.TakeDamage(_target.kickDamage))
        //_fsm.Transition(_hitStateEnemy);
        else if (_leader.CurrentHealth <= 0)
            _fsm.Transition(_dieState);

    }

    public override void Sleep()
    {
        Debug.Log("Leader AttackState Sleep");
    }

    public void RouletteAction()
    {
        Node nodeRoulette = _roulette.Run(_rouletteNodes);
        nodeRoulette.Execute();
    }

    public void CreateRoulette()
    {
        Debug.Log("Ruleta Enemy Creada");
        _roulette = new Roulette();

        ActionNode aPunch = new ActionNode(_leaderAnimations.PunchAnimation);
        ActionNode Kick = new ActionNode(_leaderAnimations.KickAnimation);

        _rouletteNodes.Add(aPunch, 30);
        _rouletteNodes.Add(Kick, 50);

        ActionNode rouletteAction = new ActionNode(RouletteAction);

    }

}

