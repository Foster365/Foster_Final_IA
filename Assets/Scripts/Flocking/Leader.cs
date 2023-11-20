using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Transform target;
    CharacterModel model;

    private void Awake()
    {
        model = GetComponent<CharacterModel>();
    }

    private void Start()
    {
        target = GameObject.Find(model.Data.BossName).transform;
    }

    public Vector3 GetDir(List<CharacterModel> boids, CharacterModel self)
    {
        return (target.position - self.transform.position).normalized * multiplier;
    }
}
