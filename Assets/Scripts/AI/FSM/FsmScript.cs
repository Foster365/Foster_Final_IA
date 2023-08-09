
namespace _Main.Scripts.FSM_SO_VERSION
{
    public class FsmScript
    {
        private StateData _currentState;
        readonly EntityModel _entityModel;

        public FsmScript(EntityModel entityModel, StateData initStateData)
        {
            _entityModel = entityModel;
            _currentState = initStateData;

            _currentState.State.EnterState(_entityModel);
        }

        public void UpdateState()
        {
            _currentState.State.ExecuteState(_entityModel);
            CheckForConditions();
        }

        private void ChangeState(StateData nextState)
        {
            _currentState.State.ExitState(_entityModel);
            _currentState = nextState;
            _currentState.State.EnterState(_entityModel);
        }
        private void CheckForConditions()
        {
            for (int i = 0; i < _currentState.StateConditions.Length; i++)
            {
                if (_currentState.StateConditions[i].CompleteCondition(_entityModel))
                {
                    ChangeState(_currentState.ExitStates[i]);
                    break;
                }
            }
        }
    }
}