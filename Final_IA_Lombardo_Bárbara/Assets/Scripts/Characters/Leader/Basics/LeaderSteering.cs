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
    Transform sbTransformTarget;
    [SerializeField]
    Rigidbody sbRigidbodyEntity;
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
    LayerMask sbObstacleLayerMask;

    [SerializeField]
    Vector3 sbDirection;

    [SerializeField]
    bool sbMove;

    LineOfSight lineOfSight;
    [SerializeField]
    AgentAStar agentAstar;
    //

    ISteeringBehaviour sb;

    Evade sbEvade;
    Flee sbFlee;
    ObstacleAvoidance sbObstacleAvoidance;
    Pursuit sbPursuit;
    Seek sbSeek;

    public Seek SbSeek { get => sbSeek; set => sbSeek = value; }

    private void Awake()
    {

        sbRigidbodyEntity = GetComponent<Rigidbody>();
        lineOfSight = GetComponent<LineOfSight>();
        agentAstar = GetComponent<AgentAStar>();

    }

    // Start is called before the first frame update
    void Start()
    {

        sbEvade = new Evade(transform, sbTransformTarget, sbRigidbodyEntity, sbTimePrediction);
        sbFlee = new Flee(sbMove, sbSpeed, sbRotationSpeed, transform, lineOfSight, sbTransformTarget, sbDirection);
        sbObstacleAvoidance = new ObstacleAvoidance(transform, sbRadius, sbAvoidWeight, sbObstacleLayerMask);
        sbPursuit = new Pursuit(transform, sbTransformTarget, sbRigidbodyTarget, sbTime);
        sbSeek = new Seek(sbSpeed, sbRotationSpeed, sbWaypoints, lineOfSight, agentAstar, transform, sbRigidbodyTarget);

        //sbTransformTarget = GetTarget();
        //sbRigidbodyTarget = GetTarget().GetComponent<Rigidbody>();//Ojo

    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
    }

    public void ChangeSteering(ISteeringBehaviour _sb)
    {
        sb = _sb;
    }

    Transform GetTarget()
    {
        return lineOfSight.GetLineOfSight();
    }
}
