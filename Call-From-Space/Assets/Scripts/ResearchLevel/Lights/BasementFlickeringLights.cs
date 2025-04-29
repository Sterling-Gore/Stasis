using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementFlickeringLights : MonoBehaviour
{
    public Light[] targetLights;
    public Renderer[] targetLightCones;
    //public Material targetMaterial;

    private float originalSpotAngle;
    private Color originalColor;
    Color color;
    private bool flickeringOff = true;

    public bool isComplete = false;

    public float duration = 2f;

    [Header("Lights On")]
    public Material lightsOnMaterial;
    public Color lightsOnColor ;
    // Start is called before the first frame update
    void Start()
    {
        originalSpotAngle = targetLights[0].spotAngle;
        originalColor = targetLightCones[0].material.color;
        color = originalColor;
        if(isComplete)
        {
            CompleteLights();
        }
        else
        {

            StartCoroutine(FlickerLoop());
        }
    }

    private System.Collections.IEnumerator FlickerLoop()
    {
        while (!isComplete)
        {
            yield return Flicker(flickeringOff);
            flickeringOff = !flickeringOff;
        }
    }

    private System.Collections.IEnumerator Flicker(bool toOff)
    {
        //float duration = 3f;
        float time = 0f;

        float startAngle = toOff ? originalSpotAngle : 1f;
        float endAngle = toOff ? 1f : originalSpotAngle;

        float startAlpha = toOff ? originalColor.a : 0f;
        float endAlpha = toOff ? 0f : originalColor.a;

        while (time < duration && !isComplete)
        {
            float t = time / duration;
            t = Mathf.SmoothStep(0, 1, t);

            // Lerp spot angle
            foreach( Light targetLight in targetLights)
                targetLight.spotAngle = Mathf.Lerp(startAngle, endAngle, t);

            // Lerp material alpha
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            foreach( Renderer targetLightCone in targetLightCones)
                targetLightCone.material.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        if(!isComplete)
        {
            foreach( Light targetLight in targetLights)
                targetLight.spotAngle = endAngle;
            Color finalColor = color;
            finalColor.a = endAlpha;
            foreach( Renderer targetLightCone in targetLightCones)
                targetLightCone.material.color = finalColor;
        }
    }

    public void CompleteLights()
    {
        isComplete = true;
        foreach( Light targetLight in targetLights)
        {
            targetLight.spotAngle = originalSpotAngle;
            targetLight.color = lightsOnColor;
        }
        foreach( Renderer targetLightCone in targetLightCones)
            targetLightCone.material = lightsOnMaterial;
    }
}
