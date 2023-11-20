using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

namespace _Main.Scripts.Entities.Enemies
{
    public class BoidController : MonoBehaviour
    {
        [SerializeField] StateData initState;

        //private EnemyMinion _model;
        private FsmScript _enemyFsm;

        private void Update()
        {
            _enemyFsm.UpdateState();
        }
    }
}