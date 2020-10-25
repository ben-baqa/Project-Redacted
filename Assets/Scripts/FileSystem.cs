using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    public List<FileTree> trees;
    private List<FileNode> nodes;

    private FileNode currentNode;
    private List<FileNode> childNodes;

    public void Start()
    {
        currentNode = null;
        nodes = new List<FileNode>();
        foreach (FileTree tree in trees)
        {
            nodes.AddRange(tree.theTree);
        }
    }

    public bool OpenNode(string node_name)
    {
        for (int i = 0; i < childNodes.Count; i++) 
        {
            if (childNodes[i].name == node_name) 
            {
                currentNode = childNodes[i];
                childNodes = new List<FileNode>();
                foreach (FileNode node in nodes) 
                {
                    if (node.parentName == node_name)
                    {
                        childNodes.Add(node); 
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool OpenParentNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].name == currentNode.parentName)
            {
                currentNode = nodes[i];
                childNodes = new List<FileNode>();
                foreach (FileNode node in nodes)
                {
                    if (node.parentName == currentNode.parentName)
                    {
                        childNodes.Add(node);
                    }
                }
                return true;
            }
        }
        return false;
    }
}
