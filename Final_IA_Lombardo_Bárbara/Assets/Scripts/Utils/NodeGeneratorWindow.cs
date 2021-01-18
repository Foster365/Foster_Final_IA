using UnityEngine;
using UnityEditor;

public class NodeGeneratorWindow : EditorWindow
{
    public NodeMesh node;

    [MenuItem("Window/CustomTools")]
    public static void ShowWindow()
    {
        GetWindow<NodeGeneratorWindow>("Node Generator");
    }

    void OnGUI()
    {

    }
}

