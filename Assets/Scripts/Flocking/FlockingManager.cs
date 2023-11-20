using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public int maxBoids = 5;
    public LayerMask maskBoids;
    CharacterModel _self;
    IFlocking[] _flockings;
    Collider[] _colliders;
    List<CharacterModel> _boids;

    private void Awake()
    {
        _self = GetComponent<CharacterModel>();
        _flockings = GetComponents<IFlocking>();
        _boids = new List<CharacterModel>();
        _colliders = new Collider[maxBoids];
    }

    public void RunFlocking()
    {
        _boids.Clear();

        int count = Physics.OverlapSphereNonAlloc(_self.transform.position, _self.Data.FlockRadius, _colliders, maskBoids);

        for (int i = 0; i < count; i++)
        {
            var curr = _colliders[i];
            CharacterModel boid = curr.GetComponent<CharacterModel>();
            if (boid == null || boid == _self) continue;
            _boids.Add(boid);

        }

        Vector3 dir = Vector3.zero;

        for (int i = 0; i < _flockings.Length; i++)
        {
            var currFlock = _flockings[i];
            dir += currFlock.GetDir(_boids, _self);
        }
        _self.Move(dir.normalized);
        _self.LookDir(dir.normalized);
    }

    //aplicarla dentro de un estado de move o look dir
    public Vector3 RunFlockingDir()
    {
        _boids.Clear();

        int count = Physics.OverlapSphereNonAlloc(_self.transform.position, _self.Data.FlockRadius, _colliders, maskBoids);

        for (int i = 0; i < count; i++)
        {
            var curr = _colliders[i];
            CharacterModel boid = curr.GetComponent<CharacterModel>();
            if (boid == null || boid == _self) continue;
            _boids.Add(boid);

        }

        Vector3 dir = Vector3.zero;

        for (int i = 0; i < _flockings.Length; i++)
        {
            var currFlock = _flockings[i];
            dir += currFlock.GetDir(_boids, _self);
        }
        return dir.normalized;
    }
}
