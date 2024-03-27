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
                StartCoroutine(FadeFromBlack(speed));
            else
                StartCoroutine(FadeToBlack(speed));
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == mainScene)
        {
            StartCoroutine(FadeFromBlack());
        }
    }
    public IEnumerator FadeToBlack(float fadeSpeed = 0.5f)
    {
        isFadingFromBlack = false;
        speed = fadeSpeed;
        while (black.color.a < 1f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }

        isFadingFromBlack = true;
        yield return null;
    }

    public IEnumerator FadeFromBlack(float fadeSpeed = 0.5f)
    {
        speed = fadeSpeed;
        isFadingFromBlack = true;
        while (black.color.a > 0f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a - (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }

        isFadingFromBlack = false;
        yield return null;
    }
}

