using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class DeathMenu : MonoBehaviour
{
    Interactor interactor;
    public AudioSource ambiance;
    public AudioSource VHSstatic;
    //public AudioMixer audioMixer;
    float volume = 100f;

    public SceneLoader sceneLoader;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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
