using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace _Main.Scripts.Entities
{
    public class HealthController2
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        bool isLeader, isNPC;
        bool isDamaged, isDead;

        #region Encapsulated variables

        public bool IsLeader { get => isLeader; set => isLeader = value; }
        public bool IsNPC { get => isNPC; set => isNPC = value; }
        public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
        public bool IsDead { get => isDead; set => isDead = value; }
        #endregion

        public event Action<float> OnHealthChange;
        public event Action OnDie;

        public HealthController2(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                OnDie?.Invoke();
            else
                OnHealthChange?.Invoke(CurrentHealth);

        }

        public void Heal(float heal)
        {
            CurrentHealth += heal;

            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            OnHealthChange?.Invoke(CurrentHealth);
        }
    }
}
