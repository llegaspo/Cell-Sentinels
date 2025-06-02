using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public TMP_Text coinText;
    public Slider cytokineBar;

    private int coins = 0;
    private float cytokineLevel = 0;

    void Start()
    {
        UpdateHUD();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateHUD();
    }

    public void AddCytokine(float amount)
    {
        cytokineLevel = Mathf.Clamp(cytokineLevel + amount, 0, 100);
        UpdateHUD();
    }

    void UpdateHUD()
    {
        coinText.text = "Coins: " + coins;
        cytokineBar.value = cytokineLevel;
    }
}
