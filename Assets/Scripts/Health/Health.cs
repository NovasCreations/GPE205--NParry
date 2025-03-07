using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Health : MonoBehaviour
    {
    
    public float currentHealth;
    public float maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        
    }
    //public void takeDamage()
    //    { currentHealth = 0; }
    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(float amount, Pawn source)
    { 
        currentHealth = currentHealth - amount;
        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die(source);
        }
    }
    public void Heal(float amount, Pawn source)

    {
        currentHealth = currentHealth + amount;

        Debug.Log(source.name + " did " + amount + " healing to " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    }



    public void Die(Pawn source)
    {
        Destroy(gameObject);
    }
}
