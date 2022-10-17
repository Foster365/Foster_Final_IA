using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBFSDFS : MonoBehaviour
{
    public float radius;
    public Vector3 offset;
    public NodePathfinding init;
    public NodePathfinding finit;
    List<NodePathfinding> _list;
    BFS<NodePathfinding> _bfs = new BFS<NodePathfinding>();
    DFS<NodePathfinding> _dfs = new DFS<NodePathfinding>();
    public void PathFindingBFS()
    {
        _list = _bfs.Run(init, Satisfies, GetNeighbours, 500);
    }
    public void PathFindingDFS()
    {
        _list = _dfs.Run(init, Satisfies, GetNeighbours, 500);
    }
    List<NodePathfinding> GetNeighbours(NodePathfinding curr)
    {
        var list = new List<NodePathfinding>();
        for (int i = 0; i < curr.neighbours.Count; i++)
        {
            if (curr.neighbours[i].hasTrap) continue;
            list.Add(curr.neighbours[i]);
        }
        return list;
    }
    bool Satisfies(NodePathfinding curr)
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
        if (_list == null) return;
        Gizmos.color = Color.blue;
        foreach (var item in _list)
        {
            if (item != init && item != finit)
                Gizmos.DrawSphere(item.transform.position + offset, radius);
        }
    }
}
