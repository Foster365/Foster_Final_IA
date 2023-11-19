using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

public class CharacterAIController
{
    public delegate int AttackRouletteWheightedCondAction(int val);
    public static event AttackRouletteWheightedCondAction OnHealthCondition;

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

    //Regular Attacks roulette wheel
    Roulette _regularAttacksRouletteWheel;
    Dictionary<ActionNode, int> _regularAttacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
    //

    //Enhanced Attacks roulette wheel
    Roulette _enhancedAttacksRouletteWheel;
    Dictionary<ActionNode, int> _enhancedAttacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
    //

    //Desperate Attacks roulette wheel
    Roulette _desperateAttacksRouletteWheel;
    Dictionary<ActionNode, int> _desperateAttacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
    //

    public AStar AStarPathFinding { get => aStarPathFinding; set => aStarPathFinding = value; }
    public bool IsTargetInSight { get => isTargetInSight; set => isTargetInSight = value; }
    public Transform Target { get => target; set => target = value; }
    public EntityView View { get => view; set => view = value; }

    public CharacterAIController(CharacterModel model, StateData fsmInitialState)
    {
        this.model = model;
        this.fsmInitialState = fsmInitialState;

        //this.finalNode = finalNode;
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
        aStarPathFinding = new AStar(model.transform, model.MapGrid);
        RegularAttacksRouletteSetUp();
        EnhancedAttacksRouletteSetUp();
        DesperateAttacksRouletteSetUp();
        OnHealthCondition += HandleHealthCondition;

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
        //ActionNode Attack4 = new ActionNode(PlayAttack4);

        _regularAttacksRouletteWheelNodes.Add(Attack1, HandleRouletteOptionModifier(model.Attack1Chance));
        _regularAttacksRouletteWheelNodes.Add(Attack2, HandleRouletteOptionModifier(model.Attack2Chance));
        _regularAttacksRouletteWheelNodes.Add(Attack3, HandleRouletteOptionModifier(model.Attack3Chance));
        //_regularAttacksRouletteWheelNodes.Add(Attack4, HandleRouletteOptionModifier(model.Attack4Chance));

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

    void PlayAttack4()
    {
        model.View.CharacterAttack4Animation();
    }

    public void EnemyRegularAttacksRouletteAction()
    {
        INode node = _regularAttacksRouletteWheel.Run(_regularAttacksRouletteWheelNodes);
        node.Execute();

    }

    int HandleRouletteOptionModifier(int value)
    {
        var finalValue = value;
        if (model.gameObject.CompareTag(TagManager.LEADER_TAG)) finalValue = HandleBossRouletteOptionsModifier(value);
        else finalValue = HandleNPCRouletteOptionsModifier(value);

        return finalValue;
    }
    int HandleHealthCondition(int val)
    {
        //Debug.Log("Events for health attack enhancement value is: " + val);
        if(model.HealthController.CurrentHealth < model.CharAIData.EnhancedAttackThreshold)
            val += 1;
        else if(model.HealthController.CurrentHealth < model.CharAIData.EnhancedAttackThreshold)
            val += 2;
        //Debug.Log("Events for health attack enhancement final value is: " + val);
        return val;
    }
    int HandleBossRouletteOptionsModifier(int value)
    {
        value = OnHealthCondition?.Invoke(value) ?? value;
        return value;
    }

    int HandleNPCRouletteOptionsModifier(int value)
    {
        return value;
    }

    #endregion

    #region Enhanced Attacks Roulette Wheel
    void EnhancedAttacksRouletteSetUp()
    {
        _enhancedAttacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(PlayEnhancedAttack1);
        ActionNode Attack2 = new ActionNode(PlayEnhancedAttack2);
        ActionNode Attack3 = new ActionNode(PlayEnhancedAttack3);

        _enhancedAttacksRouletteWheelNodes.Add(Attack1, 30);
        _enhancedAttacksRouletteWheelNodes.Add(Attack2, 25);
        _enhancedAttacksRouletteWheelNodes.Add(Attack3, 35);

        ActionNode rouletteAction = new ActionNode(EnemyEnhancedAttacksRouletteAction);
    }

    void PlayEnhancedAttack1()
    {
        model.View.CharacterAttack1Animation();
    }

    void PlayEnhancedAttack2()
    {
        model.View.CharacterAttack2Animation();
    }

    void PlayEnhancedAttack3()
    {
        model.View.CharacterAttack3Animation();
    }

    public void EnemyEnhancedAttacksRouletteAction()
    {
        INode node = _enhancedAttacksRouletteWheel.Run(_enhancedAttacksRouletteWheelNodes);
        node.Execute();

    }

    #endregion

    #region Desperate Attacks Roulette Wheel
    void DesperateAttacksRouletteSetUp()
    {
        _desperateAttacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(PlayDesperateAttack1);
        ActionNode Attack2 = new ActionNode(PlayDesperateAttack2);
        ActionNode Attack3 = new ActionNode(PlayDesperateAttack3);

        _desperateAttacksRouletteWheelNodes.Add(Attack1, 30);
        _desperateAttacksRouletteWheelNodes.Add(Attack2, 25);
        _desperateAttacksRouletteWheelNodes.Add(Attack3, 35);

        ActionNode rouletteAction = new ActionNode(EnemyDesperateAttacksRouletteAction);
    }

    void PlayDesperateAttack1()
    {
        model.View.CharacterAttack1Animation();
    }

    void PlayDesperateAttack2()
    {
        model.View.CharacterAttack2Animation();
    }

    void PlayDesperateAttack3()
    {
        model.View.CharacterAttack3Animation();
    }

    public void EnemyDesperateAttacksRouletteAction()
    {
        INode node = _desperateAttacksRouletteWheel.Run(_desperateAttacksRouletteWheelNodes);
        node.Execute();

    }

    #endregion
    #endregion


}
