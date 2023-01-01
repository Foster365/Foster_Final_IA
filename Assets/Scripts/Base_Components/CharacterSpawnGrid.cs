using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnGrid : MonoBehaviour
{
    [SerializeField] Vector2 gridWorldSize;
    [SerializeField] GameObject prefab, leader;
    [SerializeField] int troopSize;
    Vector3[,] prefabsGrid;
    GameObject[] allPrefabs;//Para flocking
    int gridSizeX, gridSizeY, troopSizeCounter;
    [SerializeField] string poolTag;
    [SerializeField] float nodeRadius = 1.5f;
    float nodeDiameter = 2;
    Vector3 worldPoint;

    ObjectPooler pooler;

    public GameObject[] AllPrefabs { get => allPrefabs; }
    public int GridSizeX { get => gridSizeX; }
    public int GridSizeY { get => gridSizeY; }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        troopSizeCounter = 0;
        allPrefabs = new GameObject[troopSize];
        pooler = ObjectPooler.Instance;
        CreateGrid();
    }

    public void CreateGrid()
    {
        prefabsGrid = new Vector3[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2)
            - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (troopSizeCounter < troopSize)
                {
                    worldPoint = worldBottomLeft + (Vector3.right * ((x * nodeDiameter) + nodeRadius))
                        + (Vector3.forward * ((y * nodeDiameter) + nodeRadius));
                    var go = pooler.SpawnFromPool(poolTag, worldPoint, leader.transform.rotation);//GameObject.Instantiate(prefab, worldPoint, leader.transform.rotation);
                    allPrefabs[troopSizeCounter] = go;
                    troopSizeCounter++;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(worldPoint, 2);

    }
}
