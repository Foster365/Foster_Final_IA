using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Data", menuName = "Data/Entity Data")]
public class EntityData : ScriptableObject
{
    //Basic Entity variables
    [field:SerializeField, Header("Basic Attributes")] public int MaxHealth { get; private set; }
    [field:SerializeField] public int HealthRegenerationAmount { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; set; }
    [field: SerializeField, Header("Attack Attributes")] public float AttackRange { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField, Header("NPC Attributes")] public string BossName { get; private set; }
    [field: SerializeField] public float FlockRadius { get; private set; }

}