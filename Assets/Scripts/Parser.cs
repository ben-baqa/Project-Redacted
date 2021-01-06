using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{

    public TerminalHandler terminalTextHandler;
    public PersonalDeviceHandler personalDeviceHandler;

    private InputField inputField;

    private string previousCommand;

    public void Start()
    {
        inputField = gameObject.GetComponent<InputField>();
    }

    public void Update()
    {
        inputField.Select();
        inputField.ActivateInputField();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputField.text = previousCommand;
        }
    }

    public void DetectInput(String input_text)
    {
        if (Input.GetKeyDown(KeyCode.Return) && terminalTextHandler.TerminalIdle())
        {
            previousCommand = input_text;
            terminalTextHandler.FeedLine("> " + input_text);
            Parse(input_text);
        }
        inputField.text = "";
    }

    public void Parse(string input_text)
    {
        input_text = input_text.Trim();
        string[] inputs = input_text.Split(' ');
        string command = inputs[0].ToUpper();

        switch (command)
        {
            case "ECHO":
            case "CAT":
                PrintCommand(input_text, inputs);
                break;
            case "OPEN":
                if (inputs.Length < 2)
                {
                    personalDeviceHandler.ReturnToChoosingStorage();
                }
                else
                {
                    OpenNodeCommand(inputs);
                }
                break;
            case "BACK":
                BackCommand();
                break;
            case "CD":
                if (inputs.Length > 1)
                {
                    if (inputs[1] == "..")
                    {
                        BackCommand();
                    }
                    else
                    {
                        OpenNodeCommand(inputs);
                    }
                }
                else
                {
                    personalDeviceHandler.ReturnToChoosingStorage();
                }
                break;
            case "HELP":
                terminalTextHandler.FeedLine(
                    "List of available common commands:\n" +
                    "ECHO: print text or variable\n" +
                    "LIST: show list of available directory or file\n" +
                    "OPEN: open a file or a directory, password may be needed\n" +
                    "BACK: return to parent folder\n" +
                    "DOWNLOAD: download a file to your device"
                );
                break;
            case "DOWNLOAD":
                terminalTextHandler.FeedLine("This functionality is not implemented.");
                break;
            case "LIST":
            case "LS":
                ListCommand();
                break;
            default:
                terminalTextHandler.FeedLine("Command unknown. Please use the HELP command for a list of common commands.");
                break;
        }
    }

    private void PrintCommand(string input_text, string[] inputs)
    {
        if (inputs.Length < 2)
        {
            terminalTextHandler.FeedLine("Unable to print to terminal due to: MISSING_PRINTABLE_TEXT");
        }
        else
        {
            terminalTextHandler.FeedLine(input_text.Substring(input_text.IndexOf(" ") + 1));
        }
    }

    private void OpenNodeCommand(string[] inputs)
    {
        OpenNodeStatus status;
        if (inputs.Length > 2)
        {
            switch (personalDeviceHandler.GetFileBrowsingState())
            {
                case FileBrowsingState.CHOOSING_STORAGE:
                    status = personalDeviceHandler.OpenStorage(inputs[1], inputs[2]);
                    break;
                default:
                    status = personalDeviceHandler.OpenNodeAt(inputs[1], inputs[2]);
                    break;
            }
        }
        else
        {
            switch (personalDeviceHandler.GetFileBrowsingState())
            {
                case FileBrowsingState.CHOOSING_STORAGE:
                    status = personalDeviceHandler.OpenStorage(inputs[1]);
                    break;
                default:
                    status = personalDeviceHandler.OpenNodeAt(inputs[1]);
                    break;
            }
        }
        switch (status)
        {
            case OpenNodeStatus.NODE_NOT_FOUND:
                terminalTextHandler.FeedLine("Unable to open file due to: NODE_NOT_FOUND");
                break;
            case OpenNodeStatus.WRONG_PASSWORD:
                terminalTextHandler.FeedLine("Unable to open file due to: WRONG_PASSWORD");
                break;
            default:
                terminalTextHandler.FeedLine("Open file successfully!");
                break;
        }
    }

    private void BackCommand()
    {
        switch (personalDeviceHandler.ReturnToParent())
        {
            case OpenNodeStatus.NODE_NOT_FOUND:
                terminalTextHandler.FeedLine("Unable to open file due to: NODE_NOT_FOUND");
                break;
            default:
                terminalTextHandler.FeedLine("Open file successfully!");
                break;
        }
    }

    private void ListCommand()
    {
        switch (personalDeviceHandler.GetFileBrowsingState())
        {
            case FileBrowsingState.CHOOSING_STORAGE:
                foreach (FileStorageHandler storage in personalDeviceHandler.ListAvailableStorages())
                {
                    terminalTextHandler.FeedLine("[" + (storage.locked ? "#" : "_") + $"] [STO] {storage.storageName}");
                }
                break;
            default:
                List<FileNode> nodeToList = personalDeviceHandler.ListNodesAtCurrentDir();
                if (nodeToList.Count == 0)
                {
                    terminalTextHandler.FeedLine("This directory is empty.");
                }
                foreach (FileNode node in nodeToList)
                {
                    string node_type = node.type == NodeType.DIRECTORY ? "DIR" : node.type == NodeType.TEXT ? "TEX" : "EXE";
                    terminalTextHandler.FeedLine("[" + (node.locked ? "#" : "_") + $"] [{node_type}] {node.name}");
                }
                break;
        }
    }
}
