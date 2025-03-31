using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public List<PowerUp> powerUps;
    private List<PowerUp> removedPowerupQueue;
    // Start is called before the first frame update
    void Start()
    {
        powerUps = new List<PowerUp>();
        removedPowerupQueue = new List<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
       DecrementPowerupTimers();
    }

    private void LateUpdate()
    {
        applyRemovePowerupsQueue();
    }

    public void Add(PowerUp PowerUpToAdd)
    {
        //apply the powerup
        PowerUpToAdd.Apply(this);
        //add powerup to powerup List
        powerUps.Add(PowerUpToAdd);

        Debug.Log("added powerUp");
    }

    public void Remove(PowerUp powerupToRemove)
    {
        //remove the power up
        powerupToRemove.Remove(this);
        //add it to the remove queue
        removedPowerupQueue.Add(powerupToRemove);
    }

    public void applyRemovePowerupsQueue() 
    {
        //now that we are not ittarating through the list remove the powerups that are in our temporary list
        foreach(PowerUp powerup in removedPowerupQueue)
        {
            powerUps.Remove(powerup);
        }
        //and clear the temporary list
        removedPowerupQueue.Clear();
    }

    public void DecrementPowerupTimers()
    {
        foreach (PowerUp powerup in powerUps)
        {

            if (!powerup.isPermanent)
            {
                powerup.duration -= Time.deltaTime;
            }
             
            if (powerup.duration <= 0 && !powerup.isPermanent)
                {
                    Remove(powerup);
                }
        }
        
    }
}
