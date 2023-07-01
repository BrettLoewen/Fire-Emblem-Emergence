using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Used to track the state of the main menu
/// 
/// Note: this class has commented out calls to the SaveSystem class. These calls have been commented out
/// to remove persistent saving from the game as I figured it was more trouble than it was worth. I have
/// left the code in the project in case my future self wants to further develop this project or use the
/// code for a different project
/// </summary>
public enum MainMenuState { Selection, Load, NewGame, Options, Loading, Controls, Credits }

/// <summary>
/// Used to operate and control the main menu scene
/// </summary>
public class MainMenu: MonoBehaviour
{
    #region Variables

    [SerializeField] private InputSystemUIInputModule inputModule; // Used to get input

    // Screens
    [SerializeField] private GameObject mainScreen;
    //[SerializeField] private SaveFileScreen saveFileScreen;
    //[SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject creditsScreen;

    // Screen navigation buttons
    //[SerializeField] private GameObject loadButton;
    //[SerializeField] private GameObject newGameButton;
    //[SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject creditsButton;

    private MainMenuState menuState; // Used to track the menu's state

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    async void Awake()
    {
        // Start in the selection state
        menuState = MainMenuState.Selection;

        // Load in the persistent scene if it hasn't already been loaded in
        await LevelManager.LoadPersistentScene(Scenes.MainMenu);
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        // State that the loading has finished so the loading screen can be removed
        await LevelManager.Instance.SetLoadingFinished();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // If the player pressed cancel
        if(inputModule.cancel.action.triggered)
        {
            // If the player is on the Load, NewGame, or Options screens, bring them back to the selection screen
            if(menuState != MainMenuState.Selection && menuState != MainMenuState.Loading)
            {
                SwitchToSelectionMenu(menuState);
            }
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region OnClick Methods for Buttons

    /// <summary>
    /// Called by the Play button to start the game
    /// </summary>
    public void OnPlayClick()
    {
        //// Check whether or not the game has a current save file
        //bool hasSaveFile = await SaveSystem.HasCurrentSaveFile();

        //// If the game does have a current save file, load according to that save file
        //if(hasSaveFile)
        //{
        //    menuState = MainMenuState.Loading;
        //    LevelManager.Instance.LoadScene(Scenes.HubWorld);
        //}

        menuState = MainMenuState.Loading;
        LevelManager.Instance.LoadScene(Scenes.HubWorld);
    }//end OnPlayClick

    /// <summary>
    /// Called by the Load button to switch to the load save file screen
    /// </summary>
    //async public void OnLoadClick()
    //{
    //    // Open the save file screen in load mode
    //    menuState = MainMenuState.Load;
    //    mainScreen.SetActive(false);
    //    await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Load);
    //}//end OnLoadClick

    ///// <summary>
    ///// Called by the New Game button to switch to the new game save file scren
    ///// </summary>
    //async public void OnNewGameClick()
    //{
    //    // Open the save file screen in new game mode
    //    menuState = MainMenuState.NewGame;
    //    mainScreen.SetActive(false);
    //    await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.NewGame);
    //}//end OnNewGameClick

    /// <summary>
    /// Called by the Options button to switch to the options screen
    /// </summary>
    //public void OnOptionsClick()
    //{
    //    // Open the options screen
    //    menuState = MainMenuState.Options;
    //    mainScreen.SetActive(false);
    //    optionsScreen.SetActive(true);
    //}//end OnOptionsClick

    /// <summary>
    /// Called by the Controls button to switch to the controls screen
    /// </summary>
    public void OnControlsClick()
    {
        // Open the options screen
        menuState = MainMenuState.Controls;
        mainScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }//end OnControlsClick

    /// <summary>
    /// Called by the Credits button to switch to the credits screen
    /// </summary>
    public void OnCreditsClick()
    {
        // Open the options screen
        menuState = MainMenuState.Credits;
        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }//end OnCreditsClick

    /// <summary>
    /// Called by the Quit button to close the game
    /// </summary>
    public void OnQuitClick()
    {
        // Quit out of the game
        Debug.Log("Quitting...");
        Application.Quit();
    }//end OnQuitClick

    #endregion

    /// <summary>
    /// Used to reset the menu to the selection screen
    /// </summary>
    /// <param name="state"></param>
    private void SwitchToSelectionMenu(MainMenuState state)
    {
        // If the save file screen was open, close it
        //if(state == MainMenuState.Load || state == MainMenuState.NewGame)
        //{
        //    saveFileScreen.CloseSaveFileScreen();
        //}

        //// If the menu was on the options screen, close it
        //if(state == MainMenuState.Options)
        //{
        //    optionsScreen.SetActive(false);
        //}

        // If the menu was on the controls screen, close it
        if (state == MainMenuState.Controls)
        {
            controlsScreen.SetActive(false);
        }

        // If the menu was on the credits screen, close it
        if (state == MainMenuState.Credits)
        {
            creditsScreen.SetActive(false);
        }

        // Enable the selection screen
        mainScreen.SetActive(true);

        // Set the current selection button according to the previous state 
        switch(state)
        {
            //case MainMenuState.Load:
            //    EventSystem.current.SetSelectedGameObject(loadButton);
            //    break;
            //case MainMenuState.NewGame:
            //    EventSystem.current.SetSelectedGameObject(newGameButton);
            //    break;
            //case MainMenuState.Options:
            //    EventSystem.current.SetSelectedGameObject(optionsButton);
            //    break;
            case MainMenuState.Controls:
                EventSystem.current.SetSelectedGameObject(controlsButton);
                break;
            case MainMenuState.Credits:
                EventSystem.current.SetSelectedGameObject(creditsButton);
                break;
        }

        // Update the state to be correct
        menuState = MainMenuState.Selection;
    }//end SwitchToSelectionMenu
}