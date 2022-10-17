using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public float maxHealth = 100; float currentHealth;
    public float punchDamage = 5; public float kickDamage = 10; public float minimumLife = 10;
    public Rigidbody rb;

    public bool isLeader;
    float deathTime = 15f;

    Animator _anim;
    bool isDead;

    public float CurrentHealth { get => currentHealth; set => currentHealth = maxHealth; }

    public void Awake()
    {
        //CurrentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        //if (isLeader)
        //{
        //    _anim = GetComponentInChildren<Animator>();
        //}
        //else
        //{
        //    _anim = GetComponent<Animator>();
        //}
        

    }
    public void TakeDamage(float dm)
    {
        CurrentHealth -= dm;
        //AudioSource.PlayClipAtPoint(hitFX, transform.position);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
        else if (!isDead) { _anim.SetTrigger("Damaged"); }
    }

    public void Die()
    {
        
        if (!isDead)
        {
            Debug.Log("Se murió");
            _anim.SetTrigger("Death");
            //AudioSource.PlayClipAtPoint(deathFX, transform.position);
            Destroy(this.gameObject, deathTime);
        }

        isDead = true;

    }


}
