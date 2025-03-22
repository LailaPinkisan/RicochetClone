using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(damage));

                // ✅ Apply freeze effect if available
                if (isFreezeShot)
                {
                    enemy.ApplySlow(0.7f); // Slow down movement by 30%
                }
            }

            // Destroy shuriken after hit
            Destroy(gameObject);
        }
    }

    public bool isFreezeShot = false;
    public void ApplyFreezeEffect()
    {
        isFreezeShot = true;
    }
}
