using UnityEngine;

public interface ISteeringBehaviour
{

    /*void Move(Vector3 dir);*/
    Vector3 GetDir(Vector3 _target);

}

