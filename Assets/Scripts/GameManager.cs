using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public float numberOfAIEnemies;
    public static GameManager Instance;
    public int mapSeed;
    public bool isMapOfDay;
    public bool isRandom;

    public GameObject AIEnemy;
    public GameObject PlayerControllerPrefab;
    public GameObject TankPawnPrefab;

    
    public Camera Camera;
    
    public Transform CamAnchor;
    
    public MapGenerator mapGenerator;


    public List<AIController> AIControllers;
    public List<PlayerControler> players;
    public List<Controller> Controllers;
    

    private AIPawnSpawner[] AIPawnSpawners;
    
    private TankPawnSpawner[] tankPawnSpawners;


    // Start is called before the first frame update
    private void Awake()
    {
        if (isMapOfDay)
        {
            UnityEngine.Random.InitState(DateToInt(DateTime.Now.Date));
        }
        else if (isRandom)
        {
            isMapOfDay = false;
            UnityEngine.Random.InitState(DateToInt(DateTime.Now));

        }
        else
        {
            UnityEngine.Random.InitState(mapSeed);
        }

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
        Controllers = new List<Controller>();
        //AIControllers = new 

    }

    public void Start()
    {
        if (mapGenerator != null)
        {
            mapGenerator.generateMap();
            if (players.Count == 0)
            {
                tankPawnSpawners = FindObjectsByType<TankPawnSpawner>(FindObjectsSortMode.None);
                AIPawnSpawners = FindObjectsByType<AIPawnSpawner>(FindObjectsSortMode.None).ToArray();

                SpawnPlayer();
                spawnAIEnemy();
            }
           
            
                
            
        }
    }

    public void SpawnPlayer()
    {
        if ( tankPawnSpawners != null)
        {
            if (tankPawnSpawners.Length > 0)
            {
                Transform tankPawnSpawnPoint = tankPawnSpawners[UnityEngine.Random.Range(0,tankPawnSpawners.Length)].transform;
                GameObject playerObj = Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
                GameObject pawnObj = Instantiate(TankPawnPrefab, tankPawnSpawnPoint.position, Quaternion.identity);
                Camera camera = Camera.main;
                CameraController cameraController = camera.GetComponent<CameraController>();
                Controller playerController = playerObj.GetComponent<Controller>();
                Pawn tankPawn = pawnObj.GetComponent<Pawn>();
                Controllers.Add(playerController);
                if (cameraController == null || cameraController.targetPlayer == null)
                {
                    cameraController.targetPlayer = pawnObj;
                    cameraController.pawn = tankPawn;
                    cameraController.pawn.CamAnchor = tankPawn.CamAnchor;
                }
             

                playerController.pawn = tankPawn;
            }
        }
        
        //controller.target = pawnObj;

    }
     

    public void spawnAIEnemy()
    {
       
        //make sure that the controller and spawn lists are not empty
        if (AIControllers.Count == 0 || AIPawnSpawners.Length == 0)
        {
            Debug.Log("No AI Spawns or Controllers available");
        }
        List<Transform> AISpawnsAvailable = new List<Transform>();
        List<AIController> AIControllersAvailable = new List<AIController>();
        if (AIPawnSpawners != null)
        {
           
            
            for (int i = 0; i < numberOfAIEnemies; i++)
            {
                if (AISpawnsAvailable.Count == 0)
                {
                    foreach (AIPawnSpawner spawner in AIPawnSpawners)
                    {
                        AISpawnsAvailable.Add(spawner.transform);
                    }
                }
                if (AIControllersAvailable.Count == 0)
                {
                    foreach (AIController controller in AIControllers)
                    {
                        AIControllersAvailable.Add(controller);
                    }
                }
                int spawnIndex = UnityEngine.Random.Range(0, AISpawnsAvailable.Count);
                Transform randomSpawnPoint = AISpawnsAvailable[spawnIndex];
                AISpawnsAvailable.RemoveAt(spawnIndex);

                int controllerIndex = UnityEngine.Random.Range(0, AIControllersAvailable.Count);
                AIController RandomAIpersonality = AIControllersAvailable[controllerIndex];
                AIControllersAvailable.RemoveAt(controllerIndex);

                GameObject AIPawn = Instantiate(AIEnemy, randomSpawnPoint.position, randomSpawnPoint.rotation);
                TankShoter shoter = AIPawn.GetComponent<TankShoter>();
                Transform firePointTransform = shoter.FirePointTransform;
                AIController AIPawnController = Instantiate(RandomAIpersonality, randomSpawnPoint.position, randomSpawnPoint.rotation);
                Pawn tankPawn = AIPawn.GetComponent<Pawn>();
                TankPawn TankPawn = tankPawn.GetComponent<TankPawn>();

                if (tankPawn != null)
                {

                    AIPawnController.pawn = tankPawn;
                    AIPawnController.firePointTransform = firePointTransform;

                    Controllers.Add(AIPawnController);
                        
                    Destroy(randomSpawnPoint.gameObject);




                }
                    
            }
        }
    }
    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    

}
    

