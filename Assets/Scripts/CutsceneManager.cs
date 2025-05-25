using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Character Portraits")]
    public Sprite CheetahPortrait;
    public Sprite BunnyPortrait;

    public Sprite SquidPortrait;

    [Header("Fade Settings")]
    public CanvasGroup fadePanel; // Assign in Inspector
    public float fadeDuration = 0.5f; // Fade-in time

    [Header("Character Prefabs")]
    public Transform spawnPoint; // Assign the spawn point in the Inspector
    public GameObject cheetahPrefab; // Assign the Cheetah prefab in the Inspector
    public GameObject bunnyPrefab;   // Assign the Bunny prefab in the Inspector

    public void StartDialogue1()
    {
        dialogueManager.ShowDialogue("Cheetah", "All systems online. Sensors locked. Let’s find this Squid before he slips away again.", CheetahPortrait);
    }

    public void StartDialogue2()
    {
        dialogueManager.ShowDialogue("Bunny", "Reading faint signals ahead... Looks like Squid’s been here. Tracks are fresh.", BunnyPortrait);
    }

    public void StartDialogue3()
    {
        dialogueManager.ShowDialogue("Cheetah", "Doesn’t matter where he runs. We end this, today.", CheetahPortrait);
    }

    public void StartDialogue4()
    {
        dialogueManager.ShowDialogue("Cheetah", "There he is, down there. It's time.", CheetahPortrait);
    }
    public void StartDialogue4Bunny()
    {
        dialogueManager.ShowDialogue("Bunny", "There he is, down there. It's time.", BunnyPortrait);
    }

    public void StartDialogue5()
    {
        dialogueManager.ShowDialogue("Squid", "So, they sent the new ones, scared to face me themselves?", SquidPortrait);
    }
    public void StartDialogue5Cheetah()
    {
        dialogueManager.ShowDialogue("Cheetah", "They wouldn't waste their time with you, it's over.", CheetahPortrait);
    }
    public void StartDialogue5Bunny()
    {
        dialogueManager.ShowDialogue("Bunny", "They wouldn't waste their time with you, it's over.", BunnyPortrait);
    }
    public void StartDialogue6()
    {
        dialogueManager.ShowDialogue("Squid", "Over? Let's see about that...", SquidPortrait);
    }


    public void EndDialogue()
    {
        dialogueManager.HideDialogue();
    }

    public void StartFadeAndLoad(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Enable fade panel
        fadePanel.gameObject.SetActive(true);
        fadePanel.blocksRaycasts = true;
        fadePanel.interactable = true;

        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = timer / fadeDuration;
            yield return null;
        }

        fadePanel.alpha = 1f; // Ensure fully opaque

        // Optional pause after fade (1 second)
        yield return new WaitForSeconds(1f);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }

    public void SpawnPlayer()
    {
        string selected = PlayerPrefs.GetString("SelectedCharacter", "Cheetah"); // Default fallback in PlayerPrefs

        GameObject playerPrefab = null;

        if (selected == "Cheetah" && cheetahPrefab != null)
        {
            playerPrefab = cheetahPrefab;
        }
        else if (selected == "Bunny" && bunnyPrefab != null)
        {
            playerPrefab = bunnyPrefab;
        }
        else
        {
            Debug.LogWarning("SelectedCharacter not found or prefab missing. Spawning Cheetah by default.");
            playerPrefab = cheetahPrefab;
        }

        if (playerPrefab != null)
        {
            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

            // Update camera target after spawning the player
            CameraController cameraController = FindObjectOfType<CameraController>();
            if (cameraController != null)
            {
                cameraController.target = playerInstance.transform;
            }
        }
        else
        {
            Debug.LogError("No valid player prefab found. Please assign at least the Cheetah prefab in the inspector.");
        }
    }
    
    public void FreezeBoss()
    {
        BossController boss = FindObjectOfType<BossController>();
        if (boss != null) boss.SetFrozen(true);
        BossAttack.freezeAllAttacks = true;
    }

    public void UnfreezeBoss()
    {
        BossController boss = FindObjectOfType<BossController>();
        if (boss != null) boss.SetFrozen(false);
        BossAttack.freezeAllAttacks = false;
    }
}