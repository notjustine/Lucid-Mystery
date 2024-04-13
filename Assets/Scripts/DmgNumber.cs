using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DmgNumber : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    private float fadeSpeed = 18f;
    [SerializeField] private Sprite[] dmgSprite;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    private IEnumerator MoveUpAndFade()
    {
        while (image.color.a > 0)
        {
            Vector3.Lerp(transform.position, transform.position + new Vector3(0, 5, 0), fadeSpeed * Time.deltaTime);
            // gameObject.transform.position += new Vector3(0, 5f, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (fadeSpeed * Time.deltaTime));
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMovement(int combo, Vector3 position)
    {
        // transform.position = position + new Vector3(800, 400,0);
        float xOffset = Screen.width * 0.4f; // 40% of screen width
        float yOffset = Screen.height * 0.4f; // 40% of screen height
        transform.position = position + new Vector3(xOffset, yOffset, 0);
        image.sprite = dmgSprite[combo-1];
        
        image.SetNativeSize();
        StartCoroutine(MoveUpAndFade());
    }
}
