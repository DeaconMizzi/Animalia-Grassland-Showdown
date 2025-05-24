using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Character Portraits")]
    public Sprite CheetahPortrait;
    public Sprite BunnyPortrait;

    public void StartDialogue1()
    {
        dialogueManager.ShowDialogue("Cheetah","All systems online. Sensors locked. Let’s find this Squid before he slips away again.", CheetahPortrait);
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
}