using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Awake()
    {
        transform.position = ((Vector3)Vector3Int.RoundToInt(transform.position*2))/2;  
    }
}
