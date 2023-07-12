//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using _Main.Scripts.Entities.Enemies.Data;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Attack State", menuName = "_main/States/Enemy States/Attack State", order = 0)]
//    public class AttackState : State
//    {

//        float attackMaxCooldown = 1.5f, attackCooldown;
//        private Dictionary<EntityModel, EnemyModel> _entitiesData = new Dictionary<EntityModel, EnemyModel>();
//        public override void EnterState(EntityModel model)
//        {
//            _entitiesData.Add(model, model as EnemyModel);
//            _entitiesData[model].EnemyView.PlayWalkAnimation(false);
//            _entitiesData[model].LineOfSight(_entitiesData[model].GetTarget().transform);
//            if (_entitiesData[model].IsSeeingTarget)
//            {
//                Debug.Log("Target is in sight, executing look dir");
//                Vector3 dir = _entitiesData[model].GetTarget().transform.position - _entitiesData[model].transform.position;
//                _entitiesData[model].LookDir(dir);
//            }
//            _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
//            _entitiesData[model].Controller.EnemyRoulette.EnemyAttackOrBlockRouletteAction();
//            attackCooldown = 0f;
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Enemy attack state execute");
//            attackCooldown += Time.deltaTime;
//            if (attackCooldown >= attackMaxCooldown)
//            {
//                _entitiesData[model].IsAttacking = false;
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {
//            _entitiesData.Remove(model);
//        }


//    }
//}