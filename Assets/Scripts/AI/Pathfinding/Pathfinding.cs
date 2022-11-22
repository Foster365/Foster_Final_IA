using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding
{
    public Transform seeker, target;
    Grid grid;
    public List<Node> finalPath;
    Vector3 lastPos;

    public Pathfinding(Transform _seeker, Transform _target, Vector3 _lastPos, Grid _grid)
    {
        seeker = _seeker;
        target = _target;
        lastPos = _lastPos;
        grid = _grid;
    }

    //private void Awake()
    //{
    //    lastPos = Vector3.zero;
    //    grid = GameObject.Find("Grid").GetComponent<Grid>();
    //}

    //private void Update()
    //{
    //    if (Vector3.Distance(target.position, lastPos) > 1)
    //    {
    //        lastPos = target.position;
    //        FindPath(transform.position, target.position);
    //    }

    //}
    public void FindPath(Vector3 _startPosition, Vector3 _targetPosition)//, Func<Vector3,bool> isSatisfies)
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        Node startNode = grid.GetNodeFromWorldPoint(_startPosition);
        Console.WriteLine("Start node: " + startNode);
        Node targetNode = grid.GetNodeFromWorldPoint(_targetPosition);

        Heap<Node> openNodesList = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedNodesSet = new HashSet<Node>();
        finalPath = new List<Node>();

        openNodesList.Add(startNode);

        while (openNodesList.Count > 0)
        {
            Node currentNode = openNodesList.RemoveFirst();

            closedNodesSet.Add(currentNode);

            if (currentNode == targetNode)//(isSatisfies(currentNode.worldPosition))
            {
                sw.Stop();
                //print("Path found in " + sw.ElapsedMilliseconds + "ms");

                finalPath = RetracePath(startNode, targetNode);

                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedNodesSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                if (newMovementCostToNeighbour < neighbour.gCost || !openNodesList.ContainsItem(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openNodesList.ContainsItem(neighbour)) openNodesList.Add(neighbour);
                    else openNodesList.UpdateItem(neighbour);
                }
            }

        }


    }

    List<Node> RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = _endNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            Console.WriteLine("Current node " + currentNode.parent);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

}
