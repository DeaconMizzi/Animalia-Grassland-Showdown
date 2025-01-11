using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static string selectedCharacter;

    public void SelectCharacter(string characterName)
    {
        selectedCharacter = characterName; // Save the selected character
        SceneManager.LoadScene("GameScene"); // Load the main game scene
    }
}
