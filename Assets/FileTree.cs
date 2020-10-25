using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileTree", menuName = "Scriptable Objects/Create File Tree", order = 1)]
public class FileTree : ScriptableObject
{
    public List<FileNode> theTree;
}

public enum FileType { 
    Text,
    Directory,
    Executable
}

[System.Serializable]
public class FileNode {
    [Header("Names")]
    public string name;
    public string parentName;

    [Header("Password")]
    public bool locked;
    public string password;

    [Header("File type and content")]
    public FileType type;
    [TextArea(10, 10)]
    public string content;
}
