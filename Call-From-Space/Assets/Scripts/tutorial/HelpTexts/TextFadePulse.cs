using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadePulse : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeDuration = 2f;
    private Color startColor = Color.white;
    private Color targetColor = new Color(.5f, .5f, .5f, 0f); // Red and fully transparent
    
    void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeText(targetColor, .75f));
            //yield return new WaitForSeconds(2f);
            yield return StartCoroutine(FadeText(startColor, fadeDuration));
            //yield return new WaitForSeconds(2f);
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
}
