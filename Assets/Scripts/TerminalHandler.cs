using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalHandler : MonoBehaviour
{
    public float defaultPrintDelay = 3f;

    private Text terminalText;
    private List<string> buffer;
    private string currentlyPrinting;
    private float nextPrintTime;
    private float printDelay;

    public void Start()
    {
        terminalText = gameObject.GetComponent<Text>();
        buffer = new List<string>();
        currentlyPrinting = null;
        printDelay = defaultPrintDelay / 60f;

        nextPrintTime = Time.time;
    }

    public void Update()
    {
        if (buffer.Count > 0 && currentlyPrinting == null)
        {
            currentlyPrinting = buffer[0];
            buffer.RemoveAt(0);
        }
        else if (currentlyPrinting != null && Time.time > nextPrintTime)
        {
            if (currentlyPrinting.Length > 1)
            {
                terminalText.text += currentlyPrinting[0];
                currentlyPrinting = currentlyPrinting.Substring(1);
            }
            else
            {
                terminalText.text += currentlyPrinting + "\n";
                currentlyPrinting = null;
            }
            nextPrintTime = Time.time + printDelay;
        }
    }

    public void FeedLine(string line)
    {
        buffer.Add(line);
    }

    public void Wipe()
    {
        currentlyPrinting = null;
        buffer.Clear();
        terminalText.text = "";
    }

    public bool TerminalIdle()
    {
        return buffer.Count == 0 && currentlyPrinting == null;
    }
}
