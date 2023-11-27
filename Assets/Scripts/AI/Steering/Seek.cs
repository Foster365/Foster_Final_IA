using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteeringBehaviour
{
    private Transform _origin;
    private Transform _target;

    public Seek(Transform origin, Transform target)
    {
        _origin = origin;
        _target = target;
    }

    public Transform Target { get => _target; set => _target = value; }

    public Vector3 GetDir()
    {
        return (_target.position - _origin.position).normalized;
    }

}