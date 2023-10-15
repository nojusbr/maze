using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Coin : MonoBehaviour
{
    public float coinRotationSpeed = 1f;

    public int coinValue = 1; // Score value for each coin
    public AudioClip collectSound; // Sound to play when collecting a coin
    private int collectedCoins = 0;
    private CoinControllerUI coinUIController; // Reference to the CoinUIController

    private void Update()
    {
        transform.Rotate(Vector3.up, coinRotationSpeed * Time.deltaTime);
    }

    private void Start()
    {
        coinUIController = FindObjectOfType<CoinControllerUI>(); // Find the CoinUIController
        UpdateCoinCountUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // Play a sound (if specified)
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Increase the collected coins count
        collectedCoins++;

        // Update the UI
        UpdateCoinCountUI();

        // Deactivate the coin (you can modify this behavior)
        gameObject.SetActive(false);
    }

    void UpdateCoinCountUI()
    {
        if (coinUIController != null)
        {
            coinUIController.UpdateCoinCountUI(collectedCoins);
        }
    }
}
