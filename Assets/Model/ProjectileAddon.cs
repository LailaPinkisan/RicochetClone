using System;
using UnityEngine;
using Photon.Pun;

public class ProjectileAddOn : MonoBehaviourPun
{
    public int damage;
    public ThrowingMechanic.PowerUpType powerUpType;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Invoke(nameof(DestroySelfRPC), 5f);

        // Ignore collisions with player and other projectiles
        Collider projectileCollider = GetComponent<Collider>();
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider>();

        // Ignore collisions with player
        if (playerCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, playerCollider);
        }

        // // Ignore collisions with other projectiles
        // foreach (var otherProjectile in GameObject.FindGameObjectsWithTag("Projectile"))
        // {
        //     Collider otherCollider = otherProjectile.GetComponent<Collider>();
        //     if (otherCollider != null && otherCollider != projectileCollider)
        //     {
        //         Physics.IgnoreCollision(projectileCollider, otherCollider);
        //     }
        // }
    }
    [PunRPC]
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void DestroySelfRPC()
    {
        if (photonView.IsMine) 
        {
            photonView.RPC("DestroySelf", RpcTarget.AllBuffered);
        }
    }
    private void Update()
    {
        Debug.Log("PowerUpType is: " + powerUpType);
        // Destroy if out of bounds
        if (transform.position.y < -5f || transform.position.y > 50 || Math.Abs(transform.position.z) > 300f)
        {
            DestroySelfRPC();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return; // Only the owner processes the hit

        PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
        Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();

        // Bounce only off "BouncyBar"
        if (collision.gameObject.CompareTag("BouncyBar"))
        {
            BounceOff(collision.contacts[0].normal);
            return;
        }

        //  Apply damage to OTHER players (not self)
        if (targetView != null && !targetView.IsMine && collision.gameObject.CompareTag("Player"))
        {
            if (powerUpType == ThrowingMechanic.PowerUpType.PowerShot || powerUpType == ThrowingMechanic.PowerUpType.TripleShot)
            {
                damage = 100; // Power shots deal 100 damage
            }

            // Send damage over the network
            targetView.RPC("TakeDamage", RpcTarget.All, damage);

            // Push enemy
            if (targetRb != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                float pushForce = 5f; // Default push

                if (powerUpType == ThrowingMechanic.PowerUpType.FastShot)
                {
                    pushForce *= 2; // Double push for fast shot
                }

                targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }

            // Apply freeze effect (only to the enemy, not shooter)
            if (powerUpType == ThrowingMechanic.PowerUpType.FreezeShot)
            {
                targetView.RPC("ApplyFreezeEffect", RpcTarget.All, 12f);
                Debug.Log("FreezeShot applied! Enemy ground drag set to 12.");
            }

            DestroySelfRPC();
        }

        //  Ignore collision with other projectiles
        if (collision.gameObject.CompareTag("Projectile")) return;

        //  Destroy on hitting platforms
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Skybox"))
        {
            DestroySelfRPC();
        }
    }


    private void BounceOff(Vector3 normal)
    {
        if (rb == null) return;

        // Reflect the velocity off the surface normal
        Vector3 reflectedVelocity = Vector3.Reflect(rb.linearVelocity, normal);
        rb.linearVelocity = reflectedVelocity.normalized * rb.linearVelocity.magnitude * 2f;
    }
    public void SetPowerUp(ThrowingMechanic.PowerUpType newPowerUp)
    {
        powerUpType = newPowerUp;

        // Assign correct damage based on power-up type
        switch (powerUpType)
        {
            case ThrowingMechanic.PowerUpType.FastShot:
                damage = 35; // Adjust if needed
                break;
            case ThrowingMechanic.PowerUpType.FreezeShot:
                damage = 35;
                break;
            case ThrowingMechanic.PowerUpType.PowerShot:
                damage = 100;
                break;
            case ThrowingMechanic.PowerUpType.TripleShot:
                damage = 100;
                break;
            default:
                damage = 20; // Default damage
                break;
        }

        Debug.Log("Projectile PowerUpType: " + powerUpType + " | Damage set to: " + damage);
    }
}