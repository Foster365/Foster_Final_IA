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
            //Debug.Log(gameObject.name + "Collision with: " + other.gameObject.name);
            if (other.gameObject.GetComponent<CharacterModel>() != null && other.gameObject.GetComponent<CharacterModel>().HealthController.CanReceiveDamage)
            {
                other.gameObject.GetComponent<CharacterModel>().HealthController.TakeDamage(damage);

                //DamageHandler(other.gameObject.GetComponent<CharacterModel>());
            }
            //isAutoDestroy = true;
        }

    }

    void CheckCollisionWTarget(Collider other)
    {
        
    }

    void DamageHandler(CharacterModel model)
    {
        if (model.HealthController.CanReceiveDamage)
        {
            model.HealthController.TakeDamage(damage);
            Debug.Log(model.gameObject.name + "Health is " + model.HealthController.CurrentHealth);
        }
    }

    void CheckDeath(CharacterModel model)
    {
        if (model.HealthController.IsDead)
            Destroy(model.gameObject);
    }
}
