using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{   
    //vairaible for move speed
    public float moveSpeed;
    // variable for move speed
    public float turnSpeed;

    public Mover mover;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();  
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void Moveforward();
    public abstract void Movebackward();
    public abstract void RotateClockwise();
    public abstract void RotateClockwiseReverse();
}
