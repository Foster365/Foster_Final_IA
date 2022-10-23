using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class NodesManager
{

    int nodesGridWidth, nodesGridHeight, nodesGridDepth;
    float cellSize;
    GameObject nodeModelTest;
    Transform parentTransform;
    int[,] nodesGridArray;

    TextMesh[,] debugTextArray;

    public NodesManager(int _nodesGridWidth, int _nodesGridHeight/*, int _nodesGridDepth*/, float _cellSize,
        GameObject _gridNodeModel, Transform _parentTransform)
    {
        nodesGridHeight = _nodesGridHeight;
        nodesGridWidth = _nodesGridWidth;
        //nodesGridDepth = _nodesGridDepth;
        cellSize = _cellSize;

        nodeModelTest = _gridNodeModel;
        parentTransform = _parentTransform;

        nodesGridArray = new int[nodesGridWidth, nodesGridHeight];
        //debugTextArray = new TextMesh[nodesGridWidth, nodesGridHeight, nodesGridDepth];

        //SetValue(2, 1, 1, 56);
    }
    public void RenderGrid()
    {

        for (int x = 0; x < nodesGridArray.GetLength(0); x++)
        {

            for (int y = 0; y < nodesGridArray.GetLength(1); y++)
            {

                //for (int z = 0; z < nodesGridArray.GetLength(2); z++)
                //{
                Vector3 pos = parentTransform.position + GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                GameObject.Instantiate(nodeModelTest, pos, Quaternion.identity);

                //Debug.Log(nodesGridWidth + " " + nodesGridHeight + " " + nodesGridDepth);
                //UtilsClass.CreateWorldText(nodesGridArray[x, y, z].ToString(), null, GetWorldPosition(x, y, z) + new Vector3(cellSize, cellSize, cellSize) * .5f, 20, Color.yellow, TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x, 0, z + 1), Color.magenta, 100f);
                //Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x, 0, z), Color.magenta, 100f);
                //Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x + 1, y, z), Color.magenta, 100f);
                //}
            }

            //Debug.DrawLine(GetWorldPosition(0, 0, nodesGridDepth), GetWorldPosition(nodesGridWidth, 0, nodesGridDepth), Color.green, 100f);
            //Debug.DrawLine(GetWorldPosition(0, 0, 0), GetWorldPosition(nodesGridWidth, 0, nodesGridDepth), Color.green, 100f);
            //Debug.DrawLine(GetWorldPosition(nodesGridWidth, 0, 0), GetWorldPosition(nodesGridWidth, 0, nodesGridDepth), Color.green, 100f);

        }

    }
    Vector3 GetWorldPosition(int _x, int _y/*, int _z*/)
    {
        return new Vector3(_x, -.5f, _y) * cellSize;
    }

    public void SetValue(int _x, int _y/*, int _z*/, int _value)
    {
        if (_x >= 0 && _y >= 0 /*&& _z >= 0 */&& _x < nodesGridWidth && _y < nodesGridHeight /*&& _z < nodesGridDepth*/)
        {
            nodesGridArray[_x, _y] = _value;
            //debugTextArray[_x, _y, _z].text = nodesGridArray[_x, _y, _z].ToString();
        }
    }

}
