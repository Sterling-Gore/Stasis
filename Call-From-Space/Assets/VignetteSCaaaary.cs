using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteSCaaaary : MonoBehaviour
{
    // Start is called before the first frame update
    Image vignette;
    float upperLimit;
    float lowerLimit;

    void Start()
    {
        vignette = GetComponent<Image>();
        StartCoroutine(VignettePulsing());
    }

    private void Update()
    {
        if (!InsanityMeter.Instance.acceptingInsanityIncrease)
            upperLimit = 1;
        else
            upperLimit = InsanityMeter.Instance.currentInsanity / 100;
        lowerLimit = Mathf.Clamp(upperLimit - 0.2f, 0f, 1f);
        
    }

    IEnumerator VignettePulsing()
    {
        while (true) 
        {
            int pulseUpIntervals = 20;
            int pulseDownIntervals = 40;
            


            for (int i = 0; i <= pulseUpIntervals; i++)
            {
                //Debug.Log(vignette);
                float alpha = Mathf.Lerp(lowerLimit, upperLimit, (float)i/pulseUpIntervals);
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                yield return new WaitForSeconds(0.01f);
            }

            for (int i = 0; i <= pulseDownIntervals; i++)
            {
                float alpha = Mathf.Lerp(upperLimit, lowerLimit, (float)i / pulseDownIntervals);
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
