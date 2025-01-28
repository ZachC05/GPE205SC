using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    //Keys are use to get input from players
    [Header("Keys for Moving")]
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;

    [Header("Keys for Rotations")]
    public KeyCode rotateRightKey;
    public KeyCode rotateLeftKey;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        GetInputs();
        base.Update();
    }

    public override void GetInputs()
    {
        //gets inputs from player and communicates it to the pawn scripts
        if (Input.GetKeyDown(moveForwardKey))
        {
            pawn.MoveForward();
        }
        if (Input.GetKeyDown(moveBackwardKey))
        {
            pawn.MoveBackward();
        }
        if (Input.GetKeyDown(rotateRightKey))
        {
            pawn.RotateRight();
        }
        if (Input.GetKeyDown(rotateLeftKey))
        {
            pawn.RotateLeft();
        }
    }
}
