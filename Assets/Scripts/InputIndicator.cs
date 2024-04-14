using System;
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
    private bool inProgress = false;
    public bool startIndicator = false;
    private float time = 0.0f;
    private const float BeatTime = 0.041f;
    private int index = 0;
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
        UpdateHelper();
    }

    private void UpdateHelper()
    {
        if (startIndicator)
        {
            if (time >= BeatTime)
            {
                index++;
                time = 0.0f;
                image.sprite = sprites[index];
            }
            if (index >= sprites.Length - 1)
            {
                index = 0;
                startIndicator = false;
            }
        } else
        {
            image.sprite = sprites[0];
        }
        time += Time.deltaTime;
    }

    public void SetBeatInput(SpriteType type)
    {
        StartCoroutine(BeatInput(type));
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
}
