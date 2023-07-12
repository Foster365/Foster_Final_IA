//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Boss Damage State", menuName = "_main/States/Boss States/Boss Damage State", order = 0)]
//    public class BossDamageState : State
//    {
//        BossEnemyModel bossModel;
//        public override void EnterState(EntityModel model)
//        {
//            bossModel = model as BossEnemyModel;
//            bossModel.EnemyView.PlayWalkAnimation(false);
//            bossModel.GetRigidbody().velocity = Vector3.zero;
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Boss State Damage Execute");
//        }
//        public override void ExitState(EntityModel model)
//        {
//            bossModel.IsDamaged = false;
//        }
//    }
//}
