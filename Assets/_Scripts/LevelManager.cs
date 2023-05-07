using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

/// <summary>
/// Used to map scene indecies to a readable name that can be easily updated
/// </summary>
public enum Scenes { Persistent = 0, MainMenu = 1, HubWorld = 2 }

/// <summary>
/// Used to manage scene loading and switching
/// </summary>
public class LevelManager: Singleton<LevelManager>
{
    #region Variables

    // Loading screen
    [SerializeField] private CanvasGroup loadingScreen;
    private const float FADE_TIME = 0.25f;  // The time for the loading screen to fade in/out

    // Variables in the persistent scene that need to be enabled/disabled during the loading sequence
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject eventSystem;

    private Scenes currentScene; // The current (primary) scene

    private bool isLoading; // Tracks whether or not the game is currently loading

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        // Setup the singleton base class
        base.Awake();

        // When the game starts, just the persistent scene is open
        currentScene = Scenes.Persistent;
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        // If the game is in the persistent scene for long enough, load the main menu scene
        await Task.Delay(100);
        if(currentScene == Scenes.Persistent)
        {
            LoadScene(Scenes.MainMenu);
        }
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region Scene Management

    /// <summary>
    /// Used to load the persistent scene if the game starts in a different scene.
    /// Exists for development purposes, not needed for production.
    /// NOTE: NOT meant to be awaited
    /// </summary>
    /// <param name="_loadedFromScene">The scene that was already loaded</param>
    /// <returns></returns>
    public static async Task LoadPersistentScene(Scenes _loadedFromScene)
    {
        // If the persistent scene is already loaded, this is unnecessary, so return
        if (Instance != null)
        {
            return;
        }

        // Start loading in the persistent scene
        AsyncOperation _operation = SceneManager.LoadSceneAsync((int)Scenes.Persistent, LoadSceneMode.Additive);

        // Wait until the persistent scene is loaded in
        while (_operation.isDone == false)
        {
            await Task.Yield();
        }

        Instance.isLoading = true;

        Instance.sceneCamera.SetActive(false);
        Instance.eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        Instance.currentScene = _loadedFromScene;

        // Wait until the other scene says loading is done
        while (Instance.isLoading)
        {
            await Task.Yield();
        }

        // Fade out the loading screen
        LeanTween.alphaCanvas(Instance.loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }//end LoadPersistentScene

    /// <summary>
    /// Used to end the loading screen from another script
    /// </summary>
    /// <returns></returns>
    public async Task SetLoadingFinished()
    {
        // Fade out the loading screen
        await Task.Delay((int)(FADE_TIME * 1000f));
        isLoading = false;
    }//end SetLoadingFinished

    /// <summary>
    /// Load the passed scene and unload the previous scene
    /// </summary>
    /// <param name="_sceneToLoad"></param>
    public async void LoadScene(Scenes _sceneToLoad)
    {
        // Mark that loading is happening
        isLoading = true;

        // Fade in the loading screen
        LeanTween.alphaCanvas(loadingScreen, 1f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));

        // Enable the loading screen camera so there will always be at least one camera
        sceneCamera.SetActive(true);

        // As long as the previous scene was not the persistent scene, unload it
        if (currentScene != Scenes.Persistent)
        {
            AsyncOperation _unloadOperatoin = SceneManager.UnloadSceneAsync((int)currentScene);

            // Wait until the scene has been unloaded before continuing
            while (_unloadOperatoin.isDone == false)
            {
                await Task.Yield();
            }
        }

        // Load the new scene
        AsyncOperation _loadOperation = SceneManager.LoadSceneAsync((int)_sceneToLoad, LoadSceneMode.Additive);

        // Wait until the new scene is loaded in before continuing
        while (_loadOperation.isDone == false)
        {
            await Task.Yield();
        }

        // Disable the loading screen now that loading has concluded
        sceneCamera.SetActive(false);
        eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        // Mark that the current scene is the one that was just loaded into
        Instance.currentScene = _sceneToLoad;

        // Wait until the new scene marks loading as completed
        // Gives the scene time to do its own setup
        while (isLoading)
        {
            await Task.Yield();
        }

        // Fade out the loading screen
        LeanTween.alphaCanvas(loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }//end LoadScene

    #endregion
}