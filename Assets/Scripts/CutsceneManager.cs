using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Character Portraits")]
    public Sprite captainVegaPortrait;
    public Sprite aiCorePortrait;

    public void StartDialogue1()
    {
        dialogueManager.ShowDialogue("Captain Vega", "All systems green. Preparing for jump.", captainVegaPortrait);
    }

    public void StartDialogue2()
    {
        dialogueManager.ShowDialogue("AI Core", "Trajectory is stable. Fuel at 97%.", aiCorePortrait);
    }

    public void EndDialogue()
    {
        dialogueManager.HideDialogue();
    }
}