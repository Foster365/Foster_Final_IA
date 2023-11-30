using UnityEngine;

public class Pursuit{
    Transform _npc;
    Transform _target;
    float _timePrediction;

    public Pursuit(Transform npc, Transform target, float time)
    {
        _timePrediction = time;
        Target = target;
        _npc = npc;
    }

    public Transform Target { get => _target; set => _target = value; }

    public Vector3 GetDir()
    {
        float vel = Target.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        Vector3 posPrediction = Target.position + (Target.forward * vel * _timePrediction);
        //Vector3 dir = Vector3.zero;
        return (posPrediction - _npc.position).normalized;
    }
}

