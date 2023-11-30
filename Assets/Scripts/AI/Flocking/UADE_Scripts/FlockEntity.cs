using System.Collections;
using System.Collections.Generic;
using UADE.IA.Flocking.States;
using UnityEngine;

public class FlockEntity : MonoBehaviour
{

    [SerializeField] float neighbourRadius = 4, minFlockDist, flockStayInRadiusRd;

    private Vector3 direction;

    public Vector3 Direction
    {
        get
        {
            return direction;
        }
    }

    public float MinFlockDist { get => minFlockDist; set => minFlockDist = value; }

    public Collider mainCollider;

    FlockBehavior[] steeringBehaviours;
    List<Transform> context;


    AlignmentBehavior flockAlignmentSB;
    AvoidanceBehavior flockAvoidanceSB;
    CohesionBehavior flockCohesionSB;
    StayInRadiusBehavior flockStayInRadiusSB;

    public LayerMask flockGetNeighboursLayerMask;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        context = new List<Transform>();

        InitFlockSB();
    }

    void InitFlockSB()
    {

        steeringBehaviours = new FlockBehavior[4];
        flockAlignmentSB = new AlignmentBehavior(gameObject);
        flockAvoidanceSB = new AvoidanceBehavior(gameObject, minFlockDist);
        flockCohesionSB = new CohesionBehavior(gameObject);
        flockStayInRadiusSB = new StayInRadiusBehavior(gameObject, flockStayInRadiusRd);

        steeringBehaviours[0] = flockAlignmentSB;
        steeringBehaviours[1] = flockAvoidanceSB;
        steeringBehaviours[2] = flockCohesionSB;
        steeringBehaviours[3] = flockStayInRadiusSB;

    }

    public Vector3 UpdateDirection()
    {

        Vector3 direction = Vector3.zero;

        List<Transform> neighbours = this.GetNearbyEntities();

        for (int i = 0; i < steeringBehaviours.Length; i++)
        {
            FlockBehavior behavior = steeringBehaviours[i];

            direction += behavior.CalculateDirection(neighbours);
        }

        this.direction = direction;

        return this.direction;
    }


    public List<Transform> GetNearbyEntities()
    {

        Collider[] contextColliders = Physics.OverlapSphere(this.transform.position, neighbourRadius, flockGetNeighboursLayerMask);

        foreach (Collider collider in contextColliders)
        {
            if (collider != mainCollider)
            {
                context.Add(collider.transform);
            }
        }

        return context;
    }
}
