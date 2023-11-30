using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackCollider : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            Debug.Log("and can receive damage show layer health is" + LayerMask.LayerToName(other.gameObject.layer));
            if (other.gameObject.GetComponent<CharacterModel>() != null && other.gameObject.GetComponent<CharacterModel>().HealthController.CanReceiveDamage)
            {
                other.gameObject.GetComponent<CharacterModel>().HealthController.TakeDamage(damage);
                Debug.Log(other.gameObject.name + "Health is " + other.gameObject.GetComponent<CharacterModel>().HealthController.CurrentHealth);

                //DamageHandler(other.gameObject.GetComponent<CharacterModel>());
            }
            //isAutoDestroy = true;
        }

    }

    void CheckCollisionWTarget(Collider other)
    {

    }

}
