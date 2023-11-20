using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Data", menuName = "Data/Entity Data")]
public class EntityData : ScriptableObject
{
    //Basic Entity variables
    [field:SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int MovementSpeed { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float CooldownToAttack { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public string BossName { get; private set; }
    [field: SerializeField] public float FlockRadius { get; private set; }

}