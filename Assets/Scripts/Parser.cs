using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{

    public TerminalHandler terminalTextHandler;
    public FileSystem fileSystem;

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
                if (inputs.Length < 2)
                {
                    terminalTextHandler.FeedLine("Unable to print to terminal due to: MISSING_PRINTABLE_TEXT");
                }
                else
                {
                    terminalTextHandler.FeedLine(input_text.Substring(input_text.IndexOf(" ") + 1));
                }
                break;
            case "OPEN":
                if (inputs.Length < 2)
                {
                    terminalTextHandler.FeedLine("Unable to open file due to: MISSING_FILE_NAME");
                }
                else
                {
                    switch (fileSystem.OpenChildNode(inputs[1], inputs.Length > 2 ? inputs[2] : ""))
                    {
                        case OpenFileStatus.ACCESS_DENIED:
                            terminalTextHandler.FeedLine("Unable to open file due to: ACCESS_DENIED");
                            break;
                        case OpenFileStatus.WRONG_PASSWORD:
                            terminalTextHandler.FeedLine("Unable to open file due to: WRONG_PASSWORD");
                            break;
                        case OpenFileStatus.FILE_NOT_FOUND:
                            terminalTextHandler.FeedLine("Unable to open file due to: FILE_NOT_FOUND");
                            break;
                        default:
                            terminalTextHandler.FeedLine("Opened file successfully");
                            break;
                    }
                }
                break;
            case "HELP":
                terminalTextHandler.FeedLines(new String[] { 
                    "List of available common commands:",
                    "ECHO: print text or variable",
                    "LIST: show list of available directory or file",
                    "OPEN: open a file or a directory, password may be needed",
                    "DOWNLOAD: download a file to your device"
                });
                break;
            case "DOWNLOAD":
                terminalTextHandler.FeedLine("This functionality is not implemented.");
                break;
            case "LIST":
                foreach (FileNode node in fileSystem.GetChildNodes())
                {
                    terminalTextHandler.FeedLine(node.name);
                }
                break;
            default:
                terminalTextHandler.FeedLine("Command unknown. Please use the HELP command for a list of common commands.");
                break;
        }
    }
}
