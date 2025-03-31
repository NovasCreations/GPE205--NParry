using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Attacker : AIController
{
    public float WaitTime;
    public float numberOfWaypoint;
    public float WaypointRadius;

    private float guardTime;
    private float timeToWait;
    
    private float distanceToTarget;
    private Vector3 spawnPosition;
    private Transform spawnTransform;

    // Start is called before the first frame update
    public override void Start()
    {
        timeToWait = Time.time + WaitTime;
        guardTime = Time.time + guardWaitTime;
        spawnPosition = gameObject.transform.position;
        spawnTransform = gameObject.transform;
        waypoints = CreateGuardTransforms(spawnTransform,WaypointRadius, numberOfWaypoint).ToArray();
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        base.Update();
    }

    public override void ProcessInputs()
    {
        switch (currentState)
        {
            case AIState.Guard:

                if (CanHear(target))
                {
                    pawn.rotateTowards(target.transform.position);
                    if (CanSee(target)) 
                    {
                        ChangeState(AIState.Chase);
                    }

                }
                else if (Time.time > timeToWait)
                {
                    timeToWait = Time.time + WaitTime;
                    Debug.Log("Time Elapsed switching to Patrol");
                    ChangeState(AIState.Patrol);
                }

                break;
            case AIState.Attack:
                if (!IsInsideShootRange(target, fleeDistance, triggerDistance) || !CanSee(target))
                {
                    ChangeState(AIState.Chase);
                }
                doAttackState();
                break;

            case AIState.Chase:
                if (IsAvoidingObstacles)
                {
                    ChangeState(AIState.Avoid);
                }
                if (IsInsideShootRange(target, fleeDistance, triggerDistance))
                {
                    ChangeState(AIState.Attack);
                }
                if (!CanSee(target) && guardTime < Time.time)
                {
                    guardTime = Time.time + guardWaitTime;
                    ChangeState(AIState.Guard);
                }
                if (isDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                doChaseState();

                break;
            case AIState.Patrol:
                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                if (IsAvoidingObstacles)
                {
                    ChangeState(AIState.Avoid);
                }
                doPatrolState();
                break;
            case AIState.Avoid:
                if (!IsAvoidingObstacles || CanSee(target) || CanHear(target))
                {  
                    ChangeState(AIState.Chase); 
                }
                doAvoidState();
                Debug.Log("avoiding obstacles...");
                
                    break;
            case AIState.Flee:
                {
                    if(distanceToTarget > fleeDistance && !CanSee(target))
                    {
                        ChangeState(AIState.Chase);
                    }
                    doFleeState();

                }
                break;
        }
        base.ProcessInputs();
    }
}        


