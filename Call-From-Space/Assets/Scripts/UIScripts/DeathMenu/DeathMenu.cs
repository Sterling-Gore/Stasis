using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    Interactor interactor;
    
    //public AudioMixer audioMixer;
    float volume = 100f;
    public float Duration = 4f;

    public SceneLoader sceneLoader;

    [Header("VideoPlayer")]
    public VideoPlayer videoPlayer; 
    public RawImage videoMaterial;  

    [Header("Audios")]
    public AudioSource StartAudio;
    public AudioSource LoopAudio;

    [Header("Buttons")]
    public GameObject lastCheckpoint;
    public GameObject backToStart;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(fadeOutVideo(Duration));
        StartCoroutine(audioChange());
    }

    IEnumerator audioChange()
    {
        yield return new WaitForSeconds(6.5f);
        LoopAudio.Play();
        StartAudio.Stop();
        //LoopAudio.Play();
    }

    IEnumerator fadeOutVideo(float duration)
    {
        float elapsedTime = 0f;
        Color materialColor = videoMaterial.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            videoMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);
            yield return null;
        }

        videoMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0f);
        videoPlayer.Stop(); // Stop the video when fully faded out
        lastCheckpoint.SetActive(true);
        backToStart.SetActive(true);
    }

    public void GoToCheckPoint()
    {
        //GameStateManager.instance.LoadGame(GameStateManager.checkPointFilePath);
        //gameObject.SetActive(false);
        //interactor.inUI = false;
        sceneLoader.LoadSceneFromSavePoint();
    }

    public void ExitGame()
    {
        //gameObject.SetActive(false);
        SceneManager.LoadScene("Start");
    }
}
