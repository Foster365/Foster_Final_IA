using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCAIController : MonoBehaviour
{

    NPCCharacterModel charModel;
    GameObject leaderGameObject;

    #region FSM_Variables
    //FSM Variables
    [Header("Finite State Machine variables")]
    [SerializeField] float patrolTimer = 10;
    [SerializeField] float seekRange = 5;
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackCooldown = .5f;
    [SerializeField] float blockCooldown = 2f;

    FSM<string> npcFSM;
    #endregion

    #region Line_Of_Sight_Variables
    //Line of Sight variables
    [Header("Line of Sight variables")]
    [SerializeField] float lineOfSightViewDistance = 10;
    [SerializeField] float lineOfSightViewCone = 90;
    [SerializeField] RaycastHit lineOfSightHitInfo;
    [SerializeField] LayerMask lineOfSightObstacleLayer;
    [SerializeField] LayerMask lineOfSightTarget;

    LineOfSight characterLineOfSight;
    #endregion

    #region Steering_Behaviours_Variables
    //Steering Behaviours variables
    [Header("Steering Behaviours variables")]
    [Header("Obstacle Avoidance variables")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float obstacleAvoidanceRadius, obstacleAvoidanceAvoidWeight;

    ObstacleAvoidance characterObstacleAvoidance;

    [Header("Pursuit variables")]
    [SerializeField] float pursuitSBTimePrediction;
    Pursuit npcPursuitSteeringBehaviour;

    [Header("Flee variables")]
    Flee npcFleeSteeringBehaviour;
    //
    #endregion

    #region Pathfinding_Variables
    //Pathfinding variables
    Pathfinding characterPathfinding;
    #endregion

    #region Roulette_Wheel_Variables
    //Roulette Wheel variables

    Roulette attackRoulette;
    Dictionary<BehaviourTreeNode, int> attackRouletteWheelNodes = new Dictionary<BehaviourTreeNode, int>();
    BehaviourTreeNode attackRouletteWheelIinitNode;
    //
    #endregion

    #region Flocking_Variables
    //Flocking variables
    [Header("Flocking variables")]
    [SerializeField] float flockingAvoidDist;
    [SerializeField] float flockingTurningDistance;


    [Header("Flocking Agent settings")]

    [SerializeField] float flockingMinSpeed;
    float flockingSpeed;
    [SerializeField] float flockingMaxSpeed;
    [Range(1.0f, 10.0f)]
    [SerializeField] float flockingNeighbourDist;
    [Range(.5f, 5.0f)]
    [SerializeField] float flockingRotationSpeed;

    //FlockManager myManager;
    bool flockingTurning = false;
    Vector3 flockingMovementLimits;
    Vector3 flockingGoalPos;
    CharacterSpawnGrid charSpawnGrid;

    Flocking flockingBehaviour;

    FlockEntity flockingEntity;

    //
    #endregion

    #region Encapsulated Variables
    public LineOfSight CharacterLineOfSight { get => characterLineOfSight; set => characterLineOfSight = value; }
    public NPCCharacterModel CharModel { get => charModel; set => charModel = value; }
    public Pathfinding CharacterPathfinding { get => characterPathfinding; set => characterPathfinding = value; }
    public ObstacleAvoidance CharacterObstacleAvoidance { get => characterObstacleAvoidance; set => characterObstacleAvoidance = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float BlockCooldown { get => blockCooldown; set => blockCooldown = value; }
    public Roulette AttackRoulette { get => attackRoulette; set => attackRoulette = value; }
    public float SeekRange { get => seekRange; set => seekRange = value; }
    //public FlockManager MyManager { get => myManager; set => myManager = value; }
    public float MinSpeed { get => flockingMinSpeed; set => flockingMinSpeed = value; }
    public float MaxSpeed { get => flockingMaxSpeed; set => flockingMaxSpeed = value; }
    public float NeighbourDist { get => flockingNeighbourDist; set => flockingNeighbourDist = value; }
    public float RotationSpeed { get => flockingRotationSpeed; set => flockingRotationSpeed = value; }
    public GameObject LeaderGameObject { get => leaderGameObject; set => leaderGameObject = value; }
    public Vector3 FlockingGoalPos { get => flockingGoalPos; set => flockingGoalPos = value; }
    public FlockEntity FlockingEntity { get => flockingEntity; set => flockingEntity = value; }
    public Pursuit NpcPursuitSteeringBehaviour { get => npcPursuitSteeringBehaviour; set => npcPursuitSteeringBehaviour = value; }
    public Flee NpcFleeSteeringBehaviour { get => npcFleeSteeringBehaviour; set => npcFleeSteeringBehaviour = value; }
    public float PursuitSBTimePrediction { get => pursuitSBTimePrediction; set => pursuitSBTimePrediction = value; }
    #endregion

    private void Awake()
    {
        charModel = GetComponent<NPCCharacterModel>();
        flockingEntity = GetComponent<FlockEntity>();
        CheckLeaderTarget();
    }

    // Start is called before the first frame update
    void Start()
    {
        charSpawnGrid = GameObject.Find("NPC_A_Object_Pooler").gameObject.GetComponent<CharacterSpawnGrid>();
        flockingSpeed = charModel.MovementSpeed;
        //Debug.Log("Componentes: " + "AllPrefabs " + charSpawnGrid.AllPrefabs + " GO " + gameObject + " FlockAvoidDist " + flockingAvoidDist
        //    + " FlockNbDist " + flockingNeighbourDist + " FlockSpeed " + flockingSpeed + " FlockMaxSpeed " + flockingMaxSpeed
        //    + " FlockRotSpeed " + flockingRotationSpeed + " Flock Target Pos " + leaderGameObject.transform.position);

        SetUpAIComponents();
    }

    // Update is called once per frame
    void Update()
    {
        //charModel.Patrol(characterPathfinding);
        //FlockBehaviour();
        npcFSM.OnUpdate();
    }

    void CheckLeaderTarget()
    {
        if (gameObject.tag == TagManager.NPC_A_NAME_TAG) leaderGameObject = GameObject.Find("Leader_A").gameObject;
        else if (gameObject.tag == TagManager.NPC_B_NAME_TAG) leaderGameObject = GameObject.Find("Leader_B").gameObject;
    }

    public void SetUpAIComponents()
    {
        SetUpFSM();
        //CreateAttackRoulette();
        flockingMovementLimits = new Vector3(charSpawnGrid.GridSizeX, 1, charSpawnGrid.GridSizeY);
        flockingGoalPos = leaderGameObject.transform.position;
        flockingBehaviour = new Flocking(charSpawnGrid.AllPrefabs, gameObject, flockingAvoidDist, flockingNeighbourDist, flockingMaxSpeed,
            flockingRotationSpeed, leaderGameObject.transform.position);
        characterLineOfSight = new LineOfSight(lineOfSightViewDistance, lineOfSightViewCone, new RaycastHit(),
                                            transform, lineOfSightObstacleLayer, lineOfSightTarget);
        characterPathfinding = new Pathfinding(transform, characterLineOfSight.Target, charModel.PathfindingLastPosition, charModel.MapGrid);

        //characterObstacleAvoidance = new ObstacleAvoidance(transform, obstacleAvoidanceRadius, obstacleAvoidanceAvoidWeight, obstacleLayer);
    }

    #region Finite_State_Machine
    void SetUpFSM()
    {

        npcFSM = new FSM<string>();

        NPCIdleState<string> npcIdleState = new NPCIdleState<string>(this, npcFSM,
            TagManager.NPC_FSM_FOLLOW_LEADER_STATE_TAG, TagManager.NPC_FSM_SEEK_STATE_TAG, TagManager.NPC_FSM_ATTACK_STATE_TAG,
            TagManager.NPC_FSM_DAMAGE_STATE_TAG, TagManager.NPC_FSM_FLEE_STATE_TAG, TagManager.NPC_FSM_DEATH_STATE_TAG);

        NPCFollowLeaderState<string> npcFollowLeaderState = new NPCFollowLeaderState<string>(this, npcFSM, TagManager.NPC_FSM_IDLE_STATE_TAG,
            TagManager.NPC_FSM_SEEK_STATE_TAG);

        NPCSeekState<string> npcSeekState = new NPCSeekState<string>(this, npcFSM, TagManager.NPC_FSM_FOLLOW_LEADER_STATE_TAG,
            TagManager.NPC_FSM_ATTACK_STATE_TAG);

        NPCAttackState<string> npcAttackState = new NPCAttackState<string>(this, npcFSM,
            TagManager.NPC_FSM_IDLE_STATE_TAG, TagManager.NPC_FSM_DAMAGE_STATE_TAG);

        NPCFleeState<string> npcFleeState = new NPCFleeState<string>(this, npcFSM, TagManager.NPC_FSM_IDLE_STATE_TAG,
            TagManager.NPC_FSM_DEATH_STATE_TAG);

        NPCDamageState<string> npcDamageState = new NPCDamageState<string>(this, npcFSM, TagManager.NPC_FSM_IDLE_STATE_TAG);

        NPCDeathState<string> npcDeathState = new NPCDeathState<string>(this, npcFSM);

        npcIdleState.AddTransition(TagManager.NPC_FSM_FOLLOW_LEADER_STATE_TAG, npcFollowLeaderState);
        npcIdleState.AddTransition(TagManager.NPC_FSM_SEEK_STATE_TAG, npcSeekState);
        npcIdleState.AddTransition(TagManager.NPC_FSM_ATTACK_STATE_TAG, npcAttackState);
        npcIdleState.AddTransition(TagManager.NPC_FSM_DAMAGE_STATE_TAG, npcDamageState);
        npcIdleState.AddTransition(TagManager.NPC_FSM_FLEE_STATE_TAG, npcFleeState);// Probar si con idle -> flee no es mejor que con damage -> flee
        npcIdleState.AddTransition(TagManager.NPC_FSM_DEATH_STATE_TAG, npcDeathState);// Probar si con idle -> flee no es mejor que con damage -> flee

        npcFollowLeaderState.AddTransition(TagManager.NPC_FSM_IDLE_STATE_TAG, npcIdleState);
        npcFollowLeaderState.AddTransition(TagManager.NPC_FSM_SEEK_STATE_TAG, npcSeekState);

        npcSeekState.AddTransition(TagManager.NPC_FSM_FOLLOW_LEADER_STATE_TAG, npcFollowLeaderState);
        npcSeekState.AddTransition(TagManager.NPC_FSM_ATTACK_STATE_TAG, npcAttackState);

        npcAttackState.AddTransition(TagManager.NPC_FSM_IDLE_STATE_TAG, npcIdleState);
        npcAttackState.AddTransition(TagManager.NPC_FSM_DAMAGE_STATE_TAG, npcDamageState);

        npcFleeState.AddTransition(TagManager.NPC_FSM_IDLE_STATE_TAG, npcIdleState);
        npcFleeState.AddTransition(TagManager.NPC_FSM_DEATH_STATE_TAG, npcDeathState);

        npcDamageState.AddTransition(TagManager.NPC_FSM_IDLE_STATE_TAG, npcIdleState);
        npcDamageState.AddTransition(TagManager.NPC_FSM_FLEE_STATE_TAG, npcFleeState);



        npcFSM.SetInit(npcIdleState);

    }
    #endregion

    #region Roulette_Wheel

    #region Attack_Roulette_Wheel
    public void AttackRouletteAction()
    {
        BehaviourTreeNode attackNodeRoulette = attackRoulette.Run(attackRouletteWheelNodes);
        attackNodeRoulette.Execute();
    }

    public void CreateAttackRoulette()
    {
        Debug.Log("Ruleta Enemy Creada");
        attackRoulette = new Roulette();

        ActionNode attack1 = new ActionNode(charModel.Attack1);
        ActionNode attack2 = new ActionNode(charModel.Attack2);
        ActionNode block = new ActionNode(charModel.Block1);

        attackRouletteWheelNodes.Add(attack1, 40);
        attackRouletteWheelNodes.Add(attack2, 35);
        attackRouletteWheelNodes.Add(block, 15);

        ActionNode attackRouletteAction = new ActionNode(AttackRouletteAction);

    }
    #endregion
    #endregion

    #region Flocking

    public void FlockBehaviour()
    {
        Bounds b = new Bounds(leaderGameObject.transform.position, flockingMovementLimits * 2);
        RaycastHit hit;
        Vector3 direction = Vector3.zero;

        RandomlyChangeFlockDirection();

        if (!b.Contains(transform.position))
        {
            flockingTurning = true;
            direction = leaderGameObject.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, transform.forward * flockingAvoidDist, out hit))
        {
            flockingTurning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
        }
        //else if (Vector3.Distance(transform.position, myManager.GoalPosition) <= turningDistance) turning = true;
        else flockingTurning = false;

        if (flockingTurning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                flockingRotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                flockingSpeed = Random.Range(flockingMinSpeed, flockingMaxSpeed);

            if (Random.Range(0, 1000) < 10)
            {
                flockingBehaviour.ApplyFlockingRules();
                flockingSpeed = flockingBehaviour.Speed;
            }

        }

        transform.Translate(0, 0, Time.deltaTime * flockingSpeed);
        //charModel.Move(direction.normalized, flockingSpeed);
        //return direction;
        //characterPathfinding.FindPath(transform.position, flockingGoalPos);
        //if (characterPathfinding.finalPath.Count > 1) charModel.Run(characterPathfinding.finalPath);// transform.Translate(0, 0, Time.deltaTime * flockingSpeed);

    }

    void RandomlyChangeFlockDirection()
    {
        if (Random.Range(0, 500) < 100)
            flockingGoalPos = this.transform.position + new Vector3(Random.Range(-flockingMovementLimits.x, flockingMovementLimits.x),
                    Random.Range(-flockingMovementLimits.y, flockingMovementLimits.y), Random.Range(-flockingMovementLimits.z, flockingMovementLimits.z));
    }
    #endregion
}
