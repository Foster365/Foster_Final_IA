using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController
{
    EntityModel model;

    int maxHealth, currentHealth, damage;

    bool isLeader, isNPC;

    #region Encapsulated variables

    public bool IsLeader { get => isLeader; set => isLeader = value; }
    public bool IsNPC { get => isNPC; set => isNPC = value; }
    public int Damage { get => damage; set => damage = value; }
    public int CurrentHealth { get => currentHealth; }
    #endregion

    public HealthController(EntityModel model)
    {
        this.model = model;
        currentHealth = maxHealth;
    }

    public bool IsEntityDead() => model.IsDead;
    public virtual void Die() { }
    public virtual void DoDamage(EntityModel affectedModel) { }
    
    public void GetDamage(int damage) { }
    public void Heal(int healingPoints) { }

}
