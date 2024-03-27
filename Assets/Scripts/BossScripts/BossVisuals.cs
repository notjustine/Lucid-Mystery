using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    private List<string> bossGeos;
    private List<string> flashing;
    [SerializeField] Color flashColor;
    [SerializeField] Color bossColor;

    private const float BLINK_SPEED = 0.06f;
    private GameObject tempObject;
    private Renderer tempRenderer;
    private MaterialPropertyBlock propBlock;


    // Start is called before the first frame update
    void Start()
    {
        flashing = new List<string>();
        InitBossGeos();
    }


    void Update()
    {
        foreach (string name in flashing)
        {
            tempObject = GameObject.Find(name);
            tempRenderer = tempObject.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
            tempRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", Color.Lerp(flashColor, bossColor, Mathf.PingPong(Time.time, BLINK_SPEED)));
            tempRenderer.SetPropertyBlock(propBlock);
        }
    }


    public void FlashDamageColor()
    {
        flashing.AddRange(bossGeos);
        StartCoroutine(DisableFlashAfterDelay());
    }


    private void InitBossGeos()
    {
        bossGeos = new List<string>
        {
            "baseBody_GEO",
            "topBody_GEO",
            "rotatableHead_GEO",
            "turretBody_GEO"
        };

        // Add 'bars'
        for (int i = 1; i <= 12; i++)
        {
            bossGeos.Add($"bars_GEO{i}");
        }

        // Add arms
        for (int i = 1; i <= 4; i++)
        {
            bossGeos.Add($"armL{i}_GEO");
            bossGeos.Add($"armR{i}_GEO");
        }
    }


    IEnumerator DisableFlashAfterDelay()
    {
        yield return new WaitForSeconds(BLINK_SPEED);
        flashing.Clear();

        // set back to original color
        foreach (string name in bossGeos)
        {
            tempObject = GameObject.Find(name);
            Renderer renderer = tempObject.GetComponent<Renderer>();
            propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", bossColor);
            renderer.SetPropertyBlock(propBlock);
        }
    }
}