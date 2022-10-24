using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour, IPoints
{
    public List<Node> waypoints;
    public float speed = 2;
    public float rotationSpeed = 10;
    bool readyToMove;
    int _nextPoint = 0;
    Animator _anim;

    public bool ReadyToMove { get => readyToMove; set => readyToMove = value; }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        transform.position += Time.deltaTime * dir * speed; ;
        transform.forward = Vector3.Lerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        //_anim.SetFloat("Vel", 1);
    }
    public void SetWayPoints(List<Node> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        //_anim.Play("CIA_Idle");
        waypoints = newPoints;
        var pos = waypoints[_nextPoint].transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
        readyToMove = true;
    }
    public void Run()
    {
        var point = waypoints[_nextPoint];
        var posPoint = point.transform.position;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < waypoints.Count)
                _nextPoint++;
            else
            {
                readyToMove = false;
                //_anim.SetTrigger("Finish");
                //_anim.SetFloat("Vel", 0);
                return;
            }
        }
        Move(dir.normalized);
    }
}
