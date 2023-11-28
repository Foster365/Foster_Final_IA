using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour, IFlocking
{
    public float multiplier;
    public float predatorRange;
    public int predatorMax;
    public LayerMask predatorMask;
    Collider[] _colliders;
    private void Awake()
    {
        _colliders = new Collider[predatorMax];
    }
    public Vector3 GetDir(List<CharacterModel> boids, CharacterModel self)
    {
        int count = Physics.OverlapSphereNonAlloc(self.transform.position, predatorRange, _colliders, predatorMask);
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            print(_colliders[i]);
            var diff = self.transform.position - _colliders[i].transform.position;
            dir += diff.normalized * (predatorRange - diff.magnitude);
        }
        return dir.normalized * multiplier;
    }
}
