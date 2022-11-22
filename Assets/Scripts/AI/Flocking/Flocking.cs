using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking
{

    GameObject[] prefabsGrid;
    GameObject myFlockAgent;

    float avoidDistance, speed, neighbourDist, maxSpeed, rotationSpeed;
    Vector3 goalPosition;

    public Flocking(GameObject[] _prefabsGrid, GameObject _myFlockAgent, float _avoidDistance, float _neighbourDist
        , float _maxSpeed, float _rotationSpeed, Vector3 _goalPosition)
    {
        prefabsGrid = _prefabsGrid;
        myFlockAgent = _myFlockAgent;

        avoidDistance = _avoidDistance;
        neighbourDist = _neighbourDist;

        maxSpeed = _maxSpeed;
        rotationSpeed = _rotationSpeed;

        goalPosition = _goalPosition;
    }

    public float Speed { get => speed; set => speed = value; }

    public void ApplyFlockingRules()
    {
        GameObject[] neighbours;
        neighbours = prefabsGrid; //Guardo todos los agentes que considere vecinos (Si quiero agrandar o achicar
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
            if (neighbour != myFlockAgent) //Distinto para no compararme a mi mismo, sinò que para compararme con los demàs.
            {
                neighbourDistance = Vector3.Distance(neighbour.transform.position, myFlockAgent.transform.position); //Calculo mi distancia con el siguiente agente.

                if (neighbourDistance <= neighbourDist) //Si está en el rango de agregarlo al grupo.
                {
                    neighboursCenter += neighbour.transform.position; //Agrego la posición del agente que esté junto a mí en el grupo
                    groupSize++;

                    if (neighbourDistance <= avoidDistance)
                    {
                        avoidanceVector += myFlockAgent.transform.position + neighbour.transform.position; //Establezco un avoidance vector, para no
                                                                                                           //chocarme con el agente vecino.
                    }

                    Flocking anotherFlock = neighbour.GetComponent<Flocking>(); //Obtengo el flock component del agente comparado.
                    globalSpeed += anotherFlock.speed; //Establezco la velocidad global del grupo, acorde al vecino comparado.

                    if (anotherFlock.speed == 0) speed = 0;
                }

            }
        }

        if (groupSize > 0)
        {

            neighboursCenter = (neighboursCenter / groupSize) + (goalPosition - myFlockAgent.transform.position);
            speed = globalSpeed / groupSize;

            if (speed >= maxSpeed) speed = maxSpeed;

            Vector3 centerAvoidance = neighboursCenter + avoidanceVector;
            Vector3 direction = centerAvoidance - myFlockAgent.transform.position;

            if (direction != Vector3.zero)
                myFlockAgent.transform.rotation = Quaternion.Slerp(myFlockAgent.transform.rotation, Quaternion.LookRotation(direction), //Hago que lentamente los agentes vayan todos hacia
                    rotationSpeed * Time.deltaTime);                                              //La dirección en común que tienen (Para esto creé el rotationSpeed) 
        }

    }

}
