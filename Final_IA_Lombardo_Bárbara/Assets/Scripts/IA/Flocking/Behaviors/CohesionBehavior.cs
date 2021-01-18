using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UADE.IA.Flocking.States {
    public class CohesionBehavior : FlockBehavior
    {
        public override Vector3 CalculateDirection(List<Transform> context)
        {
            Vector3 direction = Vector3.zero;
            
            FlockEntity entity = this.GetComponent<FlockEntity>();
            Vector3 centerOfMass = Vector3.zero;

            if(context.Count > 0) 
            {
                foreach (Transform item in context)
                {
                    centerOfMass += item.position;
                }

                centerOfMass /= context.Count;

                direction = centerOfMass - this.transform.position;
            }

            return direction;
        }
    }
}
