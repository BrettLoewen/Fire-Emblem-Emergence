using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public enum MainMenuState { Selection, Load, NewGame, Options, Loading }

public class MainMenu: MonoBehaviour
{
    #region Variables

    [SerializeField] private InputSystemUIInputModule inputModule;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private SaveFileScreen saveFileScreen;
    [SerializeField] private GameObject optionsScreen;

    [SerializeField] private GameObject loadButton;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject optionsButton;

    private MainMenuState menuState;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    async void Awake()
    {
        menuState = MainMenuState.Selection;

        await LevelManager.LoadPersistentScene(Scenes.MainMenu);
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        await LevelManager.Instance.SetLoadingFinished();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // If the player pressed cancel
        if(inputModule.cancel.action.triggered)
        {
            if(menuState == MainMenuState.Load || menuState == MainMenuState.NewGame || menuState == MainMenuState.Options)
            {
                SwitchToSelectionMenu(menuState);
            }
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    public async void OnContinueClick()
    {
        Debug.Log("Loading the current save file");

        bool hasSaveFile = await SaveSystem.HasCurrentSaveFile();

        if(hasSaveFile)
        {
            menuState = MainMenuState.Loading;
            LevelManager.Instance.LoadScene(Scenes.HubWorld);
        }
    }

    async public void OnLoadClick()
    {
        Debug.Log("Opening the save file screen so you can choose which file you want to load");

        menuState = MainMenuState.Load;
        mainScreen.SetActive(false);
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Load);
    }

    async public void OnNewGameClick()
    {
        Debug.Log("Opening the save file screen so you can a file to write a new game save to");

        menuState = MainMenuState.NewGame;
        mainScreen.SetActive(false);
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.NewGame);
    }

    public void OnOptionsClick()
    {
        Debug.Log("Opening the options screen");

        menuState = MainMenuState.Options;
        mainScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OnQuitClick()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    private void SwitchToSelectionMenu(MainMenuState state)
    {
        menuState = state;

        if(state == MainMenuState.Load || state == MainMenuState.NewGame)
        {
            saveFileScreen.CloseSaveFileScreen();
        }

        if(state == MainMenuState.Options)
        {
            optionsScreen.SetActive(false);
        }

        mainScreen.SetActive(true);

        switch(state)
        {
            case MainMenuState.Load:
                EventSystem.current.SetSelectedGameObject(loadButton);
                break;
            case MainMenuState.NewGame:
                EventSystem.current.SetSelectedGameObject(newGameButton);
                break;
            case MainMenuState.Options:
                EventSystem.current.SetSelectedGameObject(optionsButton);
                break;
        }
    }

    #endregion
}