using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static string selectedCharacter;
    public CanvasGroup fadePanel;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
        fadePanel.interactable = false;
    }

    public void SelectCharacter(string characterName)
    {
        selectedCharacter = characterName;
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        StartCoroutine(FadeAndLoadScene("Cutscene_Intro"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.blocksRaycasts = true;
        fadePanel.interactable = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = timer / fadeDuration;
            yield return null;
        }

        fadePanel.alpha = 1f;
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
