using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PowerUp : MonoBehaviourPun
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
        Debug.Log($"PowerUp collided with: {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("PowerUp absorbed by player!");
            ApplyEffect(other.gameObject);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Destroying power-up locally (Master Client)");
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Debug.Log("Requesting Master Client to destroy power-up");
                photonView.RPC("RequestDestroyPowerUp", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    void RequestDestroyPowerUp()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void ApplyEffect(GameObject player)
    {
        ResetThrowUI();
        if (throwmechanic == null)
        {
            throwmechanic = FindFirstObjectByType<ThrowingMechanic>();
        }

        if (throwmechanic == null)
        {
            Debug.LogError("ThrowingMechanic is STILL NULL! Power-up cannot be applied.");
            return;
        }
        // Example effect logic based on prefab tag or name
        if (prefab.CompareTag("FastShot"))
        {
            // player.GetComponent<PlayerController>().ActivateFastShot(); // Example use
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.FastShot);
            throwmechanic.ActivatePowerUp(ThrowingMechanic.PowerUpType.FastShot);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("FreezeShot"))
        {
            // player.GetComponent<PlayerController>().ActivateFreezeShot();
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.FreezeShot);
            throwmechanic.ActivatePowerUp(ThrowingMechanic.PowerUpType.FreezeShot);
        }
        else if (prefab.CompareTag("PowerShot"))
        {
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.PowerShot);
            // player.GetComponent<PlayerController>().ActivatePowerShot();
            throwmechanic.ActivatePowerUp(ThrowingMechanic.PowerUpType.PowerShot);
        }
        else if (prefab.CompareTag("TripleShot"))
        {
            shurikenUI.SetPowerUpUI(ThrowingMechanic.PowerUpType.TripleShot);
            // player.GetComponent<PlayerController>().ActivateTripleShot();
            throwmechanic.ActivatePowerUp(ThrowingMechanic.PowerUpType.TripleShot);
        }

        // Optional: Add a sound or VFX on collection
        // AudioManager.PlaySound("powerup");
        // Instantiate(pickupVFX, transform.position, Quaternion.identity);
    }

    private void ResetThrowUI()
    {
        if (throwmechanic == null)
        {
            throwmechanic = FindFirstObjectByType<ThrowingMechanic>();
        }

        if (throwmechanic == null)
        {
            Debug.LogError("ThrowingMechanic is NULL in PowerUp! Make sure it's in the scene.");
            return;

        }
        if (shurikenUI == null)
        {
            Debug.LogError("ShurikenUIManager is NULL in PowerUp!");
            return;
        }
        throwmechanic.currentThrows = 3;

        // Reset any greyed out shurikens
        for (int i = 0; i < shurikenUI.shurikenIcons.Length; i++)
        {
            shurikenUI.shurikenIcons[i].color = Color.white;
            shurikenUI.shurikenIcons[i].GetComponent<Outline>().enabled = true;
        }
    }
}
