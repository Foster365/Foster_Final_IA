using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

public class CharacterAIController
{

    CharacterModel model;
    StateData fsmInitialState;
    FsmScript charFSM;
    Seek sbSeek;
    Flee sbFlee;
    ObstacleAvoidance sbObstacleAvoidance;
    AStar aStarPathFinding;
    GameObject finalNode;
    EntityView view;

    Transform target;

    bool isTargetInSight;

    //Regular Attack roulette wheel
    private Roulette _regularAttacksRouletteWheel;
    private Dictionary<ActionNode, int> _regularAttacksRouletteWheelNodes = new Dictionary<ActionNode, int>();

    public AStar AStarPathFinding { get => aStarPathFinding; set => aStarPathFinding = value; }
    public bool IsTargetInSight { get => isTargetInSight; set => isTargetInSight = value; }
    public Transform Target { get => target; set => target = value; }
    public EntityView View { get => view; set => view = value; }

    public CharacterAIController(CharacterModel model, StateData fsmInitialState, GameObject finalNode)
    {
        this.model = model;
        this.fsmInitialState = fsmInitialState;

        this.finalNode = finalNode;
        if (model.HealthController.IsLeader) view = (LeaderView)view;
    }

    public void InitControllerComponents()
    {
        charFSM = new FsmScript(model, fsmInitialState);
        sbSeek = new Seek(model.transform, target);
        sbFlee = new Flee(model.transform, target);
        sbObstacleAvoidance = new ObstacleAvoidance(model.transform, model.CharAIData.ObstacleAvoidanceRadius,
            model.CharAIData.ObstacleAvoidanceMaxObstacles, model.CharAIData.ObstacleAvoidanceViewAngle,
            model.CharAIData.ObstacleAvoidanceLayerMask);
        aStarPathFinding = new AStar(model.transform, finalNode.transform, model.MapGrid);
        RegularAttacksRouletteSetUp();
    }

    public void UpdateControllerComponents()
    {
        charFSM.UpdateState();
    }

    #region Finite State Machine

    #endregion

    #region Line Of Sight
    public bool LineOfSight()
    {
        target = null;
        Collider[] overlapSphere = Physics.OverlapSphere(model.transform.position, model.CharAIData.SightRange, model.CharAIData.TargetLayer);

        if (overlapSphere.Length > 0)
        {
            target = overlapSphere[0].transform;
        }

        isTargetInSight = false;
        if (target != null)
        {
            // 1 - Si está en mi rango de visión
            float distanceToTarget = Vector3.Distance(model.transform.position, target.position);

            if (distanceToTarget <= model.CharAIData.SightRange)
            {
                // 2 - Si está en mi cono de visión
                Vector3 targetDir = (target.position - model.transform.position).normalized; // Asi se calcula
                float angleToTarget = Vector3.Angle(model.transform.forward, targetDir);

                if (angleToTarget <= model.CharAIData.TotalSightDegrees)
                {
                    RaycastHit hitInfo = new RaycastHit();

                    if (!Physics.Raycast(model.transform.position, targetDir, out hitInfo, distanceToTarget, model.CharAIData.ObstacleLayerMask))
                    {
                        isTargetInSight = true;
                    }

                }
                if (isTargetInSight)
                {
                    model.IsAllert = true;
                    model.IsSeeingTarget = true;
                }
                else
                {
                    model.IsAllert = false;
                    model.IsSeeingTarget = false;
                }

            }
        }
        return isTargetInSight;
    }
    #endregion

    #region Steering Behaviours

    #region Seek

    #endregion

    #region Flee
    #endregion

    #region Obstacle Avoidance
    #endregion

    #endregion

    #region Roulette Wheel

    #region Regular Attacks Roulette Wheel
    void RegularAttacksRouletteSetUp()
    {
        _regularAttacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(PlayAttack1);
        ActionNode Attack2 = new ActionNode(PlayAttack2);
        ActionNode Attack3 = new ActionNode(PlayAttack3);

        _regularAttacksRouletteWheelNodes.Add(Attack1, 30);
        _regularAttacksRouletteWheelNodes.Add(Attack2, 25);
        _regularAttacksRouletteWheelNodes.Add(Attack3, 35);

        ActionNode rouletteAction = new ActionNode(EnemyRegularAttacksRouletteAction);
    }

    void PlayAttack1()
    {
        model.View.CharacterAttack1Animation();
    }

    void PlayAttack2()
    {
        model.View.CharacterAttack2Animation();
    }

    void PlayAttack3()
    {
        model.View.CharacterAttack3Animation();
    }

    public void EnemyRegularAttacksRouletteAction()
    {
        INode node = _regularAttacksRouletteWheel.Run(_regularAttacksRouletteWheelNodes);
        node.Execute();

    }

    #endregion
    #endregion


}
