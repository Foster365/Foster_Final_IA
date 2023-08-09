using UnityEngine;

namespace _Main.Scripts.FSM_SO_VERSION.LogicGates
{
    [CreateAssetMenu(fileName = "AndCondition", menuName = "_main/Conditions/Logic/OR")]
    public class OrCondition : StateCondition
    {
        [SerializeField] private StateCondition conditionOne;
        [SerializeField] private StateCondition conditionTwo;
        public override bool CompleteCondition(EntityModel model)
        {
            return conditionOne.CompleteCondition(model) || conditionTwo.CompleteCondition(model);
        }
    }
}