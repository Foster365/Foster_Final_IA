using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMagicAttack : MonoBehaviour
{
    [SerializeField] float projectileSpeed, lifeTime;
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
        timer += Time.deltaTime;
        HandleAutoDestroy();
    }

    void HandleAutoDestroy()
    {
        if (timer >= lifeTime)
        {
            Debug.Log("Destroy por timer");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagManager.WALL_TAG)
        {
            Debug.Log("Destroy porque chocó contra una pared");
            Destroy(gameObject);
        }
    }

}
