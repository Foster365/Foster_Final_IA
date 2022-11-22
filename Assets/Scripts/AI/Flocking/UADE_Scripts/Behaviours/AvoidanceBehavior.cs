using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UADE.IA.Flocking.States
{
    public class AvoidanceBehavior : FlockBehavior
    {

        float personalSpaceRadius = 3;
        GameObject agent;

        public AvoidanceBehavior(GameObject _agent, float _personalSpaceRadius)
        {
            agent = _agent;
            personalSpaceRadius = _personalSpaceRadius;
        }

        public override Vector3 CalculateDirection(List<Transform> context)
        {
            Vector3 direction = Vector3.zero;

            foreach (Transform item in context)
            {
                Vector3 offset = agent.transform.position - item.transform.position;

                if (offset.magnitude < personalSpaceRadius)
                {

                    float scale = offset.magnitude / Mathf.Sqrt(personalSpaceRadius);

                    Vector3 forceVector = offset.normalized / scale;

                    direction += forceVector;
                }
            }

            return direction;
        }
    }
}
