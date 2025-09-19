using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCoverScript : MonoBehaviour
{
    public float fadeTime = 1.0f;
    float timer;
    bool fading;
    bool unfading;

    public SpriteRenderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fading || unfading)
        {
            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {
                timer = 0.0f;
                SetSpriteAlphas(fading ? 0.0f : 1.0f);
                fading = false;
                unfading = false;
            }
            else
            {
                SetSpriteAlphas(fading ? (timer / fadeTime) : (1.0f - (timer / fadeTime)));
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        float currentAlpha = renderers[0].color.a;
        timer = fadeTime * currentAlpha;
        fading = true;
        unfading = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        float currentAlpha = renderers[0].color.a;
        timer = fadeTime * (1.0f - currentAlpha);
        fading = false;
        unfading = true;
    }
    
    void SetSpriteAlphas(float alpha)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            Color c = renderer.color;
            c.a = alpha;
            renderer.color = c;
        }
    }
}
