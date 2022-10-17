using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UADE.IA.Flocking.States {
    public class StayInRadiusBehavior : FlockBehavior
    {
        [SerializeField]
        private float _radius = 10;
        public override Vector3 CalculateDirection(List<Transform> context)
        {
            Vector3 direction = Vector3.zero;            
            Vector3 center = Vector3.zero;            

            Vector3 centerOffset = center - this.transform.position;

            float df = centerOffset.magnitude / _radius;

            if(df > .9f) {
                direction = centerOffset * df * df;
            }

            return direction;
        }
    }
}
