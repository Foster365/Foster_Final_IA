using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] GameObject fishPrefab;
    [SerializeField] int fishNum = 20;
    [SerializeField] GameObject[] allFish;
    [SerializeField] Vector3 swimLimits = new Vector3(5, 5, 5);
    [SerializeField] Vector3 goalPosition;

    [Header("Fish settings")]
    [Range(.0f, 5.0f)]
    [SerializeField] float minSpeed;

    [Range(.0f, 5.0f)]
    [SerializeField] float maxSpeed;

    [Range(1.0f, 10.0f)]
    [SerializeField] float neighbourDist;

    [Range(.0f, 5.0f)]
    [SerializeField] float rotationSpeed;

    public float MinSpeed { get => minSpeed; set => minSpeed = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float NeighbourDist { get => neighbourDist; set => neighbourDist = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public GameObject[] AllFish { get => allFish; set => allFish = value; }
    public Vector3 GoalPosition { get => goalPosition; set => goalPosition = value; }
    public Vector3 SwimLimits { get => swimLimits; set => swimLimits = value; }

    void Start()
    {
        allFish = new GameObject[fishNum];
        for (var i = 0; i < allFish.Length; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().MyManager  = this;
        }
        goalPosition = transform.position;
    }

    void Update()
    {
        //IMPORTANTE: Si mi personaje necesita tener colliders hay que manejarlo con distintas capas (Layers) de colliders,
        //en principio el personaje no debería tener colliders, porque los tienen los pilares y no queremos que se mezclen
        //(Chequear esto con proyecto de obs avoidance de clase)
        if (Random.Range(0, 100) < 10)
            goalPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                    Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
    }

}
