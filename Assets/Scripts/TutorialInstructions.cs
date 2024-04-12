using UnityEngine;
using UnityEngine.UI;

public class TutorialInstructions : MonoBehaviour
{
    public enum SpriteType
    {
        Start,
        OnBeat,
        Heal,
        ApproachMachine,
        Attack,
        Strengthen,
        End
    }

    private Image displayImage;
    [SerializeField] private Sprite[] sprites;
    public SpriteType type = SpriteType.Start;

    void Update()
    {
        displayImage.sprite = type switch
        {
            SpriteType.Start => sprites[0],
            SpriteType.OnBeat => sprites[1],
            SpriteType.Heal => sprites[2],
            SpriteType.ApproachMachine => sprites[3],
            SpriteType.Attack => sprites[4],
            SpriteType.Strengthen => sprites[5],
            SpriteType.End => sprites[6],
            _ => displayImage.sprite
        };

        displayImage.SetNativeSize();
    }
}
