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

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override async void Awake()
    {
        base.Awake();

        await LoadMainMenu();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    private async Task LoadMainMenu()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)Scenes.MainMenu, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            await Task.Yield();
        }

        sceneCamera.SetActive(false);
        eventSystem.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;

        LeanTween.alphaCanvas(loadingScreen, 0f, FADE_TIME);
        await Task.Delay((int)(FADE_TIME * 1000f));
    }

    #endregion
}