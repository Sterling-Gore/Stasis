using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class introStart : MonoBehaviour
{
    public SaveManager saveManager;
    AsyncOperation sceneLoadOperation;
    bool sceneLoaded = false;
    bool skipReady = false;
    public GameObject skipButton;
    public VideoPlayer videoPlayer;

    public GameObject optionsMenuUI;
    bool wasPlayingBeforePause = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneInBackground("tutorial"));
        StartCoroutine(WaitTenSeconds());
    }

    private IEnumerator LoadSceneInBackground(string sceneName)
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        sceneLoadOperation.allowSceneActivation = false; // Prevents automatic switching
        
        
        while (!sceneLoadOperation.isDone)
        {
            Debug.Log($"Loading Progress: {sceneLoadOperation.progress * 100}%");

            // Scene is fully loaded, waiting for activation
            if (sceneLoadOperation.progress >= 0.9f)
            {
                sceneLoaded = true;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator WaitTenSeconds()
    {
        yield return new WaitForSeconds(10f);
        skipReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneLoaded && skipReady)
        {
            skipButton.SetActive(true);
        }
        if (videoPlayer != null && !videoPlayer.isPlaying && videoPlayer.time >= videoPlayer.length - 0.1)
        {
            Debug.Log("Video has ended!");
            endScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            optionsMenuUI.SetActive(!optionsMenuUI.activeSelf);
        }

        if(optionsMenuUI.activeSelf && !wasPlayingBeforePause)
        {
            videoPlayer.Pause();
            wasPlayingBeforePause = true;
        }
        if(!optionsMenuUI.activeSelf && wasPlayingBeforePause == true)
        {
            videoPlayer.Play();
            wasPlayingBeforePause = false;
        }
    }
    

    public void endScene()
    {
        Debug.Log("ENDING");
        if (sceneLoadOperation != null && sceneLoadOperation.progress >= 0.9f)
        {
            saveManager.UpdateSave(SavePointID.tutorial);
            sceneLoadOperation.allowSceneActivation = true;
        }
    }
}
