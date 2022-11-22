using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] float destroyGameObjectDelay;

    int damageTaken;
    bool isDamage, isDead;
    bool isLeader, isNPC;

    CharacterAnimationsController characterAnimController;

    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsLeader { get => isLeader; set => isLeader = value; }
    public bool IsNPC { get => isNPC; set => isNPC = value; }
    public bool IsDamage { get => isDamage; set => isDamage = value; }
    public int DamageTaken { get => damageTaken; }
    public int CurrentHealth { get => currentHealth; }
    public int MaxHealth { get => maxHealth; }

    private void Awake()
    {
        characterAnimController = GetComponent<CharacterAnimationsController>();
    }

    private void Update()
    {
        if (currentHealth <= 0) IsDead = true;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        isDamage = false;
        isDead = false;
        isLeader = false;
        isNPC = false;
        damageTaken = 0;//Los pongo por default.
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        isDamage = true;
        characterAnimController.CharacterDamageAnimation();
        if (currentHealth <= 0) isDead = true;
    }

    public void Die()
    {
        if (isDead)
        {
            characterAnimController.CharacterDeathAnimation();
            Destroy(gameObject, destroyGameObjectDelay);
        }
    }
}
