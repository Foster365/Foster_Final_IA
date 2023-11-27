using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    GameObject agentPrefab;
    Vector3 movementLimits;
    Vector3 goalPosition;

    CharacterSpawnGrid charsGrid;
    public Vector3 GoalPosition { get => goalPosition; set => goalPosition = value; }
    public Vector3 MovementLimits { get => movementLimits; set => movementLimits = value; }

    public FlockManager(GameObject _agentPrefab, CharacterSpawnGrid _charsGrid)
    {
        agentPrefab = _agentPrefab;
        charsGrid = _charsGrid;
    }

    public void InitVariables()
    {
        goalPosition = agentPrefab.transform.position;
        movementLimits = new Vector3(charsGrid.GridSizeX, 1, charsGrid.GridSizeY);
    }

    public void RandomlyChangeGoalPosition()
    {

        if (Random.Range(0, 500) < 100)
            goalPosition = this.transform.position + new Vector3(Random.Range(-movementLimits.x, movementLimits.x),
                    Random.Range(-movementLimits.y, movementLimits.y), Random.Range(-movementLimits.z, movementLimits.z));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, MovementLimits);
        //Gizmos.DrawWireSphere(transform.position, dist);
    }
}
