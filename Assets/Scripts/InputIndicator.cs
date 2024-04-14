using UnityEngine;
using UnityEngine.UI;


public class InputIndicator : MonoBehaviour
{

    public enum SpriteType
    {
        ON_BEAT,
        OFF_BEAT,
        ON_BEAT_INPUTTED,
        OFF_BEAT_INPUTTED
    }
    
    public static InputIndicator Instance { get; private set; }
    private Image displayImage;
    [SerializeField] private Sprite[] sprites;
    public SpriteType type = SpriteType.ON_BEAT;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }
    
        Instance = this;
        displayImage = GetComponent<Image>();
       
    }

    void Update()
    {
         displayImage.sprite = type switch
        {
            SpriteType.ON_BEAT => sprites[0],
            SpriteType.OFF_BEAT => sprites[1],
            SpriteType.ON_BEAT_INPUTTED => sprites[2],
            SpriteType.OFF_BEAT_INPUTTED => sprites[3],
            _ => displayImage.sprite
        };

        displayImage.SetNativeSize();
    }
}
