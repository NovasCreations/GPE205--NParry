using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Gaurd, Chase, Attack, Flee, Patrol };

    public AIState currentState;

    public GameObject target;
    public float triggerDistance;
    public float fleeDistance;

    //patrol variables
    public Transform[] waypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;
    // hearing variable
    public float hearingDistance;
    //vision variables
    public float FieldOfView;
    //public float fleeVector;
    public Transform firePointTransform;

    // Start is called before the first frame update
    public override void Start()
    {
        if (!isHasTarget())
        {
            targetPlayerOne();
        }
        base.Start();
        //currentState = AIState.Gaurd;
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
        if (!isHasTarget())
        {
            Debug.Log("Target Aquried" + target);
            targetPlayerOne();
        }
        base.Update();
    }
    public override void ProcessInputs()
    {
        //this is where the decision making happens
        switch (currentState)
        {
            case AIState.Gaurd:
                // any work that happens for our gaurd
                if (CanSee(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                //any work for chase
                doChaseState();
                if (!CanSee(target))
                {
                    ChangeState(AIState.Gaurd);
                }
                break;
            case AIState.Attack:
                //
                doAttackState();
                break;
            case AIState.Flee:
                doFleeState();
                break;
            case AIState.Patrol:
                doPatrolState();
                break;
        }
    }
    public void doGaurdState()
    {
        //do nothing
    }
    public void doChaseState()
    {
        Seek(target);

    }
    public void doAttackState()
    {
        Seek(target);

        Shoot();
    }
    public void doFleeState()
    {
        Flee();
    }

    public void doPatrolState()
    {
        patrol();
    }




    // call the shoot funcution for the pawn
    public void Shoot()
    {
        pawn.Shoot();
    }
    //set of functions representing the behaviors of the FSM
    public void Seek(GameObject target)
    {
        Seek(target.transform.position);

    }
    public void Seek(Vector3 targetPosition)
    {
        pawn.rotateTowards(targetPosition);

        pawn.MoveForward();
    }
    public void Seek(Transform targetTransform)
    {
        Seek(targetTransform.position);
    }
    public void Seek(Pawn targetPawn)
    {
        Seek(targetPawn.transform);
    }
    public void Flee()
    {
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);

        float percentOfFleeDistance = targetDistance / fleeDistance;

        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);

        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        // find vector to target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        //find the vector from the target bey negating the vectorToTarget
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        //find the vector we would traveldownin order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;

        // seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);

        pawn.rotateTowards(fleeVector);

        pawn.MoveForward(pawn.moveSpeed * flippedPercentOfFleeDistance);
    }
    public void patrol()
    {
        // if we have enough waypoint in our list move to a current waiypoint
        if (waypoints.Length > currentWaypoint)
        {
            // seekk that waypoint

            Seek(waypoints[currentWaypoint]);
            //if we are close enough, then increment the waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint = currentWaypoint + 1;
                Debug.Log("current Waypoint" + currentWaypoint + " waypoint number: " + (currentWaypoint));
            }
        }
        else
        {
            restartPartol();
            Debug.Log("restarted Waypoint List");

        }
    }
    //helper Transititions functions
    public bool isDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void restartPartol()
    {
        currentWaypoint = 0;
    }
    // Change state helper function
    public void ChangeState(AIState state)
    {
        currentState = state;
    }

    public void targetPlayerOne()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.players != null)
            {
                if (GameManager.Instance.players.Count > 0)
                {
                    target = GameManager.Instance.players[0].pawn.gameObject;
                    Debug.Log(target);
                }
            }
        }
    }
    protected bool isHasTarget()
    { return target != null; }

    public bool CanHear(GameObject target)
    {
        Noisemaker noisemaker = target.GetComponent<Noisemaker>();
        if (noisemaker == null)
        {
            return false;
        }
        if (noisemaker.volumeDistance <= 0)
        {
            return false;
        }

        float totalDistance = noisemaker.volumeDistance + hearingDistance;
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            return true;
        }

        return false;
    }
    public bool CanSee(GameObject target)
    {
        Vector3 targetVector = target.transform.position - pawn.transform.position;

        float targetAngle = Vector3.Angle(targetVector, pawn.transform.forward);

        if (targetAngle < FieldOfView)
        {
            Debug.Log("Target = " + target + "Field Of View = " + FieldOfView + "Angle = " + targetAngle +"vector = " + targetVector );
            RaycastHit hit;
            Vector3 rayStart = firePointTransform.position;
            Debug.DrawRay(rayStart, pawn.transform.forward * FieldOfView, Color.red);
            if (Physics.Raycast(rayStart, pawn.transform.forward, out hit))
            {
                
                if (hit.transform.gameObject == target)
                {
                    Debug.Log("can see" + target);
                    return true;
                }
            } 
        }
        Debug.Log("cant see" + target);
        return false;
    }
}


