using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] LayerMask targetLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayerMask.value) != 0)
        {
            if (other.gameObject.GetComponent<CharacterModel>() != null && other.gameObject.GetComponent<CharacterModel>().HealthController.CanReceiveDamage)
            {
                other.gameObject.GetComponent<CharacterModel>().HealthController.TakeDamage(damage);
            }
        }

    }

}
