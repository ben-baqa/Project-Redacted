using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileTree", menuName = "Scriptable Objects/Create File Tree", order = 1)]
public class FileTree : ScriptableObject
{
    public List<FileNode> TheTree;
}

[System.Serializable]
public class FileNode {
    public string ParentName;
    public string Name;
}
