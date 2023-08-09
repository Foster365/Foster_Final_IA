using UnityEngine;

public class Pursuit// : ISteeringBehaviour
{
    Transform _npc;
    float _timePrediction;

    public Pursuit(Transform npc, float time)
    {
        _timePrediction = time;
        _npc = npc;
    }

    public Vector3 GetDir(Transform _target, Rigidbody _rbTarget)
    {
        float vel = _rbTarget.velocity.magnitude;
        Vector3 posPrediction = _target.position + (_target.forward * vel * _timePrediction);
        Vector3 dir = (posPrediction - _npc.position).normalized;
        //Vector3 dir = Vector3.zero;
        return dir;
    }
}

