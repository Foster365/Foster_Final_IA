using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour
{
    public bool move = false;
    public float speed;
    public float rotSpeed;
    private LineOfSight sight;
    private Transform target;
    private Vector3 direction;

    public Transform Target { get => target; set => target = value; }

    private void Awake()
    {
        sight = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Target = sight.Target;

        if (move && Target != null)
        {
            //Consigo el vector entre el objetivo y mi posición, y luego lo niego para alejarme de él.
            Vector3 deltaVector = -(Target.transform.position - transform.position);
            deltaVector.y = 0;
            //Me guardo la dirección unicamente.
            direction = deltaVector.normalized;

            //Roto mi objeto hacia la dirección obtenida
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotSpeed);
            //Muevo mi objeto
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

}
