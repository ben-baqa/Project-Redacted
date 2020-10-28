using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OpenFileStatus
{
    SUCCESSFUL,
    FILE_NOT_FOUND,
    WRONG_PASSWORD,
    ACCESS_DENIED
}

public class FileSystem : MonoBehaviour
{
    public TerminalHandler fileTerminal;
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

        childNodes = nodes;
        OpenNode("root");
    }

    public OpenFileStatus OpenNode(string node_name, string password = "")
    {
        for (int i = 0; i < childNodes.Count; i++)
        {
            if (childNodes[i].name == node_name)
            {
                if ((childNodes[i].locked && childNodes[i].password == password) || (!childNodes[i].locked))
                {
                    currentNode = childNodes[i];
                    if (currentNode.type == FileType.DIRECTORY)
                    {
                        childNodes = new List<FileNode>();
                        foreach (FileNode node in nodes)
                        {
                            if (node.parentName == node_name)
                            {
                                childNodes.Add(node);
                            }
                        }
                    }
                    else if (currentNode.type == FileType.TEXT)
                    {
                        fileTerminal.FeedLine(currentNode.content);
                    }
                    return OpenFileStatus.SUCCESSFUL;
                }
                return OpenFileStatus.WRONG_PASSWORD;
            }
        }
        return OpenFileStatus.FILE_NOT_FOUND;
    }

    public OpenFileStatus OpenParentNode()
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
                return OpenFileStatus.SUCCESSFUL;
            }
        }
        return OpenFileStatus.FILE_NOT_FOUND;
    }

    public List<FileNode> GetChildNodes()
    {
        return childNodes;
    }
}
