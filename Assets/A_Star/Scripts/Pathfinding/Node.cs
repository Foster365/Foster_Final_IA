using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Node : MonoBehaviour
{
    public List<Node> neightbourds;
    public bool isObstacle;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        GetNeighbours();
    }
    private void Update()
    {
        if (isObstacle)
            mat.color = Color.red;
        else
            mat.color = Color.white;
    }
    void GetNeighbours()
    {
        GetNeighbour(Vector3.right);
        GetNeighbour(Vector3.left);
        GetNeighbour(Vector3.forward);
        GetNeighbour(Vector3.back);
    }
    void GetNeighbour(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        {
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
                neightbourds.Add(node);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, .5f);
        
    }
}
