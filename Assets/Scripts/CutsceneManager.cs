using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Character Portraits")]
    public Sprite CheetahPortrait;
    public Sprite BunnyPortrait;

    [Header("Fade Settings")]
    public CanvasGroup fadePanel; // Assign in Inspector
    public float fadeDuration = 0.5f; // Fade-in time

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
}