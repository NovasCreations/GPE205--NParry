using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwise;
    public KeyCode rotateCounterClockwise;
    public KeyCode shootkey;

    public float volumeDistance;

    // Start is called before the first frame update
    public override void Start()
    {
        //verify only one instance of game manager
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.players != null)
            {
                GameManager.Instance.players.Add(this);

            }
        }
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
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
            pawn.noisemaker.volumeDistance = volumeDistance;
        }
        if (Input.GetKeyUp(moveForwardKey))
        {
            pawn.noisemaker.volumeDistance = 0;
        }
        if (Input.GetKey(moveBackwardKey))
        {
            pawn.Movebackward();
            pawn.noisemaker.volumeDistance = volumeDistance;
        }
        if (Input.GetKeyUp(moveBackwardKey))
        {
            pawn.noisemaker.volumeDistance = 0;
        }
        if (Input.GetKey(rotateClockwise))
        {
            pawn.RotateClockwise();
            pawn.noisemaker.volumeDistance = volumeDistance;
        }
        if (Input.GetKeyUp(rotateClockwise))
        {
            pawn.noisemaker.volumeDistance = 0;
        }
        if (Input.GetKey(rotateCounterClockwise))
        {
            pawn.RotateClockwiseReverse();
            pawn.noisemaker.volumeDistance = volumeDistance;
        }
        if (Input.GetKeyUp(rotateCounterClockwise))
        {
            pawn.noisemaker.volumeDistance = 0;
        }
        if (Input.GetKeyDown(shootkey))
        {
            pawn.Shoot();
            pawn.noisemaker.volumeDistance = volumeDistance;
            pawn.noisemaker.volumeDistance = 0;
        }
    }
    public void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.players != null)
            {
                GameManager.Instance.players.Remove(this);
            }
        }
    }
}
