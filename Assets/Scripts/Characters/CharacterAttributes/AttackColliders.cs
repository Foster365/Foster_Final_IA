using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliders : MonoBehaviour
{
    public Entity attacker;
    public bool isKick, isPunch;

    public GameObject hitFX;

    public void Start()
    {
        attacker = GetComponentInParent<Entity>();

    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Attack Trigger");
        if (other.gameObject.GetComponent<Entity>())
        {
            if (isKick)
            {
                other.gameObject.GetComponent<Entity>().TakeDamage(attacker.kickDamage);
                //Debug.Log(attacker + "Kick Damage");
            }

            if (isPunch)
            {
                other.gameObject.GetComponent<Entity>().TakeDamage(attacker.punchDamage);
                //Debug.Log(attacker + "Punch Damage");
            }

        }

        gameObject.SetActive(false);

    }


}
