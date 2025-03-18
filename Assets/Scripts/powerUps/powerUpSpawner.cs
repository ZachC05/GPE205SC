using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpSpawner : MonoBehaviour
{
    private GameObject spawnPickupExists;
    public GameObject powerUpPrefab;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform rf;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //checks to see if the item already exists
        if(spawnPickupExists == null)
        {
            //if time to spawn a pickup, spawn the pickup
            if (Time.time > nextSpawnTime)
            {
                //spawn the pickup and reset the timer
                spawnPickupExists = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            //if it does already exist, delay the spawn
            nextSpawnTime = Time.time + spawnDelay;
        }

    }
}
