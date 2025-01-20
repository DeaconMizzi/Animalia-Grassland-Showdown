using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cheetahPrefab; // Assign in the Inspector
    public GameObject bunnyPrefab;   // Assign in the Inspector
    public Transform spawnPoint;    // Spawn point for the player

    void Start()
    {
        // Spawn the selected character
        GameObject player;
        if (CharacterSelectManager.selectedCharacter == "Cheetah")
        {
            player = Instantiate(cheetahPrefab, spawnPoint.position, Quaternion.identity);
        }
        else if (CharacterSelectManager.selectedCharacter == "Bunny")
        {
            player = Instantiate(bunnyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}