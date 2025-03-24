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
    public void SetPowerUpUI(string powerUpName)
    {
        powerUpText.text = powerUpName;
        powerUpText.color = GetPowerUpColor(powerUpName);
        powerUpText.color = new Color(48f, 48f, 48f);
        for (int i = 0; i < shurikenIcons.Length; i++)
        {
            shurikenIcons[i].sprite = powerUpName == "FastShot" ? fastShotShuriken : powerUpName == "FreezeShot" ? 
            freezeShotShuriken : powerUpName == "PowerShot" ? powerShotShuriken : powerUpName == "TripleShot" ? tripleShotShuriken : normalShuriken;
        }
    }

    // Get the color for each power-up
    private Color GetPowerUpColor(string powerUpName)
    {
        switch (powerUpName)
        {
            case "FastShot":
                return Color.cyan;
            case "FreezeShot":
                return Color.blue;
            case "FastFreezeShot":
                return new Color(0.5f, 0.2f, 1f);
            default:
                return Color.white;
        }
    }

    // ✅ Reset UI to normal state after power-up ends
    public void ClearPowerUpUI()
    {
        powerUpText.text = "NormalDisc";
    }
    
}
