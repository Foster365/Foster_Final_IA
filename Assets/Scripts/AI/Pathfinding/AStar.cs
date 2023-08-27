using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AStar
{
    public Transform seeker, target;
    Grid grid;
    public List<Node> finalPath;
    Vector3 lastPos;

    public AStar(Transform _seeker, Transform _target, Grid _grid)
    {
        seeker = _seeker;
        target = _target;
        grid = _grid;
    }

    public AStar(Transform _seeker, Transform _target, Vector3 _lastPos, Grid _grid)
    {
        seeker = _seeker;
        target = _target;
        lastPos = _lastPos;
        grid = _grid;
    }

    public void FindPath(Vector3 startPos, Vector3 targetNodePosition)//(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        Node startNode = grid.GetNodeFromWorldPoint(startPos);//(_startPosition);
        Console.WriteLine("Start node: " + startNode);
        Node targetNode = grid.GetNodeFromWorldPoint(targetNodePosition);//(_targetPosition);

        Heap<Node> openNodesList = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedNodesSet = new HashSet<Node>();
        finalPath = new List<Node>();

        openNodesList.Add(startNode);

        while (openNodesList.Count > 0)
        {
            Node currentNode = openNodesList.RemoveFirst();

            closedNodesSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
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
