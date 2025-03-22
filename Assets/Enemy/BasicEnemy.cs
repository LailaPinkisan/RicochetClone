using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }
    public void ApplySlow(float slowFactor)
    {
        // Example: Reduce speed if the enemy has a movement component
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.speed *= slowFactor; // Reduce speed
        }
    }
}
