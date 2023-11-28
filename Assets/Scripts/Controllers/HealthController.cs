using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HealthController
{
    EntityModel model;

    int maxHealth, currentHealth, damage;

    bool isLeader, isNPC;
    bool isDamaged, canReceiveDamage, isDead;

    public event Action<float> OnHealthChange;
    public event Action OnDie;

    #region Encapsulated variables

    public bool IsLeader { get => isLeader; set => isLeader = value; }
    public bool IsNPC { get => isNPC; set => isNPC = value; }
    public int Damage { get => damage; set => damage = value; }
    public int CurrentHealth { get => currentHealth; }
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
    public bool CanReceiveDamage { get => canReceiveDamage; set => canReceiveDamage = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public int MaxHealth { get => maxHealth; }
    #endregion

    public HealthController(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        else
            OnHealthChange?.Invoke(currentHealth);

    }

    //public void Die()
    //{
    //    OnDie?.Invoke();
    //}

    public void Heal(int heal)
    {
        currentHealth += heal;

        if (currentHealth >= MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        OnHealthChange?.Invoke(currentHealth);
    }
}
