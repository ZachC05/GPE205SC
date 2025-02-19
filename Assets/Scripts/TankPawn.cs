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
        //Got from other script and allows the forward movement
        mover.Move(transform.forward, moveSpeed);
    }
    public override void MoveBackward()
    {
        //Got from other script and allows the backward movement
        mover.Move(transform.forward, -moveSpeed);
    }
    public override void RotateRight()
    {
        //Got from other script and allows the right rotation
        mover.Rotate(turnSpeed);
    }
    public override void RotateLeft()
    {
        //Got from other script and allows the left rotation
        mover.Rotate(-turnSpeed);
    }
}
