using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUI : MonoBehaviour
{
    public Image[] hearts; // Array to hold heart images

    public void InitializeHearts(int maxHealth)
    {
        // Ensure the heart display matches the max health
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < maxHealth; // Show hearts only up to maxHealth
        }
    }

    public void UpdateHearts(int health)
    {
        // Update heart visibility based on current health
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health; // Enable hearts only for remaining health
        }
    }
}
