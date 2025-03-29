using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public SaveManager saveManager;
    public SavePointID TestSavePoint;
    public GameObject optionsMenuUI;
    public GameObject regularScreenUI;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PlayGame()
    {
        //GameStateManager.instance.NewGame();
        saveManager.UpdateSave(SavePointID.intro);
        //SceneManager.LoadSceneAsync("Tutorial");
        sceneLoader.LoadSceneFromSavePoint();
    }

    public void LoadGame()
    {
        sceneLoader.LoadSceneFromSavePoint();
        //GameStateManager.instance.LoadGame(GameStateManager.saveFilePath);
        //if (GameStateManager.startedNewGame)
        //    GameStateManager.instance.LoadGame(GameStateManager.checkPointFilePath);
        //SceneManager.LoadSceneAsync("Ship");
    }

    public void Options()
    {
        regularScreenUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void TestSave()
    {
        saveManager.UpdateSave(TestSavePoint);
        sceneLoader.LoadSceneFromSavePoint();
    }

    public void TestAI()
    {
        SceneManager.LoadScene(5);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            optionsMenuUI.SetActive(false);
            regularScreenUI.SetActive(true);
        }
    }
}
