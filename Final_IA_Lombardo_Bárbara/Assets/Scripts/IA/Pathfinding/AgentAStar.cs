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

    [SerializeField]
    Seek seekCharacter;

    List<Vector3> _listVector;
    AStar<Vector3> _aStarVector = new AStar<Vector3>();

    [SerializeField]
    GameObject finit;

    LeaderSteering leaderSteering;

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
        leaderSteering = GetComponent<LeaderSteering>();
    }

    private void Start()
    {

        seekCharacter = leaderSteering.SbSeek;

    }

    public void PathFindingAStarVector()
    {
        _listVector = _aStarVector.Run(transform.position, SatisfiesVector, GetNeighboursVector, GetCostVector, HeuristicVector);
        seekCharacter.SetWayPoints(_listVector);
    }

    float HeuristicVector(Vector3 curr)
    {
        return Vector3.Distance(curr, finit.transform.position);
    }

    float GetCostVector(Vector3 p, Vector3 c)
    {
        return 1;
    }

    List<Vector3> GetNeighboursVector(Vector3 curr)
    {
        List<Vector3> list = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (z == 0 && x == 0) continue;
                var newPos = new Vector3(curr.x + x, curr.y, curr.z + z);
                var dir = (newPos - curr);

                if (Physics.Raycast(curr, dir.normalized, dir.magnitude, mask)) continue;
                list.Add(newPos);
                //Debug.Log(list.Count);
            }
        }
        return list;
    }

    bool SatisfiesVector(Vector3 curr)
    {
        return Vector3.Distance(curr, finit.transform.position) < 1;
    }

    private void OnDrawGizmos()
    {
        Vector3 init = transform.position;
        Gizmos.color = Color.red;

        if (init != null)
            Gizmos.DrawSphere(init + offset, radius);

        if (finit != null)
            Gizmos.DrawSphere(finit.transform.position + offset, radius);

        if (_listVector != null)
        {
            Gizmos.color = Color.green;
            foreach (var item in _listVector)
            {
                if (item != init && item != finit.transform.position)
                    Gizmos.DrawSphere(item + offset, radius);
            }
        }
    }
}
