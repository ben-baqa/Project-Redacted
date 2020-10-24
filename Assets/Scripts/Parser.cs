using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{

    public TerminalHandler TerminalTextHandler;

    private InputField InputFieldHandler;

    public void Start() {
        InputFieldHandler = gameObject.GetComponent<InputField>();
    }

    public void Update() {
        InputFieldHandler.Select();
        InputFieldHandler.ActivateInputField();
    }

    public void DetectInput(String input_text) {
        if (Input.GetKeyDown(KeyCode.Return) && TerminalTextHandler.TerminalIdle()) {
            TerminalTextHandler.FeedLine("> " + input_text);
            Parse(input_text);
        }
        InputFieldHandler.text = "";
    }

    public void Parse(string input_text) {
        input_text = input_text.Trim();
        string[] inputs = input_text.Split(' ');
        string command = inputs[0].ToUpper();

        switch (command) {
            case "ECHO":
                TerminalTextHandler.FeedLine(input_text.Substring(input_text.IndexOf(" ") + 1));
                break;
            default:
                TerminalTextHandler.FeedLine("Command unknown. Please use the HELP command for a list of common commands.");
                break;
        }
    }
}
