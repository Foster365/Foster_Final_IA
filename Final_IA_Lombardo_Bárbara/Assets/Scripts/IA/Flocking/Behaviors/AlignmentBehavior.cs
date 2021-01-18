using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehavior : FlockBehavior
{
    public override Vector3 CalculateDirection(List<Transform> context)
    {
        Vector3 direction = Vector3.zero;            

        if(context.Count > 0) 
        {
            foreach (Transform item in context)
            {
                Vector3 itemDirection = item.GetComponent<FlockEntity>().Direction;

                direction += itemDirection;
            }

            direction /= context.Count;
        } 
        else 
        {
            direction = this.GetComponent<FlockEntity>().Direction;
        }

        return direction;
    }
}

