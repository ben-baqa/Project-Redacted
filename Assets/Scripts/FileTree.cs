using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileTree", menuName = "Scriptable Objects/Create File Tree", order = 1)]
public class FileTree : ScriptableObject
{
    public List<FileNode> nodes;
}

public enum NodeType
{
    TEXT,
    DIRECTORY,
    EXECUTABLE
}

[System.Serializable]
public class FileNode {
    [Header("Names")]
    public string name;
    public string directory;

    [Header("Password")]
    public bool locked;
    public string password;

    [Header("Node type and content")]
    public NodeType type;
    [TextArea(10, 10)]
    public string content;
}
