using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMagicAttack : MonoBehaviour
{
    [SerializeField] float projectileSpeed, lifeTime;
    [SerializeField] int damage;
    [SerializeField] LayerMask targetLayerMask;
    float timer;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        rb.AddForce(-transform.right * projectileSpeed);
    }

    private void Update()
    {
        if(gameObject != null)
        {
            timer += Time.deltaTime;
            HandleAutoDestroy();
        }
    }

    void HandleAutoDestroy()
    {
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null) CheckCollisionWTarget(other);
    }
    void CheckCollisionWTarget(Collider other)
    {
        if ((targetLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.gameObject.GetComponent<CharacterModel>() != null)
            {
                DamageHandler(other.gameObject.GetComponent<CharacterModel>());
            }
        }
        else Destroy(gameObject);
    }

    void DamageHandler(CharacterModel model)
    {
        if (model.HealthController.CanReceiveDamage)
        {
            model.HealthController.TakeDamage(damage);
        }
    }

}
