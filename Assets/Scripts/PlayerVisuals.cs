using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private List<string> playerGeos;
    private List<string> flashing;
    [SerializeField] Color flashColor;
    [SerializeField] Color toolColor;
    [SerializeField] Color vestColor;
    [SerializeField] Color pantsColor;
    [SerializeField] Color blouseColor;

    private const float BLINK_SPEED = 8f;
    private const float BLINK_DELAY = 0.1667f;
    private GameObject tempObject;
    private Renderer tempRenderer;
    private MaterialPropertyBlock propBlock;
    private float sinceHit;


    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        flashing = new List<string>();
        InitPlayerGeos();
        sinceHit = 0f;
    }


    void Update()
    {
        sinceHit += Time.deltaTime;
        Color initialColor = vestColor;
        foreach (string name in flashing)
        {
            tempObject = GameObject.Find(name);
            tempRenderer = tempObject.GetComponent<Renderer>();

            if (name == "vest_GEO")
            {
                initialColor = vestColor;
                propBlock = new MaterialPropertyBlock();
                tempRenderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", Color.Lerp(initialColor, flashColor, Mathf.PingPong(sinceHit * BLINK_SPEED, 1)));
                tempRenderer.SetPropertyBlock(propBlock);
            }
            else if (name == "wrenchTextured")
            {
                initialColor = toolColor;
                propBlock = new MaterialPropertyBlock();
                tempRenderer.GetPropertyBlock(propBlock, 1);
                propBlock.SetColor("_BaseColor", Color.Lerp(initialColor, flashColor, Mathf.PingPong(sinceHit * BLINK_SPEED, 1)));
                tempRenderer.SetPropertyBlock(propBlock, 1);
            }
            else if (name == "blouse_GEO")
            {
                initialColor = blouseColor;
                propBlock = new MaterialPropertyBlock();
                tempRenderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", Color.Lerp(initialColor, flashColor, Mathf.PingPong(sinceHit * BLINK_SPEED, 1)));
                tempRenderer.SetPropertyBlock(propBlock);
            }
            else if (name == "pants_GEO")
            {
                initialColor = pantsColor;
                propBlock = new MaterialPropertyBlock();
                tempRenderer.GetPropertyBlock(propBlock, 0);
                propBlock.SetColor("_BaseColor", Color.Lerp(initialColor, flashColor, Mathf.PingPong(sinceHit * BLINK_SPEED, 1)));
                tempRenderer.SetPropertyBlock(propBlock, 0);
            }
        }
    }


    public void FlashDamageColor()
    {
        flashing.AddRange(playerGeos);
        playerStatus.isInvincible = true;
        sinceHit = 0;
        StartCoroutine(DisableFlashAfterDelay());
    }


    private void InitPlayerGeos()
    {
        playerGeos = new List<string>
        {
            "vest_GEO",
            "pants_GEO",
            "blouse_GEO",
            "wrenchTextured"
        };
    }


    IEnumerator DisableFlashAfterDelay()
    {
        yield return new WaitForSeconds(BLINK_DELAY);
        flashing.Clear();
        playerStatus.isInvincible = false;
        // set back to original color
        Color initialColor = vestColor;
        foreach (string name in playerGeos)
        {
            tempObject = GameObject.Find(name);
            Renderer renderer = tempObject.GetComponent<Renderer>();
            if (name == "wrenchTextured")
            {
                initialColor = toolColor;
                propBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propBlock, 1);
                propBlock.SetColor("_BaseColor", initialColor);
                renderer.SetPropertyBlock(propBlock, 1);
            }
            else if (name == "vest_GEO")
            {
                initialColor = vestColor;
                propBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", initialColor);
                renderer.SetPropertyBlock(propBlock);
            }
            else if (name == "pants_GEO")
            {
                initialColor = pantsColor;
                propBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propBlock, 0);
                propBlock.SetColor("_BaseColor", initialColor);
                renderer.SetPropertyBlock(propBlock, 0);
            }
            else if (name == "blouse_GEO")
            {
                initialColor = blouseColor;
                propBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", initialColor);
                renderer.SetPropertyBlock(propBlock);
            }
        }
    }
}
