using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AIEnemy;
    public GameObject PlayerControllerPrefab;
    public GameObject TankPawnPrefab;
    public Camera Camera;
    public Transform CamAnchor;
    public Transform tankPawnSpawner;
    public AIController Controller;
    public List<PlayerControler> players;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        players = new List<PlayerControler>();
        
    }

    public void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject playerObj = Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject pawnObj = Instantiate(TankPawnPrefab, tankPawnSpawner.position, Quaternion.identity);
        AIController controller = Controller;
        Camera camera = Camera.main;
        CameraController cameraController = camera.GetComponent<CameraController>();
        Controller playerController = playerObj.GetComponent<Controller>();
        Pawn tankPawn = pawnObj.GetComponent<Pawn>();
        if (cameraController == null || cameraController.targetPlayer == null)
            {
            cameraController.targetPlayer = pawnObj;
            cameraController.pawn = tankPawn;
            cameraController.pawn.CamAnchor = tankPawn.CamAnchor;
        }
        else
            {
                Debug.Log(cameraController.targetPlayer);
            }

        playerController.pawn = tankPawn;
        controller.target = pawnObj;
        
    }
}
