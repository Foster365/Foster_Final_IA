//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.BossStates
//{
//    [CreateAssetMenu(fileName = "Enhanced Atack State", menuName = "_main/States/Boss States/Enhanced Atack State", order = 0)]
//    public class EnhancedAttack : State
//    {
//        //Ruleta con pesos dinamicos
//        BossEnemyModel bossModel;
//        float timer = 0f, attackCooldownTimer = 0f, attackMaxCooldownTimer = 1f;
//        public override void EnterState(EntityModel model)
//        {
//            bossModel = model as BossEnemyModel;
//            timer = 0;
//        }
//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Boss State Enhanced Execute");
//            if (!bossModel.GetData().IsAttackDone)
//            {
//                timer += Time.deltaTime;
//                if (timer <= bossModel.GetData().AttackStateTimer)
//                {
//                    bossModel.Controller.BossEnemyRoulette.EnemyEnhancedAttacksRouletteAction();
//                    attackCooldownTimer += Time.deltaTime;
//                    if (attackCooldownTimer >= attackMaxCooldownTimer) attackCooldownTimer = 0;
//                }
//                else bossModel.GetData().IsAttackDone = true;
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {
//        }
//    }
//}
