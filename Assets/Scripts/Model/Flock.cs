using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    [SerializeField] FlockManager myManager;
    float speed;
    bool turning = false;

    public FlockManager MyManager { get => myManager; set => myManager = value; }

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.MinSpeed, myManager.MaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        Bounds b = new Bounds(myManager.transform.position, myManager.SwimLimits * 2);
        RaycastHit hit;
        Vector3 direction = Vector3.zero;

        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        if (Physics.Raycast(transform.position, transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
        }
        else turning = false;

        if(turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                myManager.RotationSpeed * Time.deltaTime);
        }
        else
        {

            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.MinSpeed, myManager.MaxSpeed);

            if (Random.Range(0, 100) < 20)
                ApplyFlockingRules();

        }

        transform.Translate(0, 0, Time.deltaTime * speed);

    }

    void ApplyFlockingRules()
    {
        GameObject[] neighbours;
        neighbours = myManager.AllFish; //Guardo todos los agentes que considere vecinos (Si quiero agrandar o achicar
        //la cantidad de vecinos puedo establecer un rango y overlapspherear desde mi agente hasta el rango deseado,
        //e ir almacenando todos los agentes que se encuentren dentro de ese rango
        Vector3 neighboursCenter = Vector3.zero; //Centro de los vecinos.
        Vector3 avoidanceVector = Vector3.zero; //Dist avoidance entre agentes.

        float globalSpeed = .01f; //Average speed en la que se mueve el grupo
        float neighbourDistance; //Distancia vecina entre agentes
        int groupSize = 0; // Representa la cantidad de agentes pertenecientes al grupo dentro del rebaño, la sección más
        //pequeña del rebaño/bandada

        foreach (GameObject neighbour in neighbours)
        {
            if (neighbour != gameObject)
            {
                neighbourDistance = Vector3.Distance(neighbour.transform.position, transform.position);

                if(neighbourDistance <= myManager.NeighbourDist)
                {
                    neighboursCenter += neighbour.transform.position;
                    groupSize++;

                    if(neighbourDistance < 1.0f)
                    {
                        avoidanceVector = avoidanceVector + (transform.position - neighbour.transform.position);
                    }

                    Flock anotherFlock = neighbour.GetComponent<Flock>();
                    globalSpeed = globalSpeed + anotherFlock.speed;

                }

            }
        }

        if(groupSize > 0)
        {
            neighboursCenter = neighboursCenter / groupSize + (myManager.GoalPosition - transform.position);
            speed = globalSpeed / groupSize;

            Vector3 direction = (neighboursCenter + avoidanceVector) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                    myManager.RotationSpeed * Time.deltaTime);
        }

    }
}
