using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    //A place to hold the rigid body of our component/tank
    private Rigidbody rb;

    //holds the transform of the tank/game object
    private Transform rf;

    // Start is called before the first frame update
    public override void Start()
    {
        //Gets the rigid body of the component at the start of the game/when game is launched
        rb = GetComponent<Rigidbody>();

        //gets the transform of the component
        rf = GetComponent<Transform>();
    }
    public override void Move(Vector3 direction, float speed)
    {
       //Allows movements in a forwards/backwards direction, depending on what is inputed in the speed variable
       Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
    }
    public override void Rotate(float turnSpeed)
    {
        //Allows the rotation of the tank
        rf.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
