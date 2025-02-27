using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextFadePulse : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = .75f;
    public Color startColor = Color.white;
    public Color targetColor = new Color(.5f, .5f, .5f, 0f); // grey and fully transparent
    
    void Start()
    {
        //if(text != null || image != null)
        //    StartCoroutine(FadeLoop());
    }

    void OnEnable()
    {
        if(text != null || image != null)
            StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            if(text != null)
            {
                yield return StartCoroutine(FadeText(targetColor, fadeOutDuration));
                //yield return new WaitForSeconds(2f);
                yield return StartCoroutine(FadeText(startColor, fadeInDuration));
                //yield return new WaitForSeconds(2f);
            }
            if(image != null)
            {
                yield return StartCoroutine(FadeImage(targetColor, fadeOutDuration));
                //yield return new WaitForSeconds(2f);
                yield return StartCoroutine(FadeImage(startColor, fadeInDuration));
                //yield return new WaitForSeconds(2f);
            }
            
        }
    }

    IEnumerator FadeText(Color target, float duration)
    {
        Color initialColor = text.color;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            text.color = Color.Lerp(initialColor, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        text.color = target;
    }

    IEnumerator FadeImage(Color target, float duration)
    {
        Color initialColor = image.color;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(initialColor, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = target;
    }
}
