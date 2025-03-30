using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShurikenUIManager : MonoBehaviour
{
    public Image[] shurikenIcons;
    public Sprite normalShuriken;
    public Sprite emptyShuriken;

    public Sprite fastShotShuriken;
    public Sprite freezeShotShuriken;

    public Sprite powerShotShuriken;
    public Sprite tripleShotShuriken;

    [Header("Power-Up UI")]
    public TextMeshProUGUI powerUpLabel;
    public Color fastShotColor = Color.cyan;
    public Color freezeShotColor = Color.blue;
    public Color normalColor = Color.white;
    public TextMeshProUGUI powerUpText; // Add this line

    // private bool isFastShotActive = false;
    public void UpdateShurikenUI(int currentThrows)
    {
        for (int i = 0; i < shurikenIcons.Length; i++)
        {
            if (i < currentThrows)
            {
                shurikenIcons[i].color = Color.white;
                shurikenIcons[i].GetComponent<Outline>().enabled = true; // Enable Glow
            }
            else
            {
                shurikenIcons[i].sprite = emptyShuriken;
                shurikenIcons[i].color = new Color(1f, 1f, 1f, 0.3f); // Greyed out
                shurikenIcons[i].GetComponent<Outline>().enabled = false; // Disable Glow
            }
        }
    }

    // ✅ Show the active power-up in UI
    public void SetPowerUpUI(ThrowingMechanic.PowerUpType newPowerUp)
    {
        powerUpText.text = GetPowerUpText(newPowerUp);
        powerUpText.color = new Color(48f, 48f, 48f);
        for (int i = 0; i < shurikenIcons.Length; i++)
        {
            shurikenIcons[i].sprite = GetPowerUpIcon(newPowerUp);
        }
    }

    private string GetPowerUpText(ThrowingMechanic.PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case ThrowingMechanic.PowerUpType.FastShot:
                return "Fast Shot";
            case ThrowingMechanic.PowerUpType.FreezeShot:
                return "Freeze Shot";
            case ThrowingMechanic.PowerUpType.PowerShot:
                return "Power Shot";
            case ThrowingMechanic.PowerUpType.TripleShot:
                return "Triple Shot";
            default:
                return "Normal Disc";
        }
    }

    private Sprite GetPowerUpIcon(ThrowingMechanic.PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case ThrowingMechanic.PowerUpType.FastShot:
                return fastShotShuriken;
            case ThrowingMechanic.PowerUpType.FreezeShot:
                return freezeShotShuriken;
            case ThrowingMechanic.PowerUpType.PowerShot:
                return powerShotShuriken;
            case ThrowingMechanic.PowerUpType.TripleShot:
                return tripleShotShuriken;
            default:
                return normalShuriken;
        }
    }

    // ✅ Reset UI to normal state after power-up ends
    public void ClearPowerUpUI()
    {
        powerUpText.text = "Normal Disc";
    }
    
}
