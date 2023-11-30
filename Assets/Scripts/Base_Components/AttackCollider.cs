using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] LayerMask targetLayerMask;
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
        if (((1 << other.gameObject.layer) & targetLayerMask.value) != 0)
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
