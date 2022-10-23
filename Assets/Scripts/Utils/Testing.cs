using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    NodesManager grid;
    [SerializeField] GameObject nodeModelTest;
    [SerializeField] int width, height/*, depth*/, size;
    // Start is called before the first frame update
    void Start()
    {
        grid = new NodesManager(width, height, size, nodeModelTest, transform);
        grid.RenderGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, .3f);
    }
}
