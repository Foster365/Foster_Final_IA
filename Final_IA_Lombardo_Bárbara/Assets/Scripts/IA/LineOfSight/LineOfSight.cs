using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewCone;
    [SerializeField] private RaycastHit hitInfo;
    private Transform target;
    [SerializeField] private Transform visionPoint;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask player;


    public bool targetInSight = false;

    public float ViewDistance
    {
        get
        {
            return viewDistance;
        }

        set
        {
            viewDistance = Mathf.Clamp(value, 0, 1000);
        }
    }

    public Transform Target { get => target; set => target = value; }

    public void Update()
    {
        GetLineOfSight();
    }

    public void GetLineOfSight()
    {

        Target = null;
        Collider[] overlapSphere = Physics.OverlapSphere(transform.position, viewDistance, player, QueryTriggerInteraction.Ignore);
        if (overlapSphere.Length > 0)
        {
            Target = overlapSphere[0].transform;
        }



        targetInSight = false;
        if (Target != null)
        {
            // 1 - Si está en mi rango de visión
            float distanceToTarget = Vector3.Distance(visionPoint.position, Target.position);

            if (distanceToTarget <= viewDistance)
            {
                // 2 - Si está en mi cono de visión
                Vector3 targetDir = (Target.position - visionPoint.position).normalized; // Asi se calcula
                float angleToTarget = Vector3.Angle(visionPoint.forward, targetDir);

                if (angleToTarget <= viewCone)
                {
                    // 3 - Si no hay un obstaculo en el medio
                    if (!Physics.Raycast(visionPoint.position, targetDir, out hitInfo, distanceToTarget, obstacleLayers, QueryTriggerInteraction.Ignore))
                    {
                        targetInSight = true;
                    }
                }
            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionPoint.position, viewDistance);

        Gizmos.color = Color.blue;
        Vector3 rightLimit = Quaternion.AngleAxis(viewCone, transform.up) * transform.forward;
        Gizmos.DrawLine(visionPoint.position, visionPoint.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewCone, transform.up) * transform.forward;
        Gizmos.DrawLine(visionPoint.position, visionPoint.position + (leftLimit * viewDistance));

        if (Target != null)
        {
            Gizmos.color = targetInSight ? Color.green : Color.red;
            Gizmos.DrawLine(visionPoint.position, Target.position);
        }

    }
}
