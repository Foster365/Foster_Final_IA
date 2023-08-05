using UnityEngine;

public class ObstacleAvoidance : ISteeringBehaviour
{

    private Transform _origin;
    private float _radius;
    private float _viewAngle;
    private LayerMask _mask;
    private Collider[] _allObs;

    public ObstacleAvoidance(Transform origin, float radius, int maxObs, float viewAngle, LayerMask mask)
    {
        _origin = origin;
        _radius = radius;
        _mask = mask;
        _viewAngle = viewAngle;
        _allObs = new Collider[maxObs];
    }

    public Vector3 GetDir()
    {
        var countObs = Physics.OverlapSphereNonAlloc(_origin.position, _radius, _allObs, _mask);
        Vector3 dirToAvoid = Vector3.zero;
        int trueObs = 0;
        for (int i = 0; i < countObs; i++)
        {
            var currObs = _allObs[i];
            var closestPoint = currObs.ClosestPointOnBounds(_origin.transform.position);
            var diffToPoint = closestPoint - _origin.position;

            var angleToPoint = Vector3.Angle(_origin.forward, diffToPoint.normalized);

            if (angleToPoint > _viewAngle / 2) continue;
            float dist = diffToPoint.magnitude;

            trueObs++;
            dirToAvoid += -(diffToPoint).normalized * (_radius - dist);

        }

        if (trueObs != 0)
            dirToAvoid = dirToAvoid / trueObs;

        return dirToAvoid;
    }
}