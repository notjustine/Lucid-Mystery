using UnityEngine;
using UnityEngine.UI;

public class TutorialInstruction : MonoBehaviour
{
    public enum SpriteType
    {
        Start,
        OnBeat,
        Sniper,
        Heal,
        ApproachMachine,
        Attack,
        Strengthen
    }

    public static TutorialInstruction Instance { get; private set; }
    private Image displayImage;
    [SerializeField] private Sprite[] sprites;
    public SpriteType type = SpriteType.Start;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one TutorialInstruction");
        }

        Instance = this;
        displayImage = GetComponent<Image>();
    }

    public void SetInstructionType(SpriteType newType)
    {
        type = newType;
    }

    void Update()
    {
        if (!displayImage) displayImage = GetComponent<Image>();
        displayImage.sprite = type switch
        {
            SpriteType.Start => sprites[0],
            SpriteType.OnBeat => sprites[1],
            SpriteType.Sniper => sprites[2],
            SpriteType.Heal => sprites[3],
            SpriteType.ApproachMachine => sprites[4],
            SpriteType.Attack => sprites[5],
            SpriteType.Strengthen => sprites[6],
            _ => displayImage.sprite
        };

        displayImage.SetNativeSize();
    }
}
