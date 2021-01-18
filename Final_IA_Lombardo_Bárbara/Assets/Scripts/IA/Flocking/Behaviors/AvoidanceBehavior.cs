using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UADE.IA.Flocking.States {
    public class AvoidanceBehavior : FlockBehavior
    {

        public float _personalSpaceRadius = 3;


        public override Vector3 CalculateDirection(List<Transform> context)
        {
            Vector3 direction = Vector3.zero;

            foreach (Transform item in context)
            {
                Vector3 offset = this.transform.position - item.transform.position; 

                if(offset.magnitude < _personalSpaceRadius) {

                    float scale = offset.magnitude / Mathf.Sqrt(_personalSpaceRadius);

                    Vector3 forceVector = offset.normalized / scale;

                    direction += forceVector;
                }
            }            

            return direction;
        }
    }
}
