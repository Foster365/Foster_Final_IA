using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderAIController : MonoBehaviour
{

    CharacterModel charModel;

    //FSM Variables
    [Header("Finite State Machine variables")]
    [SerializeField] float patrolTimer = 10;
    [SerializeField] float seekRange = 5;
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackCooldown = .5f;
    [SerializeField] float blockCooldown = 2f;

    FSM<string> leaderFSM;

    //Line of Sight variables
    [Header("Line of Sight variables")]
    [SerializeField] float lineOfSightViewDistance = 10;
    [SerializeField] float lineOfSightViewCone = 90;
    [SerializeField] RaycastHit lineOfSightHitInfo;
    [SerializeField] LayerMask lineOfSightObstacleLayer;
    [SerializeField] LayerMask lineOfSightTarget;

    LineOfSight characterLineOfSight;

    //Steering Behaviours variables
    [Header("Steering Behaviours variables")]
    [Header("Obstacle Avoidance variables")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float obstacleAvoidanceRadius, obstacleAvoidanceAvoidWeight;

    ObstacleAvoidance characterObstacleAvoidance;
    //

    //Pathfinding variables
    Pathfinding characterPathfinding;

    //Roulette Wheel variables

    Roulette attackRoulette, blockRoulette, idleAttackTransitionRoulette;
    Dictionary<BehaviourTreeNode, int> attackRouletteWheelNodes = new Dictionary<BehaviourTreeNode, int>();
    Dictionary<BehaviourTreeNode, int> blockRouletteWheelNodes = new Dictionary<BehaviourTreeNode, int>();
    Dictionary<BehaviourTreeNode, int> idleAttackTransitionRouletteWheelNodes = new Dictionary<BehaviourTreeNode, int>();
    BehaviourTreeNode attackRouletteWheelIinitNode, blockRouletteWheelIinitNode, idleAttackTransitionRouletteWheelInitNode;
    //

    #region Encapsulated Variables
    public LineOfSight CharacterLineOfSight { get => characterLineOfSight; set => characterLineOfSight = value; }
    public CharacterModel CharModel { get => charModel; set => charModel = value; }
    public Pathfinding CharacterPathfinding { get => characterPathfinding; set => characterPathfinding = value; }
    public ObstacleAvoidance CharacterObstacleAvoidance { get => characterObstacleAvoidance; set => characterObstacleAvoidance = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float BlockCooldown { get => blockCooldown; set => blockCooldown = value; }
    public Roulette AttackRoulette { get => attackRoulette; set => attackRoulette = value; }
    public Roulette BlockRoulette { get => blockRoulette; set => blockRoulette = value; }
    public float SeekRange { get => seekRange; set => seekRange = value; }
    public FSM<string> LeaderFSM { get => leaderFSM; set => leaderFSM = value; }
    #endregion

    private void Awake()
    {
        charModel = GetComponent<CharacterModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpAIComponents();
    }

    // Update is called once per frame
    void Update()
    {
        //charModel.Patrol(characterPathfinding);
        leaderFSM.OnUpdate();
    }

    public void SetUpAIComponents()
    {
        SetUpFSM();
        CreateAttackRoulette();
        CreateBlockRoulette();
        CreateIdleAttackTransitionRoulette();
        characterLineOfSight = new LineOfSight(lineOfSightViewDistance, lineOfSightViewCone, new RaycastHit(),
                                            transform, lineOfSightObstacleLayer, lineOfSightTarget);
        characterPathfinding = new Pathfinding(transform, charModel.Target, charModel.PathfindingLastPosition, charModel.MapGrid);
        characterObstacleAvoidance = new ObstacleAvoidance(transform, obstacleAvoidanceRadius, obstacleAvoidanceAvoidWeight, obstacleLayer);
    }
    #region Finite_State_Machine
    void SetUpFSM()
    {

        leaderFSM = new FSM<string>();

        LeaderIdleState<string> leaderIdleState = new LeaderIdleState<string>(this, 3, leaderFSM,
            TagManager.LEADER_FSM_PATROL_STATE_TAG, TagManager.LEADER_FSM_SEEK_STATE_TAG, TagManager.LEADER_FSM_ATTACK_STATE_TAG, TagManager.LEADER_FSM_BLOCK_STATE_TAG,
            TagManager.LEADER_FSM_DAMAGE_STATE_TAG, TagManager.LEADER_FSM_DEATH_STATE_TAG);

        LeaderPatrolState<string> leaderPatrolState = new LeaderPatrolState<string>(this, patrolTimer, leaderFSM,
            TagManager.LEADER_FSM_IDLE_STATE_TAG, TagManager.LEADER_FSM_SEEK_STATE_TAG);

        LeaderSeekState<string> leaderSeekState = new LeaderSeekState<string>(this, attackRange, leaderFSM,
            TagManager.LEADER_FSM_PATROL_STATE_TAG, TagManager.LEADER_FSM_ATTACK_STATE_TAG);

        LeaderAttackState<string> leaderAttackState = new LeaderAttackState<string>(this, leaderFSM, TagManager.LEADER_FSM_SEEK_STATE_TAG,
            TagManager.LEADER_FSM_DAMAGE_STATE_TAG, TagManager.LEADER_FSM_IDLE_STATE_TAG);

        LeaderBlockState<string> leaderBlockState = new LeaderBlockState<string>(this, leaderFSM, TagManager.LEADER_FSM_IDLE_STATE_TAG);

        LeaderDamageState<string> leaderDamageState = new LeaderDamageState<string>(this, leaderFSM, TagManager.LEADER_FSM_ATTACK_STATE_TAG,
            TagManager.LEADER_FSM_IDLE_STATE_TAG, TagManager.LEADER_FSM_DEATH_STATE_TAG);

        LeaderDeathState<string> leaderDeathState = new LeaderDeathState<string>(this, leaderFSM);

        leaderIdleState.AddTransition(TagManager.LEADER_FSM_PATROL_STATE_TAG, leaderPatrolState);
        leaderIdleState.AddTransition(TagManager.LEADER_FSM_SEEK_STATE_TAG, leaderSeekState);
        leaderIdleState.AddTransition(TagManager.LEADER_FSM_ATTACK_STATE_TAG, leaderAttackState);
        leaderIdleState.AddTransition(TagManager.LEADER_FSM_BLOCK_STATE_TAG, leaderBlockState);
        leaderIdleState.AddTransition(TagManager.LEADER_FSM_DAMAGE_STATE_TAG, leaderDamageState);
        leaderIdleState.AddTransition(TagManager.LEADER_FSM_DEATH_STATE_TAG, leaderDeathState);

        leaderPatrolState.AddTransition(TagManager.LEADER_FSM_IDLE_STATE_TAG, leaderIdleState);
        leaderPatrolState.AddTransition(TagManager.LEADER_FSM_SEEK_STATE_TAG, leaderSeekState);

        leaderSeekState.AddTransition(TagManager.LEADER_FSM_PATROL_STATE_TAG, leaderPatrolState);
        leaderSeekState.AddTransition(TagManager.LEADER_FSM_ATTACK_STATE_TAG, leaderAttackState);

        leaderAttackState.AddTransition(TagManager.LEADER_FSM_SEEK_STATE_TAG, leaderSeekState);
        leaderAttackState.AddTransition(TagManager.LEADER_FSM_DAMAGE_STATE_TAG, leaderDamageState);
        leaderAttackState.AddTransition(TagManager.LEADER_FSM_IDLE_STATE_TAG, leaderIdleState);
        leaderAttackState.AddTransition(TagManager.LEADER_FSM_BLOCK_STATE_TAG, leaderBlockState); //Esta transición ocurre con mi ruleta de Attack

        leaderBlockState.AddTransition(TagManager.LEADER_FSM_IDLE_STATE_TAG, leaderIdleState);

        leaderDamageState.AddTransition(TagManager.LEADER_FSM_ATTACK_STATE_TAG, leaderAttackState); //Igual pero con ruleta AttackIdle transition
        leaderDamageState.AddTransition(TagManager.LEADER_FSM_IDLE_STATE_TAG, leaderIdleState);
        leaderDamageState.AddTransition(TagManager.LEADER_FSM_DEATH_STATE_TAG, leaderDeathState);

        leaderFSM.SetInit(leaderIdleState);

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
        attackRoulette = new Roulette();

        ActionNode attack1 = new ActionNode(charModel.Attack1);
        ActionNode attack2 = new ActionNode(charModel.Attack2);
        ActionNode attack3 = new ActionNode(charModel.Attack3);
        ActionNode blockStateTransition = new ActionNode(BlockStateTransition);

        attackRouletteWheelNodes.Add(attack1, 30);
        attackRouletteWheelNodes.Add(attack2, 25);
        attackRouletteWheelNodes.Add(attack3, 40);
        attackRouletteWheelNodes.Add(blockStateTransition, 35);

        ActionNode attackRouletteAction = new ActionNode(AttackRouletteAction);

    }
    #endregion

    #region Block_Roulette_Wheel
    public void BlockRouletteAction()
    {
        BehaviourTreeNode blockNodeRoulette = blockRoulette.Run(blockRouletteWheelNodes);
        blockNodeRoulette.Execute();
    }

    public void CreateBlockRoulette()
    {

        blockRoulette = new Roulette();

        ActionNode block1 = new ActionNode(charModel.Block1);
        ActionNode block2 = new ActionNode(charModel.Block2);

        blockRouletteWheelNodes.Add(block1, 40);
        blockRouletteWheelNodes.Add(block2, 45);

        ActionNode blockRouletteAction = new ActionNode(BlockRouletteAction);

    }

    public void BlockStateTransition()
    {
        Debug.Log("Transition to block state");
        leaderFSM.Transition(TagManager.LEADER_FSM_BLOCK_STATE_TAG);
    }

    #endregion

    #region Idle_Attack_Transition_Roulette_Wheel
    public void IdleAttackTransitionRouletteAction()
    {
        BehaviourTreeNode attackNodeRoulette = idleAttackTransitionRoulette.Run(idleAttackTransitionRouletteWheelNodes);
        attackNodeRoulette.Execute();

    }

    public void CreateIdleAttackTransitionRoulette()
    {
        idleAttackTransitionRoulette = new Roulette();

        ActionNode attackStatteTransition = new ActionNode(AttackStateTransition);
        ActionNode blockStateTransition = new ActionNode(BlockStateTransition);

        idleAttackTransitionRouletteWheelNodes.Add(attackStatteTransition, 30);
        idleAttackTransitionRouletteWheelNodes.Add(blockStateTransition, 45);

        ActionNode idleAttackTransitionRouletteAction = new ActionNode(IdleAttackTransitionRouletteAction);

    }
    public void AttackStateTransition()
    {
        leaderFSM.Transition(TagManager.LEADER_FSM_ATTACK_STATE_TAG);
    }
    #endregion
    #endregion

    private void OnDrawGizmos()
    {
        if (characterLineOfSight != null)
        {

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, lineOfSightViewDistance);

            Gizmos.color = Color.blue;
            Vector3 rightLimit = Quaternion.AngleAxis(lineOfSightViewCone, Vector3.up) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + (rightLimit * lineOfSightViewDistance));

            Vector3 leftLimit = Quaternion.AngleAxis(-lineOfSightViewCone, Vector3.up) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + (leftLimit * lineOfSightViewDistance));

            if (characterLineOfSight.Target != null)
            {
                Gizmos.color = characterLineOfSight.targetInSight ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position, characterLineOfSight.Target.position);
            }

        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.forward * attackRange);

    }

}
