﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{

    public TerminalHandler terminalTextHandler;
    public FileSystem fileSystem;

    private InputField inputField;

    public void Start() {
        inputField = gameObject.GetComponent<InputField>();
    }

    public void Update() {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void DetectInput(String input_text) {
        if (Input.GetKeyDown(KeyCode.Return) && terminalTextHandler.TerminalIdle()) {
            terminalTextHandler.FeedLine("> " + input_text);
            Parse(input_text);
        }
        inputField.text = "";
    }

    public void Parse(string input_text) {
        input_text = input_text.Trim();
        string[] inputs = input_text.Split(' ');
        string command = inputs[0].ToUpper();

        switch (command) {
            case "ECHO":
                terminalTextHandler.FeedLine(input_text.Substring(input_text.IndexOf(" ") + 1));
                break;
            case "OPEN":
                if (inputs.Length < 2)
                {
                    terminalTextHandler.FeedLine("Missing argument: FILE_NAME");
                }
                else if (!fileSystem.OpenNode(inputs[1]))
                {
                    terminalTextHandler.FeedLine("Unable to open file " + inputs[1]);
                }
                else
                {
                    terminalTextHandler.FeedLine("Opened file " + inputs[1]);
                }
                break;
            case "LIST":
                foreach (FileNode node in fileSystem.GetChildNodes()) {
                    terminalTextHandler.FeedLine(node.name);
                }
                break;
            default:
                terminalTextHandler.FeedLine("Command unknown. Please use the HELP command for a list of common commands.");
                break;
        }
    }
}
