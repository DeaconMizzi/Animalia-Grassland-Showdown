using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string characterSelectSceneName = "CharacterSelect"; 
    public string loseScreenSceneName = "Lose"; 

    public void LoadCharacterSelect()
    {
        SceneManager.LoadScene(characterSelectSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
