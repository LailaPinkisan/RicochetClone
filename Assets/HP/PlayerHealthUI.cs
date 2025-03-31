using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerHealthUI : MonoBehaviourPun
{
    public Image healthBar; // Reference to your PNG-based health bar
    public TextMeshProUGUI hpText;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        // Try finding components automatically
        healthBar = GameObject.Find("Background")?.GetComponent<Image>();
        hpText = GameObject.Find("HP Text")?.GetComponent<TextMeshProUGUI>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError("Player reference is missing in PlayerHealthUI!");
        if (healthBar == null) Debug.LogError("HealthBar UI is not assigned!");

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    [PunRPC] //  Allow network damage
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        hpText.text = Mathf.RoundToInt(currentHealth).ToString(); // Update the number
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Handle respawning or disabling the player
    }
}