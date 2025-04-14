using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public int pawnWorth;

    Controller playerController;
    TankPawn owner;

    public AudioSource deathSFX;
    public AudioSource hitSFX;

    // Start is called before the first frame update
    void Start()
    {
        //Sets Health to be equal to max health at start
        currentHealth = maxHealth;
        owner = GetComponent<TankPawn>();
        playerController = owner.owner;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amout, Pawn source)
    {
        hitSFX.Play();
        //Takes damage
        currentHealth -= amout;
        Debug.Log(source.name + " Did " + amout + " damage to " + gameObject.name);

        //Returns a value and can't go below 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //if the max health is less than or equal to 0, destroy this gameobject
        if(currentHealth <= 0)
        {
            Die(source);
        }
    }
    public void GetHealth(float amout, Pawn source)
    {
        //Health Damage
        currentHealth += amout;
        Debug.Log(source.name + " Healed " + amout + " health to " + gameObject.name);

        //Can't go above maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    public void Die(Pawn source)
    {
        deathSFX.Play();
        Debug.Log(source.name + " Killed " + gameObject.name);

        source.owner.AddPoints(pawnWorth);
        if (playerController != null)
        {
            
            if (playerController.lives > 0)
            {

                
                owner.owner.RemvoeLives();
                GetHealth(20, source);
                transform.position = GameControl.instance.spawnPos.transform.position;


            }
            else
            {
                deathSFX.Play();
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
