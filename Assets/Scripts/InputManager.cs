using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SpacePressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool LeftKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }
    public bool UpKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }
    public bool RightKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }
    public bool DownKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool EnterKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

}
