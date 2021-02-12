using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathfinding : MonoBehaviour
{
    public List<NodePathfinding> neighbours;
    public bool hasTrap;
    private void Start()
    {
        Neighbor(Vector3.right);
        Neighbor(Vector3.left);
        Neighbor(Vector3.forward);
        Neighbor(Vector3.back);
    }
    void Neighbor(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        {
            neighbours.Add(hit.collider.GetComponent<NodePathfinding>());
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1);

    }
}
