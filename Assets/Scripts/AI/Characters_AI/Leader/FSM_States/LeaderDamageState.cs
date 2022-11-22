using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderDamageState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;
    float timer;

    FSM<T> _fsm;
    T _attackState;
    T _idleState;
    T _deathState;

    public LeaderDamageState(LeaderAIController _leaderAIController, FSM<T> fsm,
    T attackState, T idleState, T deathState)
    {

        leaderAIController = _leaderAIController;

        _fsm = fsm;
        _attackState = attackState;
        _idleState = idleState;
        _deathState = deathState;
    }

    public override void Awake()
    {
        timer = 0;
        //leaderAIController.CharModel.CharacterHealthController.TakeDamage(leaderAIController.CharModel.CharacterHealthController.DamageTaken);
        leaderAIController.CharModel.CharAnimController.CharacterDamageAnimation();
        Debug.Log("Te cagaron a palos");
    }

    public override void Execute()
    {
        HealthController health = leaderAIController.CharModel.CharacterHealthController;
        timer += Time.deltaTime;
        Debug.Log("Leader Damage State Execute");
        //if (leaderAIController.CharModel.CharacterHealthController.IsDamage
        //    && leaderAIController.CharModel.CharacterHealthController.DamageTaken != 0)
        //{

        if (timer >= 1f)
        {
            _fsm.Transition(_idleState);
        }
        //leaderAIController.IdleAttackTransitionRouletteAction();
        //Corro otra ruleta _fsm.Transition(_idleState);
        //}
        else if (leaderAIController.CharModel.CharacterHealthController.IsDead
            ) _fsm.Transition(_deathState);
    }

    public override void Sleep()
    {
        timer = 0;
        leaderAIController.CharModel.CharacterHealthController.IsDamage = false;
    }

}
