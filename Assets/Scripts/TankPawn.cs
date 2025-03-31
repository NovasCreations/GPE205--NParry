using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    private float secondsPerShot;
    
    private float nextShootTime;


    // Start is called before the first frame update
    public override void Start()
    {
        

        nextShootTime =  Time.time + secondsPerShot;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Start();

    }

    public override void MoveForward()
    {
        //Debug.Log("Move Forward");
        mover.Move(transform.forward, moveSpeed);
    }
    public override void MoveForward(float speed)
    {
        mover.Move(transform.position, speed);
    }

    public override void Movebackward()
    {

        Debug.Log("Move Backward");
        mover.Move(transform.forward, -moveSpeed);

    }
    public override void RotateClockwise()
    {
        Debug.Log("Rotate Clockwise");
        mover.Rotate(turnSpeed);
    }

    public override void RotateClockwiseReverse()
    {
        Debug.Log("Rotate Counter-Clockwise");
        mover.Rotate(-turnSpeed);

    }

    public override void Shoot()
    {
        secondsPerShot = 1 / fireRate;
        if (Time.time >= nextShootTime)
        {

            shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifespan);
            nextShootTime = Time.time + secondsPerShot;
        }
    }

    public override void rotateTowards(Vector3 targetPosition)
    {
        Vector3 vectorToTarget = targetPosition - transform.position;

        Quaternion targetRotation =  Quaternion.LookRotation(vectorToTarget, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
