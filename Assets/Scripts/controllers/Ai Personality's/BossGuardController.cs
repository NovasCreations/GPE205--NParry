using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossGuard : AIController
{
    public float WaitTime;
    public GameObject Boss;
    private GameManager instance;
    public float numberOfWayPoint;
    public float WaypointRadius;

    private float guardTime;
    private float timeToWait;

    private float distanceToTarget;

    private Vector3 spawnPosition;
    // Start is called before the first frame update
    public override void Start()
    {
        instance = GameManager.Instance;
        
        timeToWait = Time.time + WaitTime;
        guardTime = Time.time + guardWaitTime;
        if (instance.Controllers != null)
        {
            foreach (var controller in instance.Controllers)
            {
                if (controller.GetComponent<BossController>() != null )
                {
                    Boss = controller.gameObject;
                    waypoints = CreateGuardTransforms(Boss.transform, WaypointRadius).ToArray();
                }
                else 
                {
                    Debug.Log("no boss assigned");
                }
            }
            base.Start();
        }
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
                doGaurdState();

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
                    ChangeState(AIState.Guard);
                }
                if (IsAvoidingObstacles)
                {
                    ChangeState(AIState.Avoid);
                }
                
                if(IsDistanceGreaterThan(Boss, fleeDistance)) 
                {
                    CreateGuardTransforms(Boss.transform, WaypointRadius);
                }
                doPatrolState();
                break;
            case AIState.Avoid:
                if (!IsAvoidingObstacles || CanSee(target) )
                {
                    ChangeState(AIState.Chase);
                }
                doAvoidState();
                Debug.Log("avoiding obstacles...");

                break;
            case AIState.Flee:
                {
                    if (distanceToTarget > fleeDistance && !CanSee(target))
                    {
                        ChangeState(AIState.Chase);
                    }
                    doFleeState();

                }
                break;
        }
        base.ProcessInputs();
    }
    public List<Transform> CreateGuardTransforms(Transform GuardOrgin, float radius)
    {
        List<Transform> GuardWaypoints = new List<Transform>();
        
        for (int i = 0; i < numberOfWayPoint; i++)
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




