using UnityEngine;
using UnityEngine.UI;
 using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public Image healthBar; // Reference to your PNG-based health bar
    public TextMeshProUGUI hpText;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        hpText.text = Mathf.RoundToInt(currentHealth).ToString(); // Update the number
    }
}