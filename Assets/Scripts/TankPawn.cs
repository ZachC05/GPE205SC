using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    private float secPerShot;

    public float nextShootTime;
    // Start is called before the first frame update
    public override void Start()
    {
        secPerShot = 1/fireRate;
        nextShootTime = Time.time + secPerShot;
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
    public override void Shoot()
    {
        //sets the fire rate
        if(Time.time >= nextShootTime)
        {
            //Got from other script and allows the Shoot Action
            shooter.Shoot(bullet, bulletForce, damageApplied, bulletLifespan);
            //resets the timer
            nextShootTime = Time.time + secPerShot;
        }

    }

    //Rotates the object, mainly for AI to the position of the target
    public override void RotateTowards(Vector3 TargetPos)
    {
        Vector3 vectorToTarget = TargetPos - transform.position;

        Quaternion targetRot = Quaternion.LookRotation(vectorToTarget.normalized, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }
}
