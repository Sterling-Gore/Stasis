using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leavingWorkshop : MonoBehaviour
{
     public AudioSource Experiment87IdleSounds;
     public AudioSource Experiment87AttackSounds;
     public AudioSource Experiment87WalkSounds;
     public AlienController Experiment87;

    public void leaveScene()
    {
        Experiment87.updateAudio = false;
        StartCoroutine(FadeOutAudio(Experiment87IdleSounds));
        StartCoroutine(FadeOutAudio(Experiment87AttackSounds));
        StartCoroutine(FadeOutAudio(Experiment87WalkSounds));
        StartCoroutine(LeavingGame());
        
    }

    IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;
        yield return new WaitForSeconds(4f);
        while (elapsedTime < 4)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / 4);
            yield return null;
        }

        audioSource.volume = 0f; // Ensure volume is completely off
        
    }

    IEnumerator LeavingGame()
    {
        yield return new WaitForSeconds(15f);
        SceneManager.LoadSceneAsync("TOBECONTINUED");
    }
}
