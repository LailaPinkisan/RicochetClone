using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public PowerUpSpawner spawner;
    public GameObject prefab;
    public Transform spawnPoint;

    private ShurikenUIManager shurikenUI;
    private ThrowingMechanic throwmechanic;

    void Start()
    {
        // Get a reference to ShurikenUIManager
        shurikenUI = FindFirstObjectByType<ShurikenUIManager>();
        throwmechanic = FindFirstObjectByType<ThrowingMechanic>();
    }

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
        ResetThrowUI();

        // Example effect logic based on prefab tag or name
        if (prefab.CompareTag("FastShot"))
        {
            // player.GetComponent<PlayerController>().ActivateFastShot(); // Example use
            ThrowingMechanic.activePowerUp = ThrowingMechanic.PowerUpType.FastShot;
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.FastShot.ToString());
        }
        else if (prefab.CompareTag("FreezeShot") || gameObject.layer == LayerMask.NameToLayer("FreezeShot"))
        {
            // player.GetComponent<PlayerController>().ActivateFreezeShot();
            ThrowingMechanic.activePowerUp = ThrowingMechanic.PowerUpType.FreezeShot;
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.FreezeShot.ToString());
        }
        else if (prefab.CompareTag("PowerShot"))
        {
            ThrowingMechanic.activePowerUp = ThrowingMechanic.PowerUpType.PowerShot;
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.PowerShot.ToString());
            // player.GetComponent<PlayerController>().ActivatePowerShot();
        }
        else if (prefab.CompareTag("TripleShot"))
        {
            ThrowingMechanic.activePowerUp = ThrowingMechanic.PowerUpType.TripleShot;
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.TripleShot.ToString());
            // player.GetComponent<PlayerController>().ActivateTripleShot();
        }

        // Optional: Add a sound or VFX on collection
        // AudioManager.PlaySound("powerup");
        // Instantiate(pickupVFX, transform.position, Quaternion.identity);
    }

    private void ResetThrowUI()
    {
        throwmechanic.currentThrows = 3;

        // Reset any greyed out shurikens
        for (int i = 0; i < shurikenUI.shurikenIcons.Length; i++)
        {
            shurikenUI.shurikenIcons[i].color = Color.white;
            shurikenUI.shurikenIcons[i].GetComponent<Outline>().enabled = true;
        }
    }
}
