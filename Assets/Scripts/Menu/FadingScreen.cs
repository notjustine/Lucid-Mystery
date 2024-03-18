using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    private Image black;
    // Start is called before the first frame update
    void Awake()
    {
        black = GetComponent<Image>();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "PatentEnvironment")
        {
            StartCoroutine(FadeFromBlack());
        }
    }
    public IEnumerator FadeToBlack(float fadeSpeed = 0.5f)
    {
        while (black.color.a < 1f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
    
    public IEnumerator FadeFromBlack(float fadeSpeed = 0.5f)
    {
        while (black.color.a > 0f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a - (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}
