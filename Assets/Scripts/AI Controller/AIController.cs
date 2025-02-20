using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    //All the States that the AI can be in
    public enum AIState {Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost};

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

    [Header("AI Waypoints Stats")]
    public float waypointStopDistance;
    public Transform[] waypoints;

    private int currentWaypoint = 0;

    Health health;

    [Header("Spawning a Starting Tank")]
    public GameObject TankPawn;
    // Start is called before the first frame update
    public override void Start()
    {
        GameObject newPawn = Instantiate(TankPawn, transform.position, transform.rotation);
        pawn = newPawn.GetComponent<Pawn>();
        health = pawn.GetComponent<Health>();
        triggerDistance = guardTriggerDistance;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!HasTarget())
        {
            TargetPlayerOne();
        }

        GetInputs();

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
                if (IsDistanceLessThan(target, triggerDistance))
                {
                    ChangeState(AIState.Chase);
                }
                else if (health.currentHealth < health.maxHealth / 2)
                {
                    Debug.Log("Enter Flee State");
                    ChangeState(AIState.Flee);
                }

                break;
            case AIState.Chase:
                //Set to Chase state
                DoChase();
                //Check for any transitions
                if (!IsDistanceLessThan(target, triggerDistance))
                {
                    ChangeState(AIState.Guard);
                }
                else if(IsDistanceLessThan(target, attackTriggerDistance))
                {
                    Debug.Log("Enter Attack State");
                    ChangeState(AIState.Attack);
                }
                else if (health.currentHealth < health.maxHealth / 2 + 1)
                {
                    Debug.Log("Enter Flee State");
                    ChangeState(AIState.Flee);
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
                //Check for any transitions
                break;
            case AIState.Attack:
                //Set to Attack state
                DoAttack();
                //Check for any transitions
                if (!IsDistanceLessThan(target, attackTriggerDistance))
                {
                    Debug.Log("Left Attack State");
                    ChangeState(AIState.Chase);
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
        //Chases the Player
        Seek(target);
        triggerDistance = chaseTriggerDistance;
    }

    public void DoAttack()
    {
        Seek(target);

        pawn.Shoot();
        //checks if the player is in the attack range then starts following and shooting

    }


    public void DoFlee()
    {
        Flee();
    }
    public void DoPatrol()
    {
        //Does Nothing
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
        Seek(target);
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
    public void Patrol()
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
            else
            {
                RestartPatrol();
            }
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


    //Visual distance representation
    public void OnDrawGizmos()
    {
        if(pawn != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pawn.transform.position, triggerDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pawn.transform.position, attackTriggerDistance);

        }

    }
}
