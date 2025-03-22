using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private PlayerHealthUI playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealthUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth.TakeDamage(10);
        }
    }
}