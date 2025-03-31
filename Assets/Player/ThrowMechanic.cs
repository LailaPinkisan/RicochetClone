using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ThrowingMechanic : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject shurikenPrefab;
    public Image discIconUI;
    public ShurikenUIManager shurikenUI;
    public PlayerMovement playerMovement;

    [Header("Settings")]
    public int totalThrows = 3;
    public int currentThrows;
    public float throwCooldown = 0.1f;
    public float reloadTime = 3f;
    // public int powerUpShots;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode threeKey = KeyCode.Mouse1;
    public float baseThrowForce = 10f;
    public float throwForce;
    private bool readyToThrow;
    private bool reloading = false;

    // Power-up handling
    public static PowerUpType activePowerUp = PowerUpType.None;
    // private int powerUpShots = 3;

    void Start()
    {
        AssignUIElements();
        readyToThrow = true;
        currentThrows = totalThrows;
        throwForce = baseThrowForce;

        // initialize UI
        shurikenUI.UpdateShurikenUI(currentThrows);
        shurikenUI.ClearPowerUpUI();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && currentThrows > 0)
        {
            if (activePowerUp == PowerUpType.TripleShot)
            {
                TripleShot();
            }
            else
            {
                Throw();
            }
        }
        else if (Input.GetKeyDown(threeKey) && currentThrows > 0)
        {
            StartCoroutine(ThrowAllDiscs());
        }
    }

    private void AssignUIElements()
    {
        // Find the ShurikenIcons GameObject
        GameObject shurikenIconsObject = GameObject.Find("ShurikenIcons");

        if (shurikenIconsObject != null)
        {
            // Get the Shuriken UI Manager from it
            shurikenUI = shurikenIconsObject.GetComponent<ShurikenUIManager>();

            if (shurikenUI != null)
            {
                // Assuming the first normal shuriken image is the one you need
                discIconUI = shurikenUI.shurikenIcons[0];

                Debug.Log("Shuriken UI Manager assigned successfully!");
            }
        }
    }
    private void Throw()
    {
        if (currentThrows <= 0 || !readyToThrow) return;
        readyToThrow = false;
        //  Reduce ammo count
        currentThrows--;

        // if (activePowerUp != PowerUpType.None)
        // {
        //     powerUpShots--;
        //     Debug.Log("Power-up throw used. Remaining: " + currentThrows);
        //     // If power-up shots are depleted, disable power-up
        //     if (powerUpShots <= 0)
        //     {
        //         Debug.Log("Power-up shots depleted. Disabling power-up.");
        //         DisablePowerUp();
        //     }
        // }
        // else
        // {
        //     if (currentThrows == 0)
        //     {
        //         shurikenUI.ClearPowerUpUI();
        //     }
        // }

        //  Create a ray from the PlayerCam's position, facing forward
        Ray ray = new Ray(cam.position, cam.forward);
        Vector3 forceDirection = ray.direction;

        // Instantiate the shuriken
        GameObject projectile = Instantiate(shurikenPrefab, ray.origin, Quaternion.LookRotation(forceDirection));
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        projectileRb.linearVelocity = forceDirection * throwForce;
        //  Update UI
        shurikenUI.UpdateShurikenUI(currentThrows);
        if (currentThrows == 0)
        {
            shurikenUI.ClearPowerUpUI();
        }

        //  Start reload if needed
        if (currentThrows <= 0 && !reloading)
        {
            reloading = true;
            StartCoroutine(ReloadOneByOne());
        }

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private IEnumerator ThrowAllDiscs()
    {
        float spreadAngle = 10f;
        int throwsToMake = currentThrows;
        float spawnOffset = 2f;

        for (int i = 0; i < throwsToMake; i++)
        {
            if (currentThrows <= 0) break;

            float angleOffset = (i - (throwsToMake - 1) / 2f) * spreadAngle;
            Quaternion rotationOffset = Quaternion.Euler(0, angleOffset, 0);

            Vector3 spreadDirection = rotationOffset * cam.forward;

            float heightOffset = (throwsToMake == 3 && i == 1) ? 0.5f : 0f;
            Vector3 spawnPosition = cam.position + cam.forward * spawnOffset;
            spawnPosition.y += heightOffset;

            //  Instantiate the disc!
            GameObject projectile = Instantiate(shurikenPrefab, spawnPosition, Quaternion.LookRotation(spreadDirection));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.linearVelocity = spreadDirection * throwForce;

            // //  Reduce ammo count properly based on power-up status
            // if (activePowerUp != PowerUpType.None)
            // {
            //     // currentThrows--;
            //     powerUpShots--;
            //     // If power-up shots are depleted, disable power-up
            //     if (powerUpShots <= 0)
            //     {
            //         DisablePowerUp();
            //     }
            // }
            // else
            // {
            //     currentThrows--;
            //     {
            //         shurikenUI.ClearPowerUpUI();
            //     }
            // }
            currentThrows--;

            //  Update UI after each throw
            shurikenUI.UpdateShurikenUI(currentThrows);

            yield return new WaitForSeconds(0.05f);
        }

        if (currentThrows == 0)
        {
            shurikenUI.ClearPowerUpUI();
        }


        //  Start reload if necessary
        if (currentThrows <= 0 && !reloading)
        {
            reloading = true;
            StartCoroutine(ReloadOneByOne());
        }
    }

    private IEnumerator ReloadOneByOne()
    {
        while (currentThrows < totalThrows)
        {
            yield return new WaitForSeconds(reloadTime);
            currentThrows++;
            shurikenUI.UpdateShurikenUI(currentThrows);

            if (!readyToThrow) readyToThrow = true;

            if (currentThrows >= totalThrows)
            {
                reloading = false;
                break;
            }
        }

        // disable power-up when fully reloaded
        // if (activePowerUp != PowerUpType.None && currentThrows == totalThrows)
        // {
        //     DisablePowerUp();
        // }
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    // Activate Power-Up Handling
    public void ActivatePowerUp(PowerUpType newPowerUp)
    {
        activePowerUp = newPowerUp;
        // powerUpShots = 3;
        // currentThrows = 3;

        // shurikenUI.UpdateShurikenUI(currentThrows);

        switch (activePowerUp)
        {
            case PowerUpType.FastShot:
                throwForce = baseThrowForce * 4f;
                Debug.Log("Fast Shot power-up activated! Throw force: " + throwForce);
                break;
            case PowerUpType.FreezeShot:
                throwForce = baseThrowForce;
                playerMovement.groundDrag = 8;
                break;
            case PowerUpType.PowerShot:
                throwForce = baseThrowForce;
                break;
            case PowerUpType.TripleShot:

                throwForce = baseThrowForce;
                break;
            default:
                throwForce = baseThrowForce;
                shurikenUI.ClearPowerUpUI();
                break;
        }

    }
    // Disable power-up
    private void TripleShot()
    {
        if (currentThrows <= 0 || !readyToThrow) return;
        readyToThrow = false;
        currentThrows--; // Deduct only 1 bullet

        float spreadAngle = 10f;
        float spawnOffset = 2f;

        for (int i = 0; i < 3; i++)
        {
            float angleOffset = (i - 1) * spreadAngle;
            Quaternion rotationOffset = Quaternion.Euler(0, angleOffset, 0);
            Vector3 spreadDirection = rotationOffset * cam.forward;

            float heightOffset = (i == 1) ? 0.5f : 0f;
            Vector3 spawnPosition = cam.position + cam.forward * spawnOffset;
            spawnPosition.y += heightOffset;

            GameObject projectile = Instantiate(shurikenPrefab, spawnPosition, Quaternion.LookRotation(spreadDirection));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.linearVelocity = spreadDirection * throwForce;
        }

        shurikenUI.UpdateShurikenUI(currentThrows);
        if (currentThrows == 0)
        {
            shurikenUI.ClearPowerUpUI();
        }

        if (currentThrows <= 0 && !reloading)
        {
            reloading = true;
            StartCoroutine(ReloadOneByOne());
        }

        Invoke(nameof(ResetThrow), throwCooldown);
    }
    private void DisablePowerUp()
    {
        activePowerUp = PowerUpType.None;
        throwForce = baseThrowForce;
        shurikenUI.ClearPowerUpUI(); // Clear power-up UI
        shurikenUI.UpdateShurikenUI(currentThrows);
        // if (currentThrows <= 0 && powerUpShots <= 0 && !reloading)
        // {
        //     reloading = true;
        //     StartCoroutine(ReloadOneByOne());
        // }
    }
    // Define Power-Up Types
    public enum PowerUpType
    {
        None,
        FastShot,
        FreezeShot,
        PowerShot,
        TripleShot
    }
}
