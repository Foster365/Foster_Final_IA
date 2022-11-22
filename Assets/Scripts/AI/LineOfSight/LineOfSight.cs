using UnityEngine;

public class LineOfSight
{
    float viewDistance;
    float viewCone;
    RaycastHit hitInfo;
    Transform target;
    Transform visionPoint;
    LayerMask obstacleLayers;
    LayerMask sightTarget;

    public LineOfSight(float _viewDistance, float _viewCone, RaycastHit _hitInfo, Transform _visionPoint, LayerMask _obstacleLayer, LayerMask _lineOfSightTarget)
    {

        viewDistance = _viewDistance;
        viewCone = _viewCone;

        hitInfo = _hitInfo;

        visionPoint = _visionPoint;
        obstacleLayers = _obstacleLayer;

        sightTarget = _lineOfSightTarget;

    }

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

    public void GetLineOfSight()
    {

        Target = null;
        Collider[] overlapSphere = Physics.OverlapSphere(visionPoint.position, viewDistance, sightTarget);
        if (overlapSphere.Length > 0)
        {
            Target = overlapSphere[0].transform;
        }
        //Debug.Log(Target + "  " + ViewDistance);
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
                    if (!Physics.Raycast(visionPoint.position, targetDir, out hitInfo, distanceToTarget, obstacleLayers))
                    {
                        targetInSight = true;
                    }
                }
            }
        }


    }

}
