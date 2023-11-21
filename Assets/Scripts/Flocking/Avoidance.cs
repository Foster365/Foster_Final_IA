using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour, IFlocking
{
    public float multiplier;
    public float personalRange;
    public Vector3 GetDir(List<CharacterModel> boids, CharacterModel self)
    {
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 diff = self.transform.position - boids[i].transform.position;
            float distance = diff.magnitude;
            if (distance > personalRange) continue;
            dir += diff.normalized * (personalRange - distance);
        }
        return dir.normalized * multiplier;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, personalRange);
    }

}
