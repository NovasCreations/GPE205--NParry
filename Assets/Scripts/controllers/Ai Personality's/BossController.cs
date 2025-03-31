using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AIController 
{
    //public float WaypointRadius;
    //public float numberOfWayPoint;
    
    private Health Health;
    private float maxHealth;
    private float CurrentHealth;
    private float halfHealth;
    // Start is called before the first frame update
    public override void Start()
    {

        Health health = pawn.GetComponent<Health>();
            maxHealth = health.maxHealth;
        CurrentHealth = health.currentHealth;
        halfHealth = maxHealth * 0.5f;
        
        base.Start();
    } 
   
    // Update is called once per frame
    public override void Update()
    {
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
                    { ChangeState(AIState.Attack); }
                }
                else if (IsHalfHealth(CurrentHealth,halfHealth))
                {
                    belowhalfHealth();
                    doGaurdState();
                }
                if (IsAvoidingObstacles)
                {
                    ChangeState(AIState.Avoid);
                }
                break;
            case AIState.Attack:
                if (!IsInsideShootRange(target, triggerDistance,fleeDistance))
                {
                    ChangeState(AIState.Guard);
                }
                else if (IsHalfHealth(CurrentHealth, halfHealth))
                {
                    belowhalfHealth();
                    doAttackState();
                }
                if (isDistanceLessThan(target,fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }    
                doAttackState();
                break;
            case AIState.Flee:
                {
                    if (IsDistanceGreaterThan(target, triggerDistance))
                    {
                        ChangeState(AIState.Guard); 
                    }
                    
                    break;
                }
        }
        base.ProcessInputs();
    }

    public void belowhalfHealth()
    {
        fleeDistance = fleeDistance * 2;
        triggerDistance = triggerDistance * 2;
        pawn.damageDone = pawn.damageDone * 2;
    }
    public bool IsHalfHealth(float CurrentHealth, float halfHealth)
    {
        if (CurrentHealth < halfHealth)
        {
            return true;
        }
        return false;
    }
    //public List<Transform> CreateGuardTransforms(Transform GuardOrgin)
    //{
    //    List<Transform> GuardWaypoints = new List<Transform>();
    //    float radius = WaypointRadius;

    //    for (int i = 0; i < numberOfWayPoint; i++)
    //    {
    //        Vector3 randomPosition = GuardOrgin.position + new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));

    //        GameObject Waypoint = new GameObject("Guard Waypoint" + i);
    //        Waypoint.transform.position = randomPosition;
    //        GuardWaypoints.Add(Waypoint.transform);
    //    }
    //    Debug.Log("Guard at ");
    //    return GuardWaypoints;
    //}
}

