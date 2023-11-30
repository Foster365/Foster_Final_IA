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
    Pursuit sbPursuit;
    ObstacleAvoidance sbObstacleAvoidance;
    AStar aStarPathFinding;
    GameObject finalNode;
    EntityView view;

    Transform target;

    bool isTargetInSight;

    public AStar AStarPathFinding { get => aStarPathFinding; set => aStarPathFinding = value; }
    public bool IsTargetInSight { get => isTargetInSight; set => isTargetInSight = value; }
    public Transform Target { get => target; set => target = value; }
    public EntityView View { get => view; set => view = value; }
    public ObstacleAvoidance SbObstacleAvoidance { get => sbObstacleAvoidance; set => sbObstacleAvoidance = value; }
    public Flee SbFlee { get => sbFlee; set => sbFlee = value; }
    public Seek SbSeek { get => sbSeek; set => sbSeek = value; }
    public Pursuit SbPursuit { get => sbPursuit; set => sbPursuit = value; }

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
        sbPursuit = new Pursuit(model.transform, target, model.CharAIData.SbPursuitTime);
        sbObstacleAvoidance = new ObstacleAvoidance(model.transform, model.CharAIData.ObstacleAvoidanceRadius,
            model.CharAIData.ObstacleAvoidanceMaxObstacles, model.CharAIData.ObstacleAvoidanceViewAngle,
            model.CharAIData.ObstacleAvoidanceLayerMask);
        aStarPathFinding = new AStar(model.transform, model.MapGrid);

    }

    public void UpdateControllerComponents()
    {
        if(!model.HealthController.IsDead)
        {
            UpdateSBTargetReference();
            charFSM.UpdateState();
        }
    }

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

    void UpdateSBTargetReference()
    {

        if (target != null)
        {
            sbFlee.Target = target;
            sbSeek.Target = target;
            sbPursuit.Target = target;
        }
    }

    #endregion

}
