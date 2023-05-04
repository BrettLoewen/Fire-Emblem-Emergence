using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

[System.Serializable]
public enum ExplorationState { Setup, Explore, Menu, Customization }
public enum ExplorationMenuState { Selection, Save, Load }

public class ExplorationGameManager: Singleton<ExplorationGameManager>
{
    #region Variables

    [SerializeField] private PlayerManager player;
    private PlayerInputHandler playerInput;

    public ExplorationState explorationState { get; private set; }
    private ExplorationMenuState menuState;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private SaveFileScreen saveFileScreen;

    [Header("Customization Menu")]
    [SerializeField] private GameObject customizationMenu;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private CustomizationMenuButton buttonPrefab;


    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override async void Awake()
    {
        base.Awake();

        explorationState = ExplorationState.Setup;

        playerInput = player.inputHandler;

#pragma warning disable CS4014
        LevelManager.LoadPersistentScene(Scenes.HubWorld);

        // Make sure the save system loads the current save file
        await SaveSystem.LoadCurrentSaveFile();
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        // Tell the player to set itself up according to loaded save data
        await player.Setup();

        await Task.Delay(100);

        await LevelManager.Instance.SetLoadingFinished();

        await Task.Delay(100);

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
                menuState = ExplorationMenuState.Selection;
                EventSystem.current.SetSelectedGameObject(saveButton);
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

            switch(explorationState)
            {
                case ExplorationState.Menu:
                    switch (menuState)
                    {
                        case ExplorationMenuState.Load:
                            menuState = ExplorationMenuState.Selection;
                            saveFileScreen.CloseSaveFileScreen();
                            EventSystem.current.SetSelectedGameObject(loadButton);
                            break;
                        case ExplorationMenuState.Save:
                            menuState = ExplorationMenuState.Selection;
                            saveFileScreen.CloseSaveFileScreen();
                            EventSystem.current.SetSelectedGameObject(saveButton);
                            break;
                        case ExplorationMenuState.Selection:
                            menuState = ExplorationMenuState.Selection;
                            explorationState = ExplorationState.Explore;
                            Time.timeScale = 1f;
                            break;
                    }
                    break;
                case ExplorationState.Customization:
                    CloseCustomizationMenu();
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
        menuState = ExplorationMenuState.Save;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Save);
    }

    public async void Load()
    {
        menuState = ExplorationMenuState.Load;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Load);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    #endregion

    #region Customization Menu

    public async void OpenCustomizationMenu()
    {
        customizationMenu.SetActive(true);
        explorationState = ExplorationState.Customization;

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Gives time for the children to be destroyed
        await Task.Yield();

        Customization[] customizationOptions = DataManager.GetCustomizationOptions();

        foreach (Customization option in customizationOptions)
        {
            CustomizationMenuButton button = Instantiate(buttonPrefab, buttonParent);
            button.Setup(option, this);
        }

        if (buttonParent.childCount > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttonParent.GetChild(0).gameObject);
        }

        Time.timeScale = 0f;
    }

    public void OnClickCustomizationOption(Customization customization)
    {
        player.SetCustomization(customization);
        CloseCustomizationMenu();
    }

    private void CloseCustomizationMenu()
    {
        customizationMenu.SetActive(false);

        explorationState = ExplorationState.Explore;
        Time.timeScale = 1f;
    }

    #endregion
}