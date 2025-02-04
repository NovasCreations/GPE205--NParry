using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwise;
    public KeyCode rotateCounterClockwise;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
        base.Update();
    }
    public override void ProcessInputs()
    {
        if (Input.GetKeyDown(moveForwardKey))
        {
            Pawn.Moveforward();
        }
        if (Input.GetKeyDown(moveBackwardKey))
        {
            Pawn.Movebackward();
        }
        if (Input.GetKeyDown(rotateClockwise))
        {
            Pawn.RotateClockwise();
        }
        if ( Input.GetKeyDown(rotateCounterClockwise))
        {
            Pawn.RotateClockwiseReverse();
        }
    }
}
