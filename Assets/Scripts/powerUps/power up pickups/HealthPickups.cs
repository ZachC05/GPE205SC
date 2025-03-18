using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickups : MonoBehaviour
{
    public HealthPowerup powerup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    public void OnTriggerEnter(Collider other)
    {
        PowerupManager powerManager = other.GetComponent<PowerupManager>();
        if(powerManager != null)
        {
            //Add the powerup
            powerManager.Add(powerup);
            
            //Add effects for saying you collected powerup

            //Destroy Pichup
            Destroy(gameObject);
        }
    }
}
