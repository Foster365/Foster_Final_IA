using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathfinding : MonoBehaviour
{
    public List<NodePathfinding> neighbours;
    public bool hasTrap;
    private void Start()
    {
        Neighbour(Vector3.right);
        Neighbour(Vector3.left);
        Neighbour(Vector3.forward);
        Neighbour(Vector3.back);
    }
    void Neighbour(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 100, 1 << LayerMask.NameToLayer("Walkable")))
        {

            //neighbours.Add(hit.collider.GetComponent<NodePathfinding>());

            var node = hit.collider.GetComponent<NodePathfinding>();
            if (node != null)
                neighbours.Add(node);

        }
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        //{

        //}
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1);

    }
}
