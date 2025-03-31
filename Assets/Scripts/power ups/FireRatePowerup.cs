using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class FireRatePowerup : PowerUp
{
    public float fireRateAmount;
    
    
    public override void Apply(PowerUpManager target)
    {
        Pawn targetPawn = target.GetComponent<Pawn>();
        if (targetPawn != null)
        {
            targetPawn.fireRate += fireRateAmount;
        }
    }

    public override void Remove(PowerUpManager target)
    {
        Pawn targetPawn = target.GetComponent<Pawn>();
        if (targetPawn != null)
        {
            targetPawn.fireRate -= fireRateAmount;
        }
    }
}
