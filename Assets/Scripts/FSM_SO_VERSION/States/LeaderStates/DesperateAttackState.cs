//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using _Main.Scripts.FSM_SO_VERSION.Conditions.BossConditions;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using Unity.VisualScripting;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.BossStates
//{
//    [CreateAssetMenu(fileName = "Desperate Atack State", menuName = "_main/States/Boss States/Desperate Atack State", order = 0)]
//    public class DesperateAttackState : State
//    {
//        BossEnemyModel bossModel;
//        float timer = 0, rouletteMaxCooldown = .75f, rouletteCooldownTimer = 0;
//        public override void EnterState(EntityModel model)
//        {
//            bossModel = model as BossEnemyModel;
//            bossModel.LineOfSight(bossModel.GetTarget().transform);
//            bossModel.EnemyView.PlayDesperateAttackActivator();
//            if (bossModel.IsSeeingTarget)
//            {
//                Debug.Log("Target is in sight, executing look dir");
//                Vector3 dir = bossModel.GetTarget().transform.position - bossModel.transform.position;
//                bossModel.LookDir(dir);
//            }
//            bossModel.EnemyView.PlayWalkAnimation(false);
//            bossModel.GetRigidbody().velocity = Vector3.zero;
//            bossModel.GetData().IsInvulnerable = false;
//        }
//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Boss State Desperate Execute");
//            rouletteCooldownTimer += Time.deltaTime;

//            if (rouletteCooldownTimer >= rouletteMaxCooldown)
//            {
//                Debug.Log("Boss state enemy is doing desperate attacka");
//                bossModel.Controller.BossEnemyRoulette.EnemyDesperateAttacksRouletteAction();
//                rouletteCooldownTimer = 0;
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {
//        }
//    }
//}
