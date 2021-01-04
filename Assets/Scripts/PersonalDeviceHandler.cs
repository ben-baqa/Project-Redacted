using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpenNodeStatus
{
    SUCCESSFUL,
    NODE_NOT_FOUND,
    WRONG_PASSWORD,
    ACCESS_DENIED
}

public enum FileBrowsingState
{
    CHOOSING_STORAGE,
    AT_ROOT,
    BROWSING_FILE
}

public enum OpenedStorage
{
    NONE,
    PERSONAL_DEVICE_STORAGE,
    CONNECTED_STORAGE
}

public class PersonalDeviceHandler : MonoBehaviour
{
    public TerminalHandler fileTerminal;

    private FileStorageHandler personalDeviceStorage;
    private FileStorageHandler connectedStorage = null;
    private FileNode currentNode;
    private List<FileNode> childNodes;

    private bool connected = false;
    private FileBrowsingState fileBrowsingState;
    private OpenedStorage openedStorage;

    void Start()
    {
        personalDeviceStorage = gameObject.GetComponent<FileStorageHandler>();
    }

    public void ConnectToStorage(FileStorageHandler externalStorage)
    {
        connectedStorage = externalStorage;
        connected = true;
    }

    public void DisconnectFromStorage()
    {
        connectedStorage = null;
        connected = false;
        if (openedStorage == OpenedStorage.CONNECTED_STORAGE)
        {
            ReturnToChoosingStorage();
        }
    }

    public void ReturnToChoosingStorage()
    {
        fileBrowsingState = FileBrowsingState.CHOOSING_STORAGE;
        openedStorage = OpenedStorage.NONE;
        currentNode = null;
    }

    public void ReturnToRoot()
    {
        if (openedStorage != OpenedStorage.NONE)
        {
            fileBrowsingState = FileBrowsingState.AT_ROOT;
            currentNode = null;
            if (openedStorage == OpenedStorage.PERSONAL_DEVICE_STORAGE)
            {
                childNodes = personalDeviceStorage.GetNodesAt("");
            }
            else
            {
                childNodes = connectedStorage.GetNodesAt("");
            }
        }
    }

    public OpenNodeStatus OpenAnyNode(string name, string directory)
    {
        FileNode nodeToOpen;
        if (openedStorage == OpenedStorage.PERSONAL_DEVICE_STORAGE)
        {
            nodeToOpen = personalDeviceStorage.GetNode(name, directory);
        }
        else if (openedStorage == OpenedStorage.CONNECTED_STORAGE)
        {
            nodeToOpen = connectedStorage.GetNode(name, directory);
        }
        else
        {
            return OpenNodeStatus.NODE_NOT_FOUND;
        }
        if (nodeToOpen == null)
        {
            return OpenNodeStatus.NODE_NOT_FOUND;
        }
        switch (nodeToOpen.type)
        {
            case NodeType.DIRECTORY:
                currentNode = nodeToOpen;
                if (openedStorage == OpenedStorage.PERSONAL_DEVICE_STORAGE)
                {
                    childNodes = personalDeviceStorage.GetNodesAt($"{currentNode.directory}/{currentNode.name}");
                }
                else
                {
                    childNodes = connectedStorage.GetNodesAt($"{currentNode.directory}/{currentNode.name}");
                }
                break;
            case NodeType.TEXT:
                fileTerminal.FeedLine(nodeToOpen.content);
                break;
        }
        nodeToOpen.locked = false;
        fileBrowsingState = FileBrowsingState.BROWSING_FILE;
        return OpenNodeStatus.SUCCESSFUL;
    }

    public OpenNodeStatus OpenNodeAt(string name, string password = "")
    {
        int index = childNodes.FindIndex(node => node.name == name);
        if (index > -1)
        {
            if (!childNodes[index].locked || (childNodes[index].locked && childNodes[index].password == password))
            {
                OpenAnyNode(name, currentNode.name);
                return OpenNodeStatus.SUCCESSFUL;
            }
                return OpenNodeStatus.WRONG_PASSWORD;
        }
        else
        {
            return OpenNodeStatus.NODE_NOT_FOUND;
        }
    }

    public OpenNodeStatus ReturnToParent()
    {
        switch (fileBrowsingState)
        {
            case FileBrowsingState.CHOOSING_STORAGE:
                return OpenNodeStatus.NODE_NOT_FOUND;
            case FileBrowsingState.AT_ROOT:
                ReturnToChoosingStorage();
                return OpenNodeStatus.SUCCESSFUL;
            default:
                if (currentNode.directory == "")
                {
                    ReturnToRoot();
                }
                else
                {
                    int index = currentNode.directory.LastIndexOf("/");
                    string name = currentNode.directory.Substring(index + 1);
                    string dir = currentNode.directory.Substring(0, index);
                    OpenAnyNode(name, dir);
                }
                return OpenNodeStatus.SUCCESSFUL;
        }
    }

    public List<FileNode> ListNodesAtCurrentDir()
    {
        return childNodes;
    }

    public bool IsConnected()
    {
        return connected;
    }

    public FileBrowsingState GetFileBrowsingState()
    {
        return fileBrowsingState;
    }

    public OpenedStorage GetOpenedStorage()
    {
        return openedStorage;
    }

    public List<FileStorageHandler> ListAvailableStorages()
    {
        if (connected)
        {
            return new List<FileStorageHandler>() { personalDeviceStorage, connectedStorage };
        }
        return new List<FileStorageHandler>() { personalDeviceStorage };
    }

    public OpenNodeStatus OpenStorage(string name, string password = "")
    {
        if (connected && connectedStorage.storageName == name)
        {
            if (!connectedStorage.locked || (connectedStorage.locked && connectedStorage.password == password))
            {
                openedStorage = OpenedStorage.CONNECTED_STORAGE;
                ReturnToRoot();
                return OpenNodeStatus.SUCCESSFUL;
            }
            return OpenNodeStatus.WRONG_PASSWORD;
        }
        else if (personalDeviceStorage.storageName == name)
        {
            if (!personalDeviceStorage.locked || (personalDeviceStorage.locked && personalDeviceStorage.password == password))
            {
                openedStorage = OpenedStorage.PERSONAL_DEVICE_STORAGE;
                ReturnToRoot();
                return OpenNodeStatus.SUCCESSFUL;
            }
            return OpenNodeStatus.WRONG_PASSWORD;
        }
        return OpenNodeStatus.NODE_NOT_FOUND;
    }
}
