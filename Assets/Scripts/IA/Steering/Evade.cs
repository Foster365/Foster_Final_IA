﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : ISteeringBehaviour
{
    Transform _npc;
    Transform _target;
    Rigidbody _rbTarget;
    float _timePrediction;
    public Evade(Transform npc, Transform target, Rigidbody rb, float timePrediction)
    {
        _target = target;
        _rbTarget = rb;
        _npc = npc;
    }
    public Vector3 GetDir()
    {
        Vector3 posPrediction = _target.position + _timePrediction * _rbTarget.velocity.magnitude * _target.forward;
        Vector3 dir = (_npc.position - posPrediction).normalized;
        return dir;
    }
}

