using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    private Image black;

    [SerializeField] private string mainScene = "ZyngaMain";
    private bool isFadingFromBlack = false;
    public bool inProgress {private set; get;} = false;
    private float speed = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        black = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (black.color.a > 0 && black.color.a < 255)
        {
            if (isFadingFromBlack)
                StartCoroutine(FadeFromBlackEnding(speed));
            else
                StartCoroutine(FadeToBlack(speed));
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == mainScene)
        {
            if (SceneManager.GetSceneByName("EndMenu").isLoaded && !DeathMenu.bossDied)
            {
                StartCoroutine(FadeFromBlackEnding());
            }
            else
            {
                StartCoroutine(FadeFromBlack());
            }
        }
    }
    public IEnumerator FadeToBlack(float fadeSpeed = 0.5f)
    {
        isFadingFromBlack = false;
        inProgress = true;
        speed = fadeSpeed;
        while (black.color.a < 1f)
        {
            yield return black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            // yield return new WaitForSeconds(0.01f);
        }

        isFadingFromBlack = true;
        inProgress = false;
        // yield return null;
    }

    public IEnumerator FadeFromBlack(float fadeSpeed = 0.5f)
    {
        speed = fadeSpeed;
        isFadingFromBlack = true;
        inProgress = true;
        while (black.color.a > 0f)
        {
            yield return black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a - (fadeSpeed * Time.deltaTime));
            // yield return new WaitForSeconds(0.01f);
        }

        isFadingFromBlack = false;
        inProgress = false;
        // yield return null;
    }

    private IEnumerator FadeFromBlackEnding(float fadeSpeed = 0.5f)
    {
        yield return StartCoroutine(FadeFromBlack(fadeSpeed));
        Time.timeScale = 0;
    }
}

