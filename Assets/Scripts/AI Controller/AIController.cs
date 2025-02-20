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
    // Start is called before the first frame update
    public override void Start()
    {
        triggerDistance = guardTriggerDistance;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        

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
                if(IsDistanceLessThan(target, triggerDistance))
                {
                    ChangeState(AIState.Chase);
                }
                //Check for any transitions
                break;
            case AIState.Chase:
                //Set to Chase state
                DoChase();
                if(!IsDistanceLessThan(target, triggerDistance))
                {
                    ChangeState(AIState.Guard);
                }
                else if(IsDistanceLessThan(target, attackTriggerDistance))
                {
                    Debug.Log("Enter Attack State");
                    ChangeState(AIState.Attack);
                }
                //Check for any transitions
                break;
            case AIState.Flee:
                //Set to Flee state
                Flee();
                //Check for any transitions
                break;
            case AIState.Patrol:
                //Set to Patrol state
                Patrol();
                //Check for any transitions
                break;
            case AIState.Attack:
                //Set to Attack state
                DoAttack();
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


    public void Flee()
    {
        //Does Nothing
    }
    public void Patrol()
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pawn.transform.position, triggerDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pawn.transform.position, attackTriggerDistance);
    }
}
