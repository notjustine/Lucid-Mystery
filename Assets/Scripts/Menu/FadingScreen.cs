using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    private Image black;
    private PlayerControl player;
    // Start is called before the first frame update
    void Awake()
    {
        black = GetComponent<Image>();
        player = FindObjectOfType<PlayerControl>();
    }
    
    public IEnumerator FadeToBlack(string scene = "EndMenu", float fadeSpeed = 0.5f)
    {
        while (black.color.a < 1f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }

        // if (SceneManager.GetActiveScene().name == "PatentEnvironment")
        // {
        //     AudioManager.instance.PauseAllEvents();
        //     player.gameObject.SetActive(false);
        //     SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        // }
        // else
        // {
        //     SceneManager.LoadScene(scene);
        // }
        
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
