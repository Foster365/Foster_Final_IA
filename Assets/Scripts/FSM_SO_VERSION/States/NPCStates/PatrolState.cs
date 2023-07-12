//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Assertions;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Patrol State", menuName = "_main/States/Enemy States/Patrol State", order = 0)]
//    public class PatrolState : State
//    {
//        private Dictionary<EntityModel, DataMovementState> _movementDatas = new Dictionary<EntityModel, DataMovementState>();
//        private class DataMovementState
//        {
//            public float Timer;
//            public EnemyModel EnemyModel;
//            public int PatrolCount;
//            public bool TravelBackwards;

//            public DataMovementState(EntityModel entityModel)
//            {
//                EnemyModel = (EnemyModel)entityModel;
//                Timer = EnemyModel.GetData().RestPatrolTime;
//                PatrolCount = 0;
//                TravelBackwards = false;
//            }
//        }

//        public override void EnterState(EntityModel model)
//        {

//            if (!_movementDatas.ContainsKey(model))
//            {
//                _movementDatas.Add(model, new DataMovementState(model));
//            }
//            _movementDatas[model].EnemyModel.EnemyView.PlayWalkAnimation(false);
//            model.IsPatrolling = true;
//        }

//        public override void ExecuteState(EntityModel model)
//        {
//            //Acá corro Astar.
//            var patrolPoints = _movementDatas[model].EnemyModel.GetPatrolPoints();

//            var distToNextPoint = Vector3.Distance(patrolPoints[_movementDatas[model].PatrolCount].transform.position, model.transform.position);
//            if (distToNextPoint == 0)
//            {
//                model.Move(Vector3.zero);
//            }
//            //Si estoy lejos del punto, me muevo hacia el
//            if (distToNextPoint > 1f)
//            {
//                var dirToNextPoint = (patrolPoints[_movementDatas[model].PatrolCount].transform.position - model.transform.position).normalized;

//                model.Move(dirToNextPoint);
//            }
//            else
//            {
//                //Si ya estoy cerca, me quedo quiero un tiempo X

//                model.GetRigidbody().velocity = Vector3.zero;
//                _movementDatas[model].Timer -= Time.deltaTime;

//                //Si ese tiempo X ya paso, chequeo si tengo que seguir el mismo recorrido
//                //O tengo que cambiar el sentido de la patrulla
//                if (_movementDatas[model].Timer <= 0 && !_movementDatas[model].TravelBackwards)
//                {
//                    _movementDatas[model].PatrolCount++;

//                    _movementDatas[model].Timer = _movementDatas[model].EnemyModel.GetData().RestPatrolTime;
//                }
//                else if (_movementDatas[model].Timer <= 0 && _movementDatas[model].TravelBackwards)
//                {

//                    _movementDatas[model].PatrolCount--;

//                    _movementDatas[model].Timer = _movementDatas[model].EnemyModel.GetData().RestPatrolTime;
//                }
//            }

//            //Chequeo si tengo que cambiar el sentido de la patrulla en base al count actual
//            //si ya llegue al final, cambio el sentido
//            if (_movementDatas[model].PatrolCount >= patrolPoints.Length - 1)
//            {
//                _movementDatas[model].TravelBackwards = true;

//            }
//            else if (_movementDatas[model].PatrolCount <= 0)
//            {
//                _movementDatas[model].TravelBackwards = false;
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {
//            _movementDatas[model].EnemyModel.IsPatrolling = false;
//            _movementDatas.Remove(model);
//        }
//    }


//}