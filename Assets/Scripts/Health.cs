using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthRemaining;

    void Update()
    {
        CheckHealth();
    }

    public void TakeDamage(float amount)
    {
        HealthRemaining -= amount;
    }

    private void CheckHealth()
    {
        if (HealthRemaining <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
