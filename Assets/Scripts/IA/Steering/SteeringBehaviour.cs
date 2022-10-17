using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float rotSpeed;
    protected Vector3 direction;

    protected abstract void Move(Vector3 dir);

}
