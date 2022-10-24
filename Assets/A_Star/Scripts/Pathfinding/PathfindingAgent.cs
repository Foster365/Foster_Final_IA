using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathfindingAgent : MonoBehaviour
{
    public LayerMask mask;
    public float distanceMax;
    public float radius;
    public Vector3 offset;
    public Node init;
    public Node finit;
    public CharacterModel pj;
    public Box box;
    List<Node> _list;
    AStar<Node> _aStar = new AStar<Node>();
    public void PathFindingAStar()
    {
        _list = _aStar.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic);
        pj.SetWayPoints(_list);
        box.SetWayPoints(_list);
    }
    float Heuristic(Node curr)
    {
        float cost = 0;
        if (curr.isObstacle) cost += 5; //Commented in example
        cost += Vector3.Distance(curr.transform.position, finit.transform.position);
        return cost;
    }
    float GetCost(Node p, Node c)
    {
        return Vector3.Distance(p.transform.position, c.transform.position);
    }
    List<Node> GetNeighbours(Node curr)
    {
        return curr.neightbourds;
    }
    bool Satisfies(Node curr)
    {
        return curr == finit;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (init != null)
            Gizmos.DrawSphere(init.transform.position + offset, radius);
        if (finit != null)
            Gizmos.DrawSphere(finit.transform.position + offset, radius);
        if (_list != null)
        {
            Gizmos.color = Color.blue;
            foreach (var item in _list)
            {
                if (item != init && item != finit)
                    Gizmos.DrawSphere(item.transform.position + offset, radius);
            }
        }
    }
}
