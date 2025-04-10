using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPFollowAI : MonoBehaviour
{
    public GameObject target;
    public Vector3 followVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        followVector.x = target.transform.position.x;
        followVector.z = target.transform.position.z;
        transform.position = followVector;

    }
}
