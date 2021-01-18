using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float rotSpeed;
    protected Vector3 direction;

    //Las funciones abstractas, al igual que las interfaces, definen un contrato que las clases que hereden de esta clase tiene que cumplir e implementar
    //la función Move. Por eso no tiene cuerpo en este clase, porque será definido por las clases concretas.
    protected abstract void Move(Vector3 dir);

}
