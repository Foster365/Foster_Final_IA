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
        if(other.gameObject != null) CheckCollisionWTarget(other);

    }

    void CheckCollisionWTarget(Collider other)
    {
        if ((targetLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Collision with: " + other.gameObject.name);
            other.gameObject.GetComponent<CharacterModel>().HealthController.TakeDamage(damage);
            Debug.Log(other.name + "Health is " + other.gameObject.GetComponent<CharacterModel>().HealthController.CurrentHealth);
            //isAutoDestroy = true;
        }
    }
}
