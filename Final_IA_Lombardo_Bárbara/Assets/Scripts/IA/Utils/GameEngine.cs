using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameExample.Flocking.CheepCheep {
    public class GameEngine : MonoBehaviour
    {

        private static GameEngine _instance = null;
        public Transform _fishPrefab;
        public Transform _gameArea;
        public int _totalFlockEntities = 1;

        
		public static GameEngine Instance
		{
			get
			{
				return _instance;
			}
		}

        void Awake()
        {
           if (_instance)
			{
				Destroy(_instance.gameObject);
			}

			_instance = this;

            for (int i = 0; i < _totalFlockEntities; i++)
            {
                Transform fish = Instantiate(_fishPrefab, this.GetRandomPosition(), new Quaternion(), this.transform);                 
            }
        }

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        void OnDrawGizmosSelected()
        {
                       
        }

        public Vector3 GetRandomPosition() 
        {
            float width = _gameArea.localScale.x;
            float height = _gameArea.localScale.y;

            float x = Random.value * width - width/2;
            float y = Random.value * height - height/2;

            return new Vector3(x, y, 0);
        }
    }
}