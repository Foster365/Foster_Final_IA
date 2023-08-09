//using _Main.Scripts.Entities;
//using _Main.Scripts.Entities.Enemies;
//using _Main.Scripts.Roulette_Wheel;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Main.Scripts.FSM_SO_VERSION.States.EnemyStates
//{
//    [CreateAssetMenu(fileName = "Search State", menuName = "_main/States/Enemy States/Search State", order = 0)]
//    public class SearchState : State
//    {
//        private class SearchData
//        {
//            public EnemyModel Model;
//            public Vector3 Dir;
//            public float Timer;

//        }

//        private Dictionary<EntityModel, SearchData> _searchDatas = new Dictionary<EntityModel, SearchData>();
//        public override void EnterState(EntityModel model)
//        {
//            _searchDatas.Add(model, new SearchData());
//            _searchDatas[model].Model = (EnemyModel)model;
//            _searchDatas[model].Model.EnemyView.PlayWalkAnimation(false);
//            var myModel = _searchDatas[model].Model;
//            Vector3 lastViewDir = myModel.GetLastViewDir();


//            Dictionary<Vector3, int> dirChances = new Dictionary<Vector3, int>();
//            Vector3 opositeDir = Quaternion.AngleAxis(90f, Vector3.one) * lastViewDir;

//            //En base a la ultima direccion en la que vi al jugador
//            //guardo 4 posibilidades:

//            //La verdadera ultima direccion
//            dirChances.Add(lastViewDir, 50);

//            //Y 3 mas que son opuestas o perpendiculares
//            dirChances.Add(lastViewDir * -1, 20);
//            dirChances.Add(opositeDir, 20);
//            dirChances.Add(opositeDir * -1, 20);

//            var roulette = new Roulette();
//            //Metemos todas las direcciones en una ruleta y la activamos
//            _searchDatas[model].Dir = roulette.Run(dirChances);

//            _searchDatas[model].Timer = myModel.GetData().TimeForSearchPlayer;


//            _searchDatas[model].Model.questionSing.SetActive(true);
//            myModel.IsSearching = true;
//        }



//        public override void ExecuteState(EntityModel model)
//        {
//            Debug.Log("Enemy search state execute");
//            _searchDatas[model].Timer -= Time.deltaTime;
//            if (_searchDatas[model].Timer > 0)
//            {
//                _searchDatas[model].Model.Move(_searchDatas[model].Dir);
//            }
//            else
//            {
//                _searchDatas[model].Model.IsAllert = false;
//            }
//        }

//        public override void ExitState(EntityModel model)
//        {

//            _searchDatas[model].Model.questionSing.SetActive(false);
//            _searchDatas.Remove(model);
//            model.IsSearching = false;
//        }
//    }
//}