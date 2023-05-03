using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public enum Scenes { Persistent = 0, MainMenu = 1, HubWorld = 2 }

public class LevelManager: Singleton<LevelManager>
{
    #region Variables

    [SerializeField] private CanvasGroup loadingScreen;
    private const float FADE_TIME = 0.25f;

    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject eventSystem;

    private Scenes currentScene;

    private bool isLoading;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();

        currentScene = Scenes.Persistent;
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        await Task.Delay(100);
        if(currentScene == Scenes.Persistent)
        {
            await LoadMainMenu();
        }
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    private async Task LoadMainMenu()
    {
        isLoading = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)Scenes.MainMenu, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            await Task.Yield();
        }

        sceneCamera.SetActive(false);
        eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        Instance.currentScene = Scenes.MainMenu;

        while(isLoading)
        {
            await Task.Yield();
        }

        LeanTween.alphaCanvas(loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }

    public static async Task LoadPersistentScene(Scenes loadedFromScene)
    {
        if (Instance != null)
        {
            return;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)Scenes.Persistent, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            await Task.Yield();
        }

        Instance.isLoading = true;

        Instance.sceneCamera.SetActive(false);
        Instance.eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        Instance.currentScene = loadedFromScene;

        while (Instance.isLoading)
        {
            await Task.Yield();
        }

        LeanTween.alphaCanvas(Instance.loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }

    public async Task SetLoadingFinished()
    {
        await Task.Delay((int)(FADE_TIME * 1000f));
        isLoading = false;
    }

    public async void LoadScene(Scenes sceneToLoad)
    {
        isLoading = true;

        LeanTween.alphaCanvas(loadingScreen, 1f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));

        sceneCamera.SetActive(true);

        if (currentScene != Scenes.Persistent)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync((int)currentScene);

            while (unloadOperation.isDone == false)
            {
                await Task.Yield();
            }
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive);

        while (loadOperation.isDone == false)
        {
            await Task.Yield();
        }

        sceneCamera.SetActive(false);
        eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        Instance.currentScene = sceneToLoad;

        while (isLoading)
        {
            await Task.Yield();
        }

        LeanTween.alphaCanvas(loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }

    #endregion
}