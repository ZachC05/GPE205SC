using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    public Transform firepoint;
    public AudioSource shootSFX;

    //On start of the game
    public override void Start()
    {
        shootSFX = GetComponent<AudioSource>();
    }
    public override void Update()
    {

    }

    public override void Shoot(GameObject BulletPrefab, float force, float damage, float lifespan)
    {
        shootSFX.Play();
        //Spawns the bullet
        GameObject newshell = Instantiate(BulletPrefab, firepoint.transform.position, firepoint.transform.rotation);

        //Gets the Damage script
        DamageOnHit doh = newshell.GetComponent<DamageOnHit>();

        //Checks if it exists and applies variables
        if(doh != null)
        {
            doh.damageDone = damage;

            doh.owner = GetComponent<Pawn>();
        }
        // get rigid body of the bullet
        Rigidbody rb = newshell.GetComponent<Rigidbody>();
        //checks if it exists
        if(rb != null)
        {
            //apply a force to reigid body
            rb.AddForce(firepoint.forward * force);
        }

        Destroy(newshell, lifespan);
    }
}
