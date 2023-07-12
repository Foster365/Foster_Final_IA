using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderAIController
{

    CharacterModel charModel;

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

    Roulette attackRoulette;
    Dictionary<BehaviourTreeNode, int> attackRouletteWheelNodes = new Dictionary<BehaviourTreeNode, int>(); //TODO puede ser una funcion que retorne un nro según el ambiente (Un peso dinámico)
    BehaviourTreeNode attackRouletteWheelIinitNode;
    //

    #region Encapsulated Variables
    public LineOfSight CharacterLineOfSight { get => characterLineOfSight; set => characterLineOfSight = value; }
    public CharacterModel CharModel { get => charModel; set => charModel = value; }
    public Pathfinding CharacterPathfinding { get => characterPathfinding; set => characterPathfinding = value; }
    public ObstacleAvoidance CharacterObstacleAvoidance { get => characterObstacleAvoidance; set => characterObstacleAvoidance = value; }
    public Roulette AttackRoulette { get => attackRoulette; set => attackRoulette = value; }
    public FSM<string> LeaderFSM { get => leaderFSM; set => leaderFSM = value; }
    #endregion
    
    public void SetUpAIComponents()
    {
        SetUpFSM();
        CreateAttackRoulette();
        characterLineOfSight = new LineOfSight(lineOfSightViewDistance, lineOfSightViewCone, new RaycastHit(),
                                            charModel.transform, lineOfSightObstacleLayer, lineOfSightTarget);
        characterPathfinding = new Pathfinding(charModel.transform, charModel.Target, charModel.PathfindingLastPosition, charModel.MapGrid);
        characterObstacleAvoidance = new ObstacleAvoidance(charModel.transform, obstacleAvoidanceRadius, obstacleAvoidanceAvoidWeight, obstacleLayer);
    }

    #region Finite_State_Machine
    void SetUpFSM()
    {
    }
    #endregion

    #region Attack_Roulette_Wheel
    public void AttackRouletteAction()
    {
        BehaviourTreeNode attackNodeRoulette = attackRoulette.Run(attackRouletteWheelNodes);
        attackNodeRoulette.Execute();
    }

    public void CreateAttackRoulette()
    {
        attackRoulette = new Roulette();

        //ActionNode attack1 = new ActionNode(charModel.Attack1);
        //ActionNode attack2 = new ActionNode(charModel.Attack2);
        //ActionNode attack3 = new ActionNode(charModel.Attack3);

        //attackRouletteWheelNodes.Add(attack1, 30);
        //attackRouletteWheelNodes.Add(attack2, 25);
        //attackRouletteWheelNodes.Add(attack3, 40);

        ActionNode attackRouletteAction = new ActionNode(AttackRouletteAction);

    }
    #endregion

    private void OnDrawGizmos()
    {
        if (characterLineOfSight != null)
        {

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(charModel.transform.position, lineOfSightViewDistance);

            Gizmos.color = Color.blue;
            Vector3 rightLimit = Quaternion.AngleAxis(lineOfSightViewCone, Vector3.up) * Vector3.forward;
            Gizmos.DrawLine(charModel.transform.position, charModel.transform.position + (rightLimit * lineOfSightViewDistance));

            Vector3 leftLimit = Quaternion.AngleAxis(-lineOfSightViewCone, Vector3.up) * Vector3.forward;
            Gizmos.DrawLine(charModel.transform.position, charModel.transform.position + (leftLimit * lineOfSightViewDistance));

            if (characterLineOfSight.Target != null)
            {
                Gizmos.color = characterLineOfSight.targetInSight ? Color.green : Color.red;
                Gizmos.DrawLine(charModel.transform.position, characterLineOfSight.Target.position);
            }

        }
    }

}
