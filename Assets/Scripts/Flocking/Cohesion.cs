using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Cohesion : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Vector3 GetDir(List<CharacterModel> boids, CharacterModel self)
    {
        Vector3 center = Vector3.zero;
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            center += boids[i].transform.position;
        }
        if (boids.Count > 0)
        {
            center /= boids.Count;
            dir = center - self.transform.position;
            
        }
        return dir.normalized * multiplier;
    }

}
