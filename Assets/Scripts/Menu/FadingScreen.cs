using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    private Image black;

    private float fadeSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        black = GetComponent<Image>();
    }
    
    public IEnumerator FadeToBlack()
    {
        while (black.color.a < 1f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }
        
        AudioManager.instance.PauseAllEvents();
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
        yield return null;
    }
}
