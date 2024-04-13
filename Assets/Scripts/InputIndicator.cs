using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputIndicator : MonoBehaviour
{

    public enum SpriteType
    {
        ON_BEAT_INPUTTED,
        OFF_BEAT_INPUTTED
    }
    
    public static InputIndicator Instance { get; private set; }
    private GameObject onBeatImage;
    private GameObject offBeatImage;
    private Image image;
    [SerializeField] private Sprite[] sprites;
    // public SpriteType type = SpriteType.ON_BEAT;
    private bool inProgress = false;
    private Coroutine beatCoroutine;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }
    
        Instance = this;
        var images = gameObject.GetComponentsInChildren<Image>(true);
        image = images[0];
        onBeatImage = images[1].gameObject;
        offBeatImage = images[2].gameObject;

    }

    void Update()
    {
        
    }

    public void SetBeatInput(SpriteType type)
    {
        StartCoroutine(BeatInput(type));
    }

    public void StartBeatCoroutine()
    {
        beatCoroutine = StartCoroutine(AnimateBeat());
    }
    
    private IEnumerator AnimateBeat()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(0.041f);
        }
        image.sprite = sprites[0];
    }
    private IEnumerator BeatInput(SpriteType type)
    {
        inProgress = true;
        if (type == SpriteType.ON_BEAT_INPUTTED)
        {
            onBeatImage.SetActive(true);
            offBeatImage.SetActive(false);
        }
        else
        {
            onBeatImage.SetActive(false);
            offBeatImage.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        onBeatImage.SetActive(false);
        offBeatImage.SetActive(false);
        inProgress = false;
    }

    void OnDestroy()
    {
        StopCoroutine(beatCoroutine);
    }
}
