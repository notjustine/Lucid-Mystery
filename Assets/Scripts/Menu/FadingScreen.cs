using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    private Image black;
    private PlayerControl player;
    private float fadeSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        black = GetComponent<Image>();
        player = FindObjectOfType<PlayerControl>();
    }
    
    public IEnumerator FadeToBlack()
    {
        while (black.color.a < 1f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }
        
        AudioManager.instance.PauseAllEvents();
        player.gameObject.SetActive(false);
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
        
        yield return null;
    }
}
