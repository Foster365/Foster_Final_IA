//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Damage State", menuName = "_main/States/Enemy States/Damage State", order = 0)]
//    public class EnemyDamageState : State
//    {
//        private Dictionary<EntityModel, EnemyModel> _entitiesData = new Dictionary<EntityModel, EnemyModel>();

//        public override void EnterState(EntityModel model)
//        {
//            _entitiesData.Add(model, model as EnemyModel);
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Enemy damage state execute");
//        }
//        public override void ExitState(EntityModel model)
//        {
//            _entitiesData[model].IsDamaged = false;
//            _entitiesData.Remove(model);
//        }
//    }
//}
