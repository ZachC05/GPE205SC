using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    // Start is called before the first frame update

    //functios for the tank mover
    public abstract void Start();
    public abstract void Move(Vector3 direction, float speed);
    public abstract void Rotate(float turnSpeed);
    

}
