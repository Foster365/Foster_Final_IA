using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] int damage;
    CharacterModel attacker;
    bool isAutoDestroy;

    public bool IsAutoDestroy { get => isAutoDestroy; set => isAutoDestroy = value; }

    //[SerializeField] GameObject hitFX;

    public void Start()
    {
        attacker = GetComponentInParent<CharacterModel>();
        isAutoDestroy = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HealthController>() && gameObject != null)
        {
            other.gameObject.GetComponent<HealthController>().IsDamage = true;
            other.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            //other.gameObject.GetComponent<HealthController>().DamageTaken = 10;
            //other.gameObject.GetComponent<HealthController>().TakeDamage(10);
            //isAutoDestroy = true;
        }

        //gameObject.SetActive(false);

    }
}
