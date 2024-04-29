using UnityEngine;
using UnityEngine.UI;

public class buttonSelect : MonoBehaviour
{
    public Button buttonU;
    public Button buttonI;
    public Button buttonJ;
    public Button buttonK;
    public Button buttonL;

    private ColorBlock[] originalColors = new ColorBlock[5];
    private bool toggleU = false;

    void Start()
    {
        // Store the original colors of the buttons
        originalColors[0] = buttonU.colors;
        originalColors[1] = buttonI.colors;
        originalColors[2] = buttonJ.colors;
        originalColors[3] = buttonK.colors;
        originalColors[4] = buttonL.colors;

        ColorBlock buttonUColors = buttonU.colors;
        buttonUColors.normalColor = originalColors[0].normalColor;
        ColorBlock buttonIColors = buttonI.colors;
        buttonIColors.normalColor = originalColors[1].normalColor;
        ColorBlock buttonJColors = buttonJ.colors;
        buttonJColors.normalColor = originalColors[2].normalColor;
        ColorBlock buttonKColors = buttonK.colors;
        buttonKColors.normalColor = originalColors[3].normalColor;
        ColorBlock buttonLColors = buttonL.colors;
        buttonLColors.normalColor = originalColors[4].normalColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!toggleU)
            {
                // Change the color of buttonU to its pressed color
                ColorBlock buttonUColors = buttonU.colors;
                buttonUColors.normalColor = buttonUColors.pressedColor;
                buttonU.colors = buttonUColors;
                toggleU = true;
            }
            else
            {
                ColorBlock buttonUColors = buttonU.colors;
                buttonUColors.normalColor = originalColors[0].normalColor;
                buttonU.colors = buttonUColors;
                toggleU = false;
            }
        }

        if (Input.GetKey(KeyCode.I))
        {
            ColorBlock buttonIColors = buttonI.colors;
            buttonIColors.normalColor = buttonIColors.pressedColor;
            buttonI.colors = buttonIColors;
        }
        else
        {
            ColorBlock buttonIColors = buttonI.colors;
            buttonIColors.normalColor = originalColors[1].normalColor;
            buttonI.colors = buttonIColors;
        }

        if (Input.GetKey(KeyCode.J))
        {
            ColorBlock buttonJColors = buttonJ.colors;
            buttonJColors.normalColor = buttonJColors.pressedColor;
            buttonJ.colors = buttonJColors;
        }
        else
        {
            ColorBlock buttonJColors = buttonJ.colors;
            buttonJColors.normalColor = originalColors[2].normalColor;
            buttonJ.colors = buttonJColors;
        }

        if (Input.GetKey(KeyCode.K))
        {
            ColorBlock buttonKColors = buttonK.colors;
            buttonKColors.normalColor = buttonKColors.pressedColor;
            buttonK.colors = buttonKColors;
        }
        else
        {
            ColorBlock buttonKColors = buttonK.colors;
            buttonKColors.normalColor = originalColors[2].normalColor;
            buttonK.colors = buttonKColors;
        }

        if (Input.GetKey(KeyCode.L))
        {
            ColorBlock buttonLColors = buttonL.colors;
            buttonLColors.normalColor = buttonLColors.pressedColor;
            buttonL.colors = buttonLColors;
        }
        else
        {
            ColorBlock buttonLColors = buttonL.colors;
            buttonLColors.normalColor = originalColors[2].normalColor;
            buttonL.colors = buttonLColors;
        }
    }
}
