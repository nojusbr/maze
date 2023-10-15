using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinControllerUI : MonoBehaviour
{
    public TextMeshProUGUI coinCountText; // Reference to the TextMeshPro text

    void Start()
    {
        UpdateCoinCountUI(0); // Initialize with 0 coins
    }

    public void UpdateCoinCountUI(int coinCount)
    {
        if (coinCountText != null)
        {
            coinCountText.text = "Coins: " + coinCount.ToString();
        }
    }
}
