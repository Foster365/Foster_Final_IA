using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek
{
    public bool move;
    private LineOfSight lineOfSight;
    private Transform target;
    public List<Vector3> waypoints;
    int _nextPoint = 0;
    private AgentAStar agent;
    private float timer;
    protected float speed;
    protected float rotSpeed;
    AgentAStar agentAStar;

    Transform transform;
    Rigidbody rbTarget;

    public Transform Target { get => target; set => target = value; }

    public Seek(float _speed, float _rotSpeed, List<Vector3> _waypoints, LineOfSight _lineOfSight, AgentAStar _agentAStar, Transform _transform, Rigidbody _rbTarget)
    {
        speed = _speed;
        rotSpeed = _rotSpeed;

        waypoints = _waypoints;

        lineOfSight = _lineOfSight;
        agentAStar = _agentAStar;

        transform = _transform;
        rbTarget = _rbTarget;

    }

    private void Awake()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (move)
        {
            if (timer >= 1f)
            {
                agent.PathFindingAStarVector();
                timer = 0;
            }
            Run();
        }
    }

    public void Move(Vector3 dir)
    {
        Target = lineOfSight.Target;

        if (move && Target != null)
        {

            transform.position += Time.deltaTime * rbTarget.velocity/*dir*/ * speed; ;
            transform.forward = Vector3.Lerp(transform.forward, rbTarget.velocity/*dir*/, rotSpeed * Time.deltaTime);
        }
    }

    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        waypoints = newPoints;
        var pos = waypoints[_nextPoint];
        pos.y = transform.position.y;
        transform.position = pos;
        move = true;
    }

    public void Run()
    {
        var point = waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < waypoints.Count)
                _nextPoint++;
            else
            {
                move = false;
                return;
            }
        }
        Move(dir.normalized);
    }
}
