using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpSpawner spawner;
    public GameObject prefab;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                ApplyEffect(other.gameObject);

                // Notify spawner that the power-up is collected
                Destroy(gameObject);
            }
        }
    }

    void ApplyEffect(GameObject player)
    {
        // Example effect logic based on prefab tag or name
        if (prefab.CompareTag("FastShot"))
        {
            Debug.Log("Fast Shot collected!"); // ✅ Example feedback
            // player.GetComponent<PlayerController>().ActivateFastShot(); // Example use
        }
        else if (prefab.CompareTag("FreezeShot"))
        {
            Debug.Log("Freeze Shot collected!");
            // player.GetComponent<PlayerController>().ActivateFreezeShot();
        }
        else if (prefab.CompareTag("PowerShot"))
        {
            Debug.Log("Power Shot collected!");
            // player.GetComponent<PlayerController>().ActivatePowerShot();
        }
        else if (prefab.CompareTag("TripleShot"))
        {
            Debug.Log("Triple Shot collected!");
            // player.GetComponent<PlayerController>().ActivateTripleShot();
        }

        // Optional: Add a sound or VFX on collection
        // AudioManager.PlaySound("powerup");
        // Instantiate(pickupVFX, transform.position, Quaternion.identity);
    }
}
