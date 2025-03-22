using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject fastShotPrefab;
    public GameObject freezeShotPrefab;
    public GameObject powerShotPrefab;
    public GameObject tripleShotPrefab;

    public Transform[] firstFloorSpawns;
    public Transform[] secondFloorSpawns;

    private bool isFirstFloorOnCooldown = false;
    private bool isSecondFloorOnCooldown = false;
    private bool alternatePowerUp = false;

    void Start()
    {
        // Start spawning loops
        StartCoroutine(SpawnFirstFloorPowerUps());
        StartCoroutine(SpawnSecondFloorPowerUps());
    }

    IEnumerator SpawnFirstFloorPowerUps()
    {
        while (true)
        {
            if (!isFirstFloorOnCooldown)
            {
                isFirstFloorOnCooldown = true;

                // Pick either Freeze Shot or Fast Shot
                GameObject powerUpToSpawn = (Random.value > 0.5f) ? freezeShotPrefab : fastShotPrefab;

                Transform spawnPoint = firstFloorSpawns[Random.Range(0, firstFloorSpawns.Length)];
                GameObject spawnedPowerUp = Instantiate(powerUpToSpawn, spawnPoint.position, Quaternion.identity);

                PowerUp powerUpScript = spawnedPowerUp.GetComponent<PowerUp>();
                if (powerUpScript != null)
                {
                    powerUpScript.spawner = this;
                    powerUpScript.prefab = powerUpToSpawn;
                    powerUpScript.spawnPoint = spawnPoint;
                }

                // ✅ Wait until the player collects the power-up before restarting the timer
                yield return new WaitUntil(() => spawnedPowerUp == null);
                yield return new WaitForSeconds(15f);

                isFirstFloorOnCooldown = false;
            }

            yield return null;
        }
    }

    IEnumerator SpawnSecondFloorPowerUps()
    {
        while (true)
        {
            if (!isSecondFloorOnCooldown)
            {
                isSecondFloorOnCooldown = true;

                // Alternate between Power Shot and Triple Shot
                GameObject powerUpToSpawn = alternatePowerUp ? powerShotPrefab : tripleShotPrefab;
                alternatePowerUp = !alternatePowerUp;

                Transform spawnPoint = secondFloorSpawns[Random.Range(0, secondFloorSpawns.Length)];
                GameObject spawnedPowerUp = Instantiate(powerUpToSpawn, spawnPoint.position, Quaternion.identity);

                PowerUp powerUpScript = spawnedPowerUp.GetComponent<PowerUp>();
                if (powerUpScript != null)
                {
                    powerUpScript.spawner = this;
                    powerUpScript.prefab = powerUpToSpawn;
                    powerUpScript.spawnPoint = spawnPoint;
                }

                // ✅ Wait until the player collects the power-up before restarting the timer
                yield return new WaitUntil(() => spawnedPowerUp == null);
                yield return new WaitForSeconds(15f);

                isSecondFloorOnCooldown = false;
            }

            yield return null;
        }
    }

    private float GetSpawnDelay(GameObject prefab)
    {
        if (prefab == fastShotPrefab) return 20f;
        if (prefab == freezeShotPrefab) return 26f;
        if (prefab == powerShotPrefab || prefab == tripleShotPrefab) return 35f;
        return 10f; // Default delay if prefab doesn't match known types
    }
}
