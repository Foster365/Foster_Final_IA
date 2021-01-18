using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public bool move;
    private LineOfSight sight;
    private Transform target;
    public List<Vector3> waypoints;
    int _nextPoint = 0;
    private AgentAStar agent;
    private float timer;

    public Transform Target { get => target; set => target = value; }

    private void Awake()
    {
        sight = GetComponent<LineOfSight>();
        agent = GetComponent<AgentAStar>();
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

    protected override void Move(Vector3 dir)
    {
        Target = sight.Target;

        if (move && Target != null)
        {
            dir.y = 0;
            transform.position += Time.deltaTime * dir * speed; ;
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
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
