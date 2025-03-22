using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddOn : MonoBehaviour
{   
    public int damage;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Destroy(gameObject, 5f);

        // Ignore collisions with player and other projectiles
        Collider projectileCollider = GetComponent<Collider>();
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        
        // Ignore collisions with player
        if (playerCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, playerCollider);
        }

        // Ignore collisions with other projectiles
        foreach (var otherProjectile in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Collider otherCollider = otherProjectile.GetComponent<Collider>();
            if (otherCollider != null && otherCollider != projectileCollider)
            {
                Physics.IgnoreCollision(projectileCollider, otherCollider);
            }
        }
    }

    private void Update()
    {
        // Destroy if out of bounds
        if (transform.position.y < -0f || transform.position.y > 50 || Math.Abs(transform.position.z) > 300f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Bounce only off "BouncyBar"
        if (collision.gameObject.CompareTag("BouncyBar"))
        {
            BounceOff(collision.contacts[0].normal);
            return;
        }

        // Deal damage to enemy
        BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // ✅ Don't destroy on player or projectile collision
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Projectile"))
        {
            return;
        }

        // Destroy on platform or any other object
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Skybox"))
        {
            Destroy(gameObject);
            return;
        }

        // Failsafe destroy for anything unexpected
        Destroy(gameObject);
    }

    private void BounceOff(Vector3 normal)
    {
        if (rb == null) return;

        // Reflect the velocity off the surface normal
        Vector3 reflectedVelocity = Vector3.Reflect(rb.linearVelocity, normal);
        rb.linearVelocity = reflectedVelocity.normalized * rb.linearVelocity.magnitude * 2f;
    }
}