using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : ISteeringBehaviour
{
    Transform _target;
    Transform _npc;
    Rigidbody _rbTarget;
    float _timePrediction;

    public Pursuit(Transform npc, Transform t, Rigidbody rbTarget, float time)
    {
        _timePrediction = time;
        _rbTarget = rbTarget;
        _npc = npc;
        _target = t;
    }

    public Vector3 GetDir()
    {
        float vel = _rbTarget.velocity.magnitude;
        Vector3 posPrediction = _target.transform.position + _target.transform.forward * vel * _timePrediction;
        Vector3 dir = (posPrediction - _npc.position).normalized;
        return dir;
    }
}

