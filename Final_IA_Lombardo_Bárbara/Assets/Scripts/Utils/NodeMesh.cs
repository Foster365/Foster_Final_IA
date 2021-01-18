using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMesh : MonoBehaviour
{
    public GameObject nodeGizmo;

    public int nodeLoops;

    public float distance;

    public int columns = 100;
    public int space = 1;

    Vector3 randomPos;

    Vector3 vector3DistH;
    Vector3 vector3DistV;

    Vector2 gridSize;
    GameObject[][] gridOfGameObjects;

    List<Transform> nodes;

    public List<Transform> Nodes { get => nodes; set => nodes = value; }

    // Start is called before the first frame update
    void Start()
    {

        gridSize = new Vector2(10, 10);
        gridOfGameObjects = new GameObject[(int)gridSize.x][];
        //ChangeFormation();
        vector3DistH = new Vector3(distance, 0, 0);

        vector3DistV = new Vector3(0, 0, distance);

        NodesGenerator();
        //GenerateNodes();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChangeFormation()
    {

        //for (int i = 0; i < nodeLoops; i++)
        //{
        //    Transform go = Instantiate(nodeGizmo, transform.position, transform.rotation);//Poner Transform nodeGizmo para arreglarlo
        //    Vector3 pos = CalcPosition(i);
        //    go.position = new Vector3(transform.position.x + pos.x, 0, transform.position.y + pos.y);
        //}
        
    }

    public void NodesGenerator()
    {

        for (int x = 0; x < gridSize.x; x++)
        {
            gridOfGameObjects[x] = new GameObject[(int)gridSize.y];
            for (int y = 0; y < gridSize.y; y++)
            {

                GameObject go = Instantiate(nodeGizmo, transform.position, transform.rotation);
                // manipulate gameobject here
                gridOfGameObjects[x][y] = go;
            }
        }

    }

    Vector2 CalcPosition(int index) // call this func for all your objects
    {
        float posX = (index % columns) * space;
        float posY = (index / columns) * space;
        return new Vector2(posX, posY);
    }

    public void GenerateNodes()
    {
        var randomnumber = Random.Range(9f, 24f);
        randomPos = new Vector3(randomnumber, 0, randomnumber);
        Instantiate(nodeGizmo, nodeGizmo.transform.position, nodeGizmo.transform.rotation);
        //Instantiate(nodeMesh, nodeMesh.transform.position, transform.rotation);
        Vector3 currposH = transform.position;
        for (int i = 1; i < nodeLoops; i++)
        {

            currposH += randomPos;
            //Instantiate(nodeMesh, nodeMesh.transform.position + vector3Distance, transform.rotation);
            //nodes.Add();
            Instantiate(nodeGizmo, currposH, transform.rotation);


        }
    }

}

