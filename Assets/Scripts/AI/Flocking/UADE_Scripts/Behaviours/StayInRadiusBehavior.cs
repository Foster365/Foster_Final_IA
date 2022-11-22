using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInRadiusBehavior : FlockBehavior
{
    GameObject agent;
    float radius = 10;

    public StayInRadiusBehavior(GameObject _agent, float _radius)
    {
        agent = _agent;
        radius = _radius;
    }

    public override Vector3 CalculateDirection(List<Transform> context)
    {
        Vector3 direction = Vector3.zero;
        Vector3 center = Vector3.zero;

        Vector3 centerOffset = center - agent.transform.position;

        float df = centerOffset.magnitude / radius;

        if (df > .9f)
        {
            direction = centerOffset * df * df;
        }

        return direction;
    }
}
