using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoter : Shooter
{
    public Shooter shooter;
    public Transform FirePointTransform;
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        GameObject newShell = Instantiate(shellPrefab, FirePointTransform.position, FirePointTransform.rotation);
        damageOnHit doh = newShell.GetComponent<damageOnHit>();

        if (doh != null)
        {
            doh.damageDone = damageDone;
            doh.Owner = GetComponent<Pawn>();

        }
        Rigidbody rb = newShell.GetComponent <Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(FirePointTransform.forward * fireForce);
        }
        Destroy(newShell, lifeSpan );
    }
}
