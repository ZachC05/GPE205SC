using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //locks controls
    public bool LockControls;

    //variable to hold the pawn
    public Pawn pawn;

    [Header("Points")]
    //amount of points the player owns
    public int points;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void AddPoints(int pointsAmount);

    public abstract void RemovePoints(int pointsAmount);

    public abstract void UpdatePersonalUI();
    public abstract void GetInputs();

    public abstract void RemvoeLives();
}
