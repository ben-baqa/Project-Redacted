using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileStorageHandler : MonoBehaviour
{
    public string storageName;
    public bool locked;
    public string password;

    public FileTree originalFileTree;

    private List<FileNode> nodes;
    void Start()
    {
        nodes = originalFileTree.nodes;
    }

    public FileNode GetNode(string name, string directory)
    {
        int index = nodes.FindIndex(node => node.name == name && node.directory == directory);
        if (index > -1)
        {
            return nodes[index];
        }
        return null;
    }

    public List<FileNode> GetNodesAt(string directory)
    {
        return nodes.Where(node => node.directory == directory).ToList();
    }

    public bool RemoveNode(string name, string directory)
    {
        int index = nodes.FindIndex(node => node.name == name && node.directory == directory);
        if (index > -1)
        {
            nodes.RemoveAt(index);
            return true;
        }
        return false;
    }
}
