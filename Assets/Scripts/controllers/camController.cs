using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player GameObject.
    public GameObject targetPlayer;
    public Pawn pawn;

    // The distance between the camera and the player.
    public Vector3 offset;

    public Vector3 rotationAngles;

    //the rotation smoothing speed
    public float rotationSpeed = 5f;
    public float downwardAngle = 20f;

    // Start is called before the first frame update.
    void Start()
    {
        ////pawn = targetPlayer.GetComponent<Pawn>();
        ////rotationAngles = pawn.mover.GetComponent<Vector3>();

        //if (pawn.CamAnchor != null)
        //{
        //    // Calculate the initial offset between the camera's position and the player's position.
        //    offset = transform.position - pawn.CamAnchor.position;
        //    //transform.rotation = Quaternion.Euler(rotationAngles);
        //}
        //else
        //{
        //    Debug.LogError("cam anchor not assaigned");
        //}
    }

    // LateUpdate is called once per frame after all Update functions have been completed.
    void LateUpdate()
    {
        // Maintain the same offset between the camera and player throughout the game.
        transform.position = pawn.CamAnchor.position + offset;
        // calculate the target rotation based on the pawns forward direction
        Quaternion targetRotation =  Quaternion.Euler(downwardAngle, pawn.transform.eulerAngles.y, 0f);
        //smoothly intrepolate the cameras postition to alighn behind the pawn
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = targetRotation;
    }
}