using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class NodesManager
{

    int nodesGridWidth, nodesGridHeight;
    float cellSize;
    GameObject nodeModelTest;
    Transform parentTransform;
    int[,] nodesGridArray;

    TextMesh[,] debugTextArray;

    public NodesManager(int _nodesGridWidth, int _nodesGridHeight, float _cellSize,
        GameObject _gridNodeModel, Transform _parentTransform)
    {
        nodesGridHeight = _nodesGridHeight;
        nodesGridWidth = _nodesGridWidth;
        cellSize = _cellSize;

        nodeModelTest = _gridNodeModel;
        parentTransform = _parentTransform;

        nodesGridArray = new int[nodesGridWidth, nodesGridHeight];

    }
    public void RenderGrid()
    {

        for (int x = 0; x < nodesGridArray.GetLength(0); x++)
        {

            for (int y = 0; y < nodesGridArray.GetLength(1); y++)
            {

                Vector3 pos = parentTransform.position + GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                GameObject.Instantiate(nodeModelTest, pos, Quaternion.identity);

            }

        }

    }
    Vector3 GetWorldPosition(int _x, int _y)
    {
        return new Vector3(_x, -.5f, _y) * cellSize;
    }

    public void SetValue(int _x, int _y, int _value)
    {
        if (_x >= 0 && _y >= 0 && _x < nodesGridWidth && _y < nodesGridHeight)
        {
            nodesGridArray[_x, _y] = _value;
        }
    }

}
