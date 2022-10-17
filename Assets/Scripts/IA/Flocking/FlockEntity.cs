using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockEntity : MonoBehaviour
{


    public float _neighborRadius = 4;

    private Vector3 _direction; 

    public Vector3 Direction {
        get {
            return _direction;
        }
    }
    public Collider _mainCollider; 

    private FlockBehavior[] _steeringBehaviors;

    public LayerMask _mask;

    // Start is called before the first frame update
    void Awake()
    {
        _steeringBehaviors = this.GetComponents<FlockBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public Vector3 UpdateDirection() {

        Vector3 direction = Vector3.zero;

        List<Transform> context = this.GetNearbyEntities();

        for (int i = 0; i < _steeringBehaviors.Length; i++)
        {
            FlockBehavior behavior = _steeringBehaviors[i];

            direction += behavior.CalculateDirection(context);
        }

        _direction = direction;

        return _direction;
    }


    public List<Transform> GetNearbyEntities() {

        List<Transform> context = new List<Transform>();

        Collider[] contextColliders = Physics.OverlapSphere(this.transform.position, _neighborRadius, _mask);

        foreach(Collider collider in contextColliders)
        {
            if(collider != _mainCollider)
            {
                context.Add(collider.transform);
            }
        }

        return context;
    }
}