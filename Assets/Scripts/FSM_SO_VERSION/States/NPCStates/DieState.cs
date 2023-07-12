//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Die State", menuName = "_main/States/Enemy States/Die State", order = 0)]
//    public class DieState : State
//    {
//        private Dictionary<EntityModel, EnemyModel> _entitiesData = new Dictionary<EntityModel, EnemyModel>();
//        public override void EnterState(EntityModel model)
//        {
//            _entitiesData.Add(model, model as EnemyModel);
//            _entitiesData[model].IsDead = true;
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            _entitiesData[model].Die();
//        }

//        public override void ExitState(EntityModel model)
//        {
//            _entitiesData[model].IsDead = false;
//            _entitiesData.Remove(model);
//        }
//    }
//}