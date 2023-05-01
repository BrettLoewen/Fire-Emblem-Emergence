using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public enum ExplorationState { Setup, Explore, Menu }
public enum MenuState { Selection, Save, Load }

public class ExplorationGameManager: Singleton<ExplorationGameManager>
{
    #region Variables

    [SerializeField] private PlayerManager player;
    private PlayerInputHandler playerInput;

    public ExplorationState explorationState { get; private set; }
    private MenuState menuState;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject loadButton;

    [SerializeField] private SaveFileScreen saveFileScreen;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override async void Awake()
    {
        base.Awake();

        explorationState = ExplorationState.Setup;

        playerInput = player.inputHandler;

        // Make sure the save system loads the current save file
        await SaveSystem.LoadCurrentSaveFile();
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        // Tell the player to set itself up according to loaded save data
        await player.Setup();

        // Once as setup has ended, exploration can begin
        explorationState = ExplorationState.Explore;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // If the player pressed the menu button
        if(playerInput.menuInput)
        {
            // Cancel the button press
            playerInput.menuInput = false;

            // If the game was in exploration mode, switch to menu mode
            if(explorationState == ExplorationState.Explore)
            {
                explorationState = ExplorationState.Menu;
                menuState = MenuState.Selection;
                Time.timeScale = 0f;
            }
            // If the game was in menu mode, switch to exploration mode
            else if(explorationState == ExplorationState.Menu)
            {
                explorationState = ExplorationState.Explore;
                Time.timeScale = 1f;
            }
        }

        // If the player pressed the cancel button
        if (playerInput.cancelInput)
        {
            // Cancel the button press
            playerInput.cancelInput = false;

            switch(menuState)
            {
                case MenuState.Load:
                    menuState = MenuState.Selection;
                    saveFileScreen.CloseSaveFileScreen();
                    EventSystem.current.SetSelectedGameObject(loadButton);
                    break;
                case MenuState.Save:
                    menuState = MenuState.Selection;
                    saveFileScreen.CloseSaveFileScreen();
                    EventSystem.current.SetSelectedGameObject(saveButton);
                    break;
                case MenuState.Selection:
                    menuState = MenuState.Selection;
                    explorationState = ExplorationState.Explore;
                    Time.timeScale = 1f;
                    break;
            }
        }

        // If the game is in menu mode, enable the menu
        pauseMenu.SetActive(explorationState == ExplorationState.Menu);
    }//end Update

    #endregion //end Unity Control Methods

    #region Pause Menu

    public async void Save()
    {
        //await SaveSystem.SaveData("save2");
        menuState = MenuState.Save;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Save);
    }

    public async void Load()
    {
        menuState = MenuState.Load;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Load);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    #endregion
}