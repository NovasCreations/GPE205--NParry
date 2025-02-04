using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject PlayerControllerPrefab;
    public GameObject TankPawnPrefab;

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
    }

    public void Start()
    {
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        GameObject playerObj = Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject pawnObj = Instantiate(TankPawnPrefab, Vector3.zero, Quaternion.identity);

        Controller playerController = playerObj.GetComponent<Controller>();
        Pawn tankPawn = pawnObj.GetComponent<Pawn>();

        playerController.Pawn = tankPawn;

    }



}
