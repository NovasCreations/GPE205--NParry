using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AIController : Controller
{
    public enum AIState { Guard, Chase, Attack, Flee, Patrol, Avoid };

    public AIState currentState;

    public GameObject target;
    public float triggerDistance;
    public float fleeDistance;
    public float guardWaitTime;


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
    private Transform HitTransform;
    public float rayDistance;
    public bool IsAvoidingObstacles = false;
    private RaycastHit obstacle;

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
        if (!isHasTarget())
        {
            Debug.Log("Target Aquried" + target);
            targetPlayerOne();
        }
        ProcessInputs();



        base.Update();
    }
    public override void ProcessInputs()
    {
        //this is where the decision making happens
        switch (currentState)
        {
            case AIState.Guard:
                // any work that happens for our gaurd
                if (CanSee(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                //any work for chase
                doChaseState();
                //if (!CanSee(target))
                //{
                //    ChangeState(AIState.Guard);
                //}
                break;
            case AIState.Attack:

                doAttackState();
                break;
            case AIState.Flee:
                doFleeState();
                break;
            case AIState.Patrol:
                doPatrolState();
                break;
            case AIState.Avoid:
                if (IsAvoidingObstacles)
                {
                    doAvoidState();
                }
                else
                {
                    ChangeState(AIState.Chase);
                }
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
        pawn.rotateTowards(target.transform.position);

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
    public void doAvoidState()
    {
        AvoidObstacles();
    }




    // call the shoot funcution for the pawn
    public void Shoot()
    {
        //Seek(target.transform.position);
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

    public void AvoidObstacles()
    {
        Vector3 direction = firePointTransform.transform.forward;
        RaycastHit hit;
        bool raycast = Physics.Raycast(firePointTransform.transform.position, direction * rayDistance, out hit, rayDistance);

        Debug.DrawRay(firePointTransform.transform.position, direction * rayDistance, Color.red);
        if (raycast)
        {

            Vector3 avoidDirection = Vector3.Reflect(direction, hit.normal);
            pawn.rotateTowards(avoidDirection);
            Seek(pawn.transform.position + avoidDirection * rayDistance);
            if (raycast)
            {
                IsAvoidingObstacles = true;
            }
            else
            {
                IsAvoidingObstacles = false;
            }    
        }
        else
        {
            IsAvoidingObstacles = false;


        }
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
    public bool IsDistanceGreaterThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) > distance)
        { return true; }
        return false;
    }
    public bool IsInsideShootRange(GameObject target, float RangeStart, float RangeEnd)
    {
        if (IsDistanceGreaterThan(target, RangeStart) && isDistanceLessThan(target, RangeEnd))
        {
            return true;
        }
        return false;

    }

    public void restartPartol()
    {
        currentWaypoint = 0;
    }
    // Change state helper function
    public void ChangeState(AIState state)
    {
        currentState = state;
        Debug.Log("State Changed to " + currentState);
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
        if (target == null)
        {
            return false;
        }

        Vector3 targetVector = target.transform.position - pawn.transform.position;

        float targetAngle = Vector3.Angle(targetVector, pawn.transform.forward);

        if (targetAngle < FieldOfView)
        {
            //Debug.Log("Target = " + target + "Field Of View = " + FieldOfView + "Angle = " + targetAngle +"vector = " + targetVector );
            RaycastHit hit;

            Vector3 rayStart = firePointTransform.position;
            Debug.DrawRay(rayStart, pawn.transform.forward * rayDistance, Color.red);
            bool rayHit = Physics.Raycast(rayStart, pawn.transform.transform.forward, out hit, rayDistance);
            //HitTransform = hit.collider.transform;
            if (rayHit)
            {

                if (hit.transform.gameObject == target)
                {
                    Debug.Log("can see" + target);
                    return true;
                }
                else
                {
                    IsAvoidingObstacles = true;
                    Debug.Log("looking at obstacle:" + hit.transform.gameObject.name);
                }

            }
        }
        //Debug.Log("cant see" + target);
        return false;
    }
    public List<Transform> CreateGuardTransforms(Transform GuardOrgin, float radius, float numberOfWaypoint )
    {
        List<Transform> GuardWaypoints = new List<Transform>();
        

        for (int i = 0; i < numberOfWaypoint; i++)
        {
            Vector3 randomPosition = GuardOrgin.position + new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));

            GameObject Waypoint = new GameObject("Guard Waypoint" + i);
            Waypoint.transform.position = randomPosition;
            GuardWaypoints.Add(Waypoint.transform);
        }
        Debug.Log("Guard at ");
        return GuardWaypoints;
    }
}


