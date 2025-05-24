using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use if you’re using TextMeshPro

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text nameText; // If using TextMeshPro, otherwise use UnityEngine.UI.Text
    public TMP_Text dialogueText;
    public Image portraitImage;

    public float typingSpeed = 0.02f; // Adjust typing speed here
    public AudioSource blipSound; // Optional: Add a typing sound

    private Coroutine typingCoroutine;

    public void ShowDialogue(string speakerName, string dialogue, Sprite portrait)
    {
        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        portraitImage.sprite = portrait;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            if (blipSound != null && c != ' ') blipSound.Play(); // Optional typing sound
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
