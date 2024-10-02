using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSpriteOnHit : MonoBehaviour
{
    public Sprite hitSprite;  
    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeSprite()
    {
        if (spriteRenderer != null && hitSprite != null)
        {
            spriteRenderer.sprite = hitSprite;
            StartCoroutine(RevertSpriteAfterDelay(0.05f));  
        }
    }

    private IEnumerator RevertSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
