using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSteering : MonoBehaviour
{

    Leader leader;
    LeaderAnimations leaderAnimations;
    LeaderController leaderController;

    //Constructor variables
    [SerializeField]
    Transform sbTarget;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Rigidbody sbRigidbodyTarget;

    [SerializeField]
    float sbTimePrediction;
    [SerializeField]
    float sbSpeed;
    [SerializeField]
    float sbRotationSpeed;
    [SerializeField]
    float sbRadius;
    [SerializeField]
    float sbAvoidWeight;
    [SerializeField] float sbTime;

    [SerializeField]
    List<Vector3> sbWaypoints;

    [SerializeField]
    LayerMask sbLayerMask;

    [SerializeField]
    Vector3 sbDirection;

    [SerializeField]
    bool sbMove;

    [SerializeField]
    LineOfSight lineOfSight;
    [SerializeField]
    AgentAStar agentAstar;
    //

    ISteeringBehaviour sb;

    Evade sbEvade;
    Flee sbFlee;
    ObstacleAvoidance sbObstacleAvoidance;
    Pursuit sbPursuit;
    // Seek sbSeek;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {

        sbEvade = new Evade(transform, sbTarget, rb, sbTimePrediction);
        sbFlee = new Flee(sbMove, sbSpeed, sbRotationSpeed, transform, lineOfSight, sbTarget, sbDirection);
        sbObstacleAvoidance = new ObstacleAvoidance(transform, sbRadius, sbAvoidWeight, sbLayerMask);
        sbPursuit = new Pursuit(transform, sbTarget, sbRigidbodyTarget, sbTime);
        //sbSeek = new Seek(sbSpeed, sbRotationSpeed, sbWaypoints, lineOfSight, agentAstar, transform, sbRigidbodyTarget);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSteering(ISteeringBehaviour _sb)
    {
        sb = _sb;
    }
}
