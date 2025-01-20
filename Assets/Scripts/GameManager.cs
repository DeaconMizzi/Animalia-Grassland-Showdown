using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cheetahPrefab; // Prefab for the Cheetah
    public GameObject bunnyPrefab;   // Prefab for the Bunny
    public Transform spawnPoint;    // Spawn point for the player
    public CameraController cameraFollow; // Reference to the CameraFollow script

    void Start()
    {
        GameObject player;

        // Spawn the selected character
        if (CharacterSelectManager.selectedCharacter == "Cheetah")
        {
            player = Instantiate(cheetahPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            player = Instantiate(bunnyPrefab, spawnPoint.position, Quaternion.identity);
        }

        // Assign the player to the CameraFollow script
        if (cameraFollow != null)
        {
            cameraFollow.target = player.transform;
        }
    }
}