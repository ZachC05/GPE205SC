using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void MoveForward()
    {
        Debug.Log("Moving Forward");
    }
    public override void MoveBackward()
    {
        Debug.Log("Moving Backward");
    }
    public override void RotateRight()
    {
        Debug.Log("Rotating Right");
    }
    public override void RotateLeft()
    {
        Debug.Log("Rotating Left");
    }
}
