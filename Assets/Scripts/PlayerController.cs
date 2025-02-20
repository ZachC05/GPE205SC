using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Keys for Shooting || Can left click to shoot")]
    public KeyCode shootKey;
    // Start is called before the first frame update
    public override void Start()
    {
        //If we have a GameManager
        if(GameControl.instance != null)
        {
            //track the players
            if(GameControl.instance.players != null)
            {
                //Add to manager
                GameControl.instance.players.Add(this);
            }
        }
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
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }
        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
        }
        if (Input.GetKey(rotateRightKey))
        {
            pawn.RotateRight();
        }
        if (Input.GetKey(rotateLeftKey))
        {
            pawn.RotateLeft();
        }
        if(Input.GetKey(shootKey) || Input.GetMouseButton(0))
        {
            pawn.Shoot();
        }
    }
    public void OnDestroy()
    {
        //check if gamemanager
        if (GameControl.instance != null)
        {
            //and is tracking players
            if (GameControl.instance.players != null)
            {
                //remove from list
                GameControl.instance.players.Remove(this);
            }
        }
    }
}
