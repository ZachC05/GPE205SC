using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Get the health component from the gameobject that has been collided with
        Health otherHealth = other.GetComponent<Health>();

        //Only Damage if it does have a health component
        if(otherHealth != null)
        {
            //Procceed with damage
            otherHealth.TakeDamage(damageDone, owner);
        }

        //Destroy ourselfs, because we have collided (can be adjusted to have a bounce effect/peirce effect
        Destroy(gameObject);
    }
}
