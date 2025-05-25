using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cheetahPrefab; // Prefab for the Cheetah
    public GameObject bunnyPrefab;   // Prefab for the Bunny
    public Transform spawnPoint;     // Spawn point for the player
    public CameraController cameraFollow; // Reference to the CameraFollow script
    public bool skipAutoSpawn = false;

    void Start()
    {
        if (skipAutoSpawn)
        {
            Debug.Log("GameManager: Skipping auto-spawn in this scene.");
            return;
        }

        GameObject player;

        string selected = CharacterSelectManager.selectedCharacter;
        if (string.IsNullOrEmpty(selected))
        {
            Debug.LogWarning("No character selected. Defaulting to Cheetah.");
            selected = "Cheetah";
        }

        if (selected == "Cheetah")
        {
            player = Instantiate(cheetahPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            player = Instantiate(bunnyPrefab, spawnPoint.position, Quaternion.identity);
        }

        if (cameraFollow != null)
        {
            cameraFollow.target = player.transform;
        }
    }
}
