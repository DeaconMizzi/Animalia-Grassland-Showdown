using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneStarter : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;

                // Hide the player visual during cutscene
                SpriteRenderer[] spriteRenderers = other.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in spriteRenderers)
                {
                    sr.enabled = false;
                }

                // Play cutscene
                cutsceneDirector.Play();

                // Re-enable the player visuals after cutscene
                StartCoroutine(ReenablePlayerVisuals(other.gameObject, cutsceneDirector.duration));

                // Optional: Prevent retriggering
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ReenablePlayerVisuals(GameObject player, double delay)
    {
        yield return new WaitForSeconds((float)delay);

        SpriteRenderer[] spriteRenderers = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
