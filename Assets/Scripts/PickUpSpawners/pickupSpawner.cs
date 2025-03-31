using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupSpawner : MonoBehaviour
{
    
    public GameObject[] pickupPrefabs;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform spawnTransform;
    private GameObject pickupprefab;
    private GameObject spawnedPickup;
    // Start is called before the first frame update
    void Start()
    {
        spawnTransform =  GetComponent<Transform>();
        nextSpawnTime = Time.time+spawnDelay;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedPickup == null)
        {
            if (Time.time > nextSpawnTime)
            {
                GameObject nextPickup = pickupPrefabs[Random.Range(0,pickupPrefabs.Length)];
                spawnedPickup = Instantiate(nextPickup, spawnTransform.position, Quaternion.identity);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
