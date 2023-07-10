using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public bool isWalkable;
    public Vector3 worldPosition;
    public int gCost, hCost;


    public int gridX, gridY, movementPenalty;

    public Node parent;
    int heapIndex;


    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        isWalkable =  _isWalkable;
        worldPosition = _worldPosition;

        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

    int IComparable<Node>.CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) compare = hCost.CompareTo(nodeToCompare.hCost);
        return -compare;
    }
}
