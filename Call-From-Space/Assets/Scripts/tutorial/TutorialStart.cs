using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialStart : MonoBehaviour
{
    public Image blackscreen;
    public CameraShakeGeneral cameraShake;
    public AI_Tutorial_Sounds AI_Sounds;
    // Start is called before the first frame update
    void Awake()
    {
        AI_Sounds.PlayExplosionSpawn();
        StartCoroutine(FadeIn());
        StartCoroutine(Shake());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        while (blackscreen.color.a > 0f)
        {
            Color currentColor = blackscreen.color;
            currentColor.a -= .25f * Time.deltaTime;
            blackscreen.color = currentColor;

            yield return null;
        }
        blackscreen.enabled = false;
    }
    IEnumerator Shake()
    {
        cameraShake.StartShake(1f, 0.6f);
        yield return new WaitForSeconds(1f);
        cameraShake.StartShake(2f, 0.3f);
        yield return new WaitForSeconds(2f);
        cameraShake.StartShake(2f, 0.15f);
        yield return new WaitForSeconds(2f);
        yield return null;
    }
}
