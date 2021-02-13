using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAStar : MonoBehaviour
{
    //[SerializeField]
    //Transform characterAStar;

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    float distanceMax;

    [SerializeField]
    float radius;

    [SerializeField]
    Vector3 offset;

    //[SerializeField]
    Seek seekCharacter;

    [SerializeField]
    NodePathfinding initNode;

    [SerializeField]
    NodePathfinding finitNode;

    [SerializeField]
    Leader leader;

    List<NodePathfinding> _aStarList;
    AStar<NodePathfinding> _aStar = new AStar<NodePathfinding>();

    LeaderSteering leaderSteering;
    //[SerializeField]
    //GameObject finit;

    //public AgentAStar(Transform characterAStar, LayerMask mask, float distanceMax, float radius,
    //Vector3 offset, Seek seekCharacter, GameObject finit)
    //{
    //    this.characterAStar = characterAStar;
    //    this.mask = mask;
    //    this.distanceMax = distanceMax;
    //    this.radius = radius;
    //    this.offset = offset;
    //    this.seekCharacter = seekCharacter;
    //    this.finit = finit;
    //}

    private void Awake()
    {

        leaderSteering = gameObject.GetComponent<LeaderSteering>();

    }

    private void Start()
    {

        //seekCharacter = leaderSteering.SbSeek;

    }

    public void PathfindingAStar()
    {

        Debug.Log("Path not generated");
        _aStarList = _aStar.Run(initNode, SatisfiesVector, GetNeighbours, GetCost, Heuristic);
        if(_aStarList.Count!=0) Debug.Log("AStar empty");

        leader.SetWayPoints(_aStarList);

        //leaderSteering.SbSeek.SetWayPoints(_listVector);
        //Debug.Log("SeekCharacter" + leaderSteering.SbSeek);
        //return _listVector = _aStarVector.Run(initNode.transform.position, SatisfiesVector, GetNeighboursVector, GetCostVector, HeuristicVector);
    }

    float Heuristic(NodePathfinding curr)
    {

        float cost = 0;
        // if (curr.hasTrap) cost += 5;
        cost += Vector3.Distance(curr.transform.position, finitNode.transform.position);
        return cost;
        //return Vector3.Distance(curr, finitNode.transform.position);
    }

    float GetCost(NodePathfinding p, NodePathfinding c)
    {

        return Vector3.Distance(p.transform.position, c.transform.position);

        //return 1;
    }

    List<NodePathfinding> GetNeighbours(NodePathfinding curr)
    {
        return curr.neighbours;
        //List<Vector3> list = new List<Vector3>();
        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int z = -1; z <= 1; z++)
        //    {
        //        if (z == 0 && x == 0) continue;
        //        var newPos = new Vector3(curr.x + x, curr.y, curr.z + z);
        //        var dir = (newPos - curr);

        //        if (Physics.Raycast(curr, dir.normalized, dir.magnitude, mask)) continue;
        //        list.Add(newPos);
        //        //Debug.Log(list.Count);
        //    }
        //}
        //return list;
    }

    bool SatisfiesVector(NodePathfinding curr)
    {
        return curr == finitNode;
        //return Vector3.Distance(curr, finitNode.transform.position) < 1;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 init = transform.position;
    //    Gizmos.color = Color.red;

    //    if (init != null)
    //        Gizmos.DrawSphere(init + offset, radius);

    //    if (finitNode != null)
    //        Gizmos.DrawSphere(finitNode.transform.position + offset, radius);

    //    if (_aStarList != null)
    //    {
    //        Gizmos.color = Color.green;
    //        foreach (var item in _aStarList)
    //        {
    //            if (item != initNode && item != finitNode/*.transform.position*/)
    //                Gizmos.DrawSphere(item + offset, radius);
    //        }
    //    }
    //}
}
