using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    //Speed
    [Header("Speed")]
    //Move speed Variable (forward and backwards movement)
    public float moveSpeed;

    //Turn SPeed Variable (turning left and right)
    public float turnSpeed;

    [Header("Gets the Mover")]
    public Mover mover;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForward();

    public abstract void MoveBackward();

    public abstract void RotateRight();

    public abstract void RotateLeft();
}
