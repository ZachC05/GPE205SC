using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    [Header("UI")]
    public GameObject HealthUI;
    GameObject uiControl;
    public Vector3 spawnAddition;

    //All the States that the AI can be in
    public enum AIState {Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost};
    [Header("Register Game Controller")]
    public GameControl control;

    //Displays what type the AI is
    [Header("Select AI Type")]
    public bool exploder;
    public bool scout;
    public bool jugg;
    public bool snipe;

    [Header("Scout Special Stat")]
    public float secPerShot;
    public float shotDamage;
    public float shotSpeed;

    [Header("Scout Special Stat")]
    public GameObject[] ActiveScouts;
    public bool scoutCanSee;
    public bool thisScoutSees;

    //Selectable State
    [Header("Select Starting State")]
    public AIState currentState;

    public GameObject target;

    [Header("AI Stats")]
    public float triggerDistance;
    public float chaseTriggerDistance;
    public float guardTriggerDistance;
    public float attackTriggerDistance;
    public float fleeDistance;

    [Header("Hearing Stats")]
    public float hearingRadius;


    [Header("Seeing Stats")]
    public float feildOfView;

    [Header("AI Waypoints Stats")]
    public float waypointStopDistance;
    public Transform[] waypoints;

    public int currentWaypoint;

    Health health;

    [Header("Spawning a Starting Tank")]
    public GameObject TankPawn;
    GameObject spawnedPawn;
    // Start is called before the first frame update
    public override void Start()
    {
        GameControl.instance.AI.Add(this);
        control = GameObject.Find("GamemodeControl").GetComponent<GameControl>();
        GameObject newPawn = Instantiate(TankPawn, transform.position, transform.rotation);
        spawnedPawn = newPawn;
        pawn = newPawn.GetComponent<Pawn>();
        health = pawn.GetComponent<Health>();
        triggerDistance = guardTriggerDistance;
        Vector3 newSpawnPosition = new Vector3(transform.position.x, transform.position.y + spawnAddition.y, transform.position.z);
        Vector3 spawnRotation = new Vector3(90, 0, 0);
        uiControl = Instantiate(HealthUI, spawnAddition, Quaternion.Euler(spawnRotation));
        uiControl.GetComponent<HPFollowAI>().target = spawnedPawn;
        base.Start();
        if (snipe)
        {
            pawn.damageApplied = shotDamage;
            pawn.bulletForce = shotSpeed;
            pawn.secPerShot = secPerShot;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!HasTarget())
        {
            TargetPlayerOne();
        }
        else
        {
            //nothing
        }
        if (thisScoutSees && scout)
        {
            control = GameObject.Find("GamemodeControl").GetComponent<GameControl>();
            if(control != null)
            {
                control.playerSeenByScout = true;
            }
            else
            {
                Debug.Log("Unable to get game control");
            }
        }
        else if(thisScoutSees && scout)
        {
            control.playerSeenByScout = false;
        }
        if(pawn != null && target != null)
        {
            GetInputs();
        }
        

        base.Update();
    }
    public override void GetInputs()
    {
        //This is where the decision making will happen
        switch (currentState)
        {
            case AIState.Guard:
                //Set to guard state
                DoGuard();
                //Check for any transitions
                if (jugg)
                {
                    if (CanSee(target) || CanHear(target))
                    {
                        ChangeState(AIState.Attack);
                    }
                }
                else
                {
                    ChangeState(AIState.Patrol);
                }
                break;
            case AIState.Chase:
                //Set to Chase state
                DoChase();
                //Check for any transitions
                if (jugg)
                {
                    if (!CanHear(target) && !CanSee(target) && jugg)
                    {
                        ChangeState(AIState.Guard);
                    }
                    else if (!CanHear(target) && !CanSee(target) && !jugg)
                    {
                        ChangeState(AIState.Guard);
                    }
                }
                else if (exploder)
                {
                    //Do Nothing BUT chase and start a time for destroying itself
                }
                
                break;
            case AIState.Flee:
                //Set to Flee state
                DoFlee();
                //Check for any transitions
                if (health.currentHealth >= health.maxHealth / 2)
                {
                    Debug.Log("Exit Flee State");
                    ChangeState(AIState.Guard);
                }
                break;
            case AIState.Patrol:
                //Set to Patrol state
                DoPatrol();
                if (exploder)
                {
                    if (CanSee(target) || CanHear(target) || scoutCanSee)
                    {
                        ChangeState(AIState.Chase);
                    }
                } 
                else if (scout)
                {
                    if(CanHear(target) || CanSee(target))
                    {
                        thisScoutSees = true;
                        ChangeState(AIState.Attack);
                    }
                }
                else if (snipe)
                {
                    if (CanHear(target) || CanSee(target) || scoutCanSee)
                    {
                        ChangeState(AIState.Attack);
                    }
                }
                //Check for any transitions
                break;
            case AIState.Attack:
                //Set to Attack state
                DoAttack();
                //Check for any transitions
                if (scout)
                {
                    if (!CanHear(target) && !CanSee(target))
                    {
                        thisScoutSees = false;
                        ChangeState(AIState.Patrol);
                    }
                }
                else if (snipe)
                {
                    if (!CanHear(target) && !CanSee(target) && !scoutCanSee)
                    {
                        ChangeState(AIState.Patrol);
                    }
                }
                break;
            case AIState.Scan:
                //Set to Scan state
                Scan();
                //Check for any transitions
                break;
            case AIState.BackToPost:
                //Set to Go Back To Post state
                BackToPost();
                //Check for any transitions
                break;
        }
        
    }
    public void DoGuard()
    {
        triggerDistance = guardTriggerDistance;
    }


    public void DoChase()
    {
        if (exploder)
        {
            pawn.moveSpeed = 18;
        }
        //Chases the Player
        Seek(target.transform.position);
        triggerDistance = chaseTriggerDistance;
    }

    public void DoAttack()
    {
        if (!jugg && !snipe)
        {
            Seek(target.transform.position);
        }
        else
        {
            pawn.RotateTowards(target.transform.position);
        }


        pawn.Shoot();
    }


    public void DoFlee()
    {
        Flee();
    }
    public void DoPatrol()
    {
        Patrol();
    }

    public void Scan()
    {
        //Does Nothing
    }
    public void BackToPost()
    {
        //Does Nothing
    }

    //Functions representing the behaviors

    //Seek
    public void Seek(Vector3 target)
    {
        pawn.RotateTowards(target);

        pawn.MoveForward();
    }
    
    public void Seek(Transform target)
    {
        Seek(target.position);
    }
    
    public void Seek(GameObject target)
    {
        Seek(target.transform.position);
    }
    public void Seek(Pawn target)
    {
        Seek(target.transform.position);
    }
    

    //Flee
    public void Flee()
    {
        /*
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);

        float percentOfFleeDistance = targetDistance / fleeDistance;

        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);

        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;
        */
        //Find the vector of the target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        //Find the vector away from the target by subtracting the vectorToTarget
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // find the vector we would travel down in ordewr to flee(run away in opposite direction
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;// * flippedPercentOfFleeDistance;
        // Seel the ppoint that is "fleeVector" away from our current position

        Seek(pawn.transform.position + fleeVector);

        //Not Working Code
        /*
        pawn.RotateTowards(fleeVector);

        pawn.MoveForward(pawn.moveSpeed * flippedPercentOfFleeDistance);
        */
    }


    //Patrol
    protected void Patrol()
    {
        if(waypoints.Length > currentWaypoint)
        {
            //Seek to the waypoint
            Seek(waypoints[currentWaypoint]);
            //IF AI is close enoug to the waypoint, then go to next one
            if(Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }       
        }
        else
        {
            RestartPatrol();
        }
    }
    //Restarts the patrol waypoints
    protected void RestartPatrol()
    {
        //Set the index of the waypoints to 0
        currentWaypoint = 0;
    }


    //Transistion Functions
    public bool IsDistanceLessThan(GameObject target, float distance)
    {
        //Gets distance and returns a value if the distance is greater than the required distance
        if(Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else

        {
            return false;
        }
    }

    //Change the AI State
    public void ChangeState(AIState state)
    {
        currentState = state;
    }

    //Targets the first player seen
    public void TargetPlayerOne()
    {
        //Check if game manager exists
        if(GameControl.instance != null)
        {
            //check if players exists
            if(GameControl.instance.players != null)
            {
                //Check if players are in it
                if(GameControl.instance.players.Count > 0)
                {
                    //Set the target object of the first player in the array
                    target = GameControl.instance.players[0].pawn.gameObject;
                }
            }
        }
    }

    //If it does obtain a target
    protected bool HasTarget()
    {
        //return true if we do have a target, false if we dont
        return (target != null);
    }

    //Can hear the player
    public bool CanHear(GameObject target)
    {
        //Get the noismaker
        NoiseMaker noismaker = target.GetComponent<NoiseMaker>();
        //if they dont have any noise maker, return false
        if(noismaker == null)
        {
            return false;
        }
        //Making 0 noise and can't be heard, return false
        if( noismaker.noiseDistance <= 0)
        {
            return false;
        }
        //If they are making noise, check to see if they are in radius, if they are not in radius m return false
        float totalDistance = noismaker.noiseDistance + hearingRadius;
        if(Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            //Hear the target
            return true;
        }
        else
        {
            //Cant hear the target
            return false;
        }
        
    }
    
    public bool CanSee(GameObject target)
    {
        //Checking if the pawn can see the target in its field of view
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        float angleToTarget = Vector3.Angle(vectorToTarget, pawn.transform.position);

        if (angleToTarget < feildOfView)
        {
            return checkRaycast();
            
        }
        return false;
    }

    public bool checkRaycast()
    {
        //Checking Raycast
        RaycastHit hit;

        if (Physics.Raycast(pawn.transform.position, pawn.transform.forward, out hit))
        {
            if (hit.transform.gameObject == target)
            {

                
                return true;
            }
        }
        return false;
    }

    //Visual distance representation
    public void OnDrawGizmos()
    {
        if(pawn != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pawn.transform.position, triggerDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pawn.transform.position, attackTriggerDistance);

            //Hearing Radius
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(pawn.transform.position, hearingRadius);

        }

    }

    public void DestroyEverything()
    {
        if(TankPawn != null)
        {
            Destroy(spawnedPawn);
        }

        Destroy(gameObject);
    }

    public override void UpdatePersonalUI()
    {
        //there is no UI
    }

    public override void AddPoints(int pointsAmount)
    {
        //needs no points
    }

    public override void RemovePoints(int pointsAmount)
    {
        //needs no points
    }
    public override void RemvoeLives()
    {
        //doesnt have lives
    }

    public override void ResapawnPlayer()
    {
        //doesnt need to respawn;
    }
}
