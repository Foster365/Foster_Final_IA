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

    Transform target;

    bool isTargetInSight;

    public AStar AStarPathFinding { get => aStarPathFinding; set => aStarPathFinding = value; }
    public bool IsTargetInSight { get => isTargetInSight; set => isTargetInSight = value; }

    public CharacterAIController(CharacterModel model, StateData fsmInitialState, GameObject finalNode)
    {
        this.model = model;
        this.fsmInitialState = fsmInitialState;

        this.finalNode = finalNode;
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
            // 1 - Si est� en mi rango de visi�n
            float distanceToTarget = Vector3.Distance(model.transform.position, target.position);

            if (distanceToTarget <= model.CharAIData.SightRange)
            {
                // 2 - Si est� en mi cono de visi�n
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
    #endregion


}
