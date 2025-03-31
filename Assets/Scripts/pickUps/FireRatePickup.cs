using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePickup : MonoBehaviour
{
    public FireRatePowerup powerup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        PowerUpManager powerUpManager = other.GetComponent<PowerUpManager>();
        if (powerUpManager != null)
        {
            powerUpManager.Add(powerup);

            Destroy(gameObject);
        }

    }
}
