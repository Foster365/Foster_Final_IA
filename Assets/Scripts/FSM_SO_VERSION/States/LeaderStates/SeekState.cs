
//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using _Main.Scripts.Roulette_Wheel.EntitiesRouletteWheel;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.BossStates
//{
//    [CreateAssetMenu(fileName = "Seek State", menuName = "_main/States/Boss States/Seek State", order = 0)]
//    public class SeekState : State
//    {
//        BossEnemyModel bossModel;

//        public override void EnterState(EntityModel model)
//        {
//            bossModel = model as BossEnemyModel;
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Boss State Seek Execute");

//            var dir = bossModel.Controller.SbSeek.GetDir();
//            if (dir != Vector3.zero)
//            {
//                Debug.Log("Enemy puede moverse");
//                bossModel.Move(dir);
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {
//        }
//    }
//}
