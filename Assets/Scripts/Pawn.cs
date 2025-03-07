using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{   // mover variables
    //vairaible for move speed
    public float moveSpeed;
    // variable for move speed
    public float turnSpeed;
    public Mover mover;

    //shooter variables
    //variable for shell prefab
    public GameObject shellPrefab;
    //variable for firing force
    public float fireForce;
    //variable for damage done
    public float damageDone;
    // variable for how long the bullet will fly if the doent collide 
    public float shellLifespan;
    // variable for rate of fire
    public float fireRate;
    public Transform CamAnchor;

    public Shooter shooter;
    
    public Noisemaker noisemaker;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>(); 
        
        shooter = GetComponent<Shooter>();

        noisemaker = GetComponent<Noisemaker>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForward();
    public abstract void MoveForward(float speed);
    public abstract void Movebackward();
    public abstract void RotateClockwise();
    public abstract void RotateClockwiseReverse();

    public abstract void Shoot();
    public abstract void rotateTowards(Vector3 targetPosition);
}
