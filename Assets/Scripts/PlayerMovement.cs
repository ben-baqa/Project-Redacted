using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Keys")]
    public string[] upKeys;
    public string[] downKeys, rightKeys, leftKeys;
    [Header("Movement Variables")]
    public float speed;

    private bool upPress, downPress, rightPress, leftPress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetControls();
    }
    // Fixed Update is called 50 fps, regardless of actual framerate
    private void FixedUpdate()
    {
        ExecuteControls();
    }

    /// <summary>
    /// gets the values of all inputs
    /// </summary>
    private void GetControls()
    {
        rightPress |= GetKeyDowns(rightKeys);
        leftPress |= GetKeyDowns(leftKeys);
        upPress |= GetKeyDowns(upKeys);
        downPress |= GetKeyDowns(downKeys);

        rightPress &= GetKeyUps(rightKeys);
        leftPress &= GetKeyUps(leftKeys);
        upPress &= GetKeyUps(upKeys);
        downPress &= GetKeyUps(downKeys);
    }

    /// <summary>
    /// returns true if any of the passed keycodes have been pressed
    /// </summary>
    /// <param name="ar"> the list of keyCodes to check</param>
    /// <returns></returns>
    private bool GetKeyDowns(string[] ar)
    {
        foreach (string s in ar)
        {
            if (Input.GetKeyDown(s))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// returns false if any of the passed keycodes have been released
    /// </summary>
    /// <param name="ar"> the list of keyCodes to check</param>
    /// <returns></returns>
    private bool GetKeyUps(string[] ar)
    {
        foreach (string s in ar)
        {
            if (Input.GetKeyUp(s))
            {
                return false;
            }
        }
        return true;
    }
    
    /// <summary>
    /// runs the basic movement using input obtained from GetControls()
    /// </summary>
    private void ExecuteControls()
    {
        if (leftPress)
            transform.Translate(speed * Vector2.left);
        if (rightPress)
            transform.Translate(speed * Vector2.right);
    }
}
