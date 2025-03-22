using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShurikenUIManager : MonoBehaviour
{
    public Image[] shurikenIcons;
    public Sprite activeShuriken;
    public Sprite emptyShuriken;

    [Header("Power-Up UI")]
    public TextMeshProUGUI powerUpLabel;
    public Color fastShotColor = Color.cyan;
    public Color freezeShotColor = Color.blue;
    public Color normalColor = Color.white;
    public TextMeshProUGUI powerUpText; // Add this line
    public void UpdateShurikenUI(int currentThrows)
    {
        for (int i = 0; i < shurikenIcons.Length; i++)
        {
            if (i < currentThrows)
            {
                shurikenIcons[i].color = Color.white; // Active
                shurikenIcons[i].GetComponent<Outline>().enabled = true; // Enable Glow
            }
            else
            {
                shurikenIcons[i].color = new Color(1f, 1f, 1f, 0.3f); // Greyed out
                shurikenIcons[i].GetComponent<Outline>().enabled = false; // Disable Glow
            }
        }
    }

    // ✅ Show the active power-up in UI
    public void SetPowerUpUI(string powerUpName, Color powerUpColor)
    {
        powerUpText.text = powerUpName;
        powerUpText.color = Color.red;
    }

    // ✅ Reset UI to normal state after power-up ends
    public void ClearPowerUpUI()
    {
        powerUpText.text = "";
    }
    
}
