using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float Percentage => CurrentHealth / MaxHealth;
    
    public float MaxHealth = 10f;
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(float amt)
    {
        CurrentHealth += amt;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amt)
    {
        CurrentHealth -= amt;

        if (CurrentHealth <= 0)
        {
            RunDeath();
        }
    }

    public virtual void RunDeath()
    {
        Destroy(this.gameObject);
    }
}
