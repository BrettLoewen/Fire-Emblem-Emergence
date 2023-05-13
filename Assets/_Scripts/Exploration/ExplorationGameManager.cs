using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

/// <summary>
/// The state of the exploration scene
/// </summary>
[System.Serializable]
public enum ExplorationState { Setup, Explore, Menu, Customization, Market }

/// <summary>
/// The state of the exploration pause menu
/// </summary>
public enum ExplorationMenuState { Selection, Save, Load, Inventory }

/// <summary>
/// The state of the market menu
/// </summary>
[System.Serializable]
public enum MarketMenuState { Selection, Buy, Sell }

/// <summary>
/// Used to manage the exploration scene
/// </summary>
public class ExplorationGameManager: Singleton<ExplorationGameManager>
{
    #region Variables

    // References to player components
    [SerializeField] private PlayerManager player;
    private PlayerInputHandler playerInput;

    // Variables for managing scene state
    public ExplorationState ExplorationState { get; private set; }
    private ExplorationMenuState menuState;

    [Header("Pause Menu")]
    // Variables for the pause menu
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private SaveFileScreen saveFileScreen;
    [SerializeField] private ItemList inventoryMenuScreen;

    [Header("Customization Menu")]
    // Variables for the customization menu
    [SerializeField] private GameObject customizationMenu;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private CustomizationMenuButton buttonPrefab;

    [Header("Market Menu")]
    // 
    [SerializeField] private GameObject marketMenu;
    [SerializeField] private GameObject buySellButtonParent;
    [SerializeField] private ItemList marketItemList;
    private MarketMenuState marketMenuState;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject sellButton;


    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override async void Awake()
    {
        // Setup the base class singleton
        base.Awake();

        // Get a reference to the player input handler
        playerInput = player.InputHandler;

        // Start in the setup state
        ExplorationState = ExplorationState.Setup;

        // In case the persistent scene is not loaded, load it
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

        // Close the loading screen
        await LevelManager.Instance.SetLoadingFinished();

        await Task.Delay(100);

        // Once as setup has ended, exploration can begin
        ExplorationState = ExplorationState.Explore;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // If the player pressed the menu button
        if(playerInput.MenuInput)
        {
            // Cancel the button press
            playerInput.MenuInput = false;

            // If the game was in exploration mode, switch to menu mode
            if(ExplorationState == ExplorationState.Explore)
            {
                ExplorationState = ExplorationState.Menu;
                menuState = ExplorationMenuState.Selection;
                EventSystem.current.SetSelectedGameObject(inventoryButton);
                Time.timeScale = 0f;
            }
            // If the game was in menu mode, switch to exploration mode
            else if(ExplorationState == ExplorationState.Menu)
            {
                ExplorationState = ExplorationState.Explore;
                Time.timeScale = 1f;
            }
        }

        // If the player pressed the cancel button
        if (playerInput.CancelInput)
        {
            // Cancel the button press
            playerInput.CancelInput = false;

            switch(ExplorationState)
            {
                // If cancel was pressed while in the pause menu
                case ExplorationState.Menu:
                    switch (menuState)
                    {
                        // If cancel was pressed while on the load screen, go back to the selection screen
                        case ExplorationMenuState.Load:
                            menuState = ExplorationMenuState.Selection;
                            saveFileScreen.CloseSaveFileScreen();
                            EventSystem.current.SetSelectedGameObject(loadButton);
                            break;
                        // If cancel was pressed while on the save screen, go back to the selection screen
                        case ExplorationMenuState.Save:
                            menuState = ExplorationMenuState.Selection;
                            saveFileScreen.CloseSaveFileScreen();
                            EventSystem.current.SetSelectedGameObject(saveButton);
                            break;
                        // If cancel was pressed while on the inventory screen, go back to the selection screen
                        case ExplorationMenuState.Inventory:
                            menuState = ExplorationMenuState.Selection;
                            CloseInventoryMenu();
                            EventSystem.current.SetSelectedGameObject(inventoryButton);
                            break;
                        // If cancel was pressed while on the selection screen, close the pause menu
                        case ExplorationMenuState.Selection:
                            menuState = ExplorationMenuState.Selection;
                            ExplorationState = ExplorationState.Explore;
                            Time.timeScale = 1f;
                            break;
                    }
                    break;
                // If cancel was pressed while in the customization menu, close it
                case ExplorationState.Customization:
                    CloseCustomizationMenu();
                    break;
                // If cancel was pressed while in the customization menu, close it
                case ExplorationState.Market:
                    CloseMarketMenu();
                    break;
            }
        }

        // If the game is in menu mode, enable the menu
        pauseMenu.SetActive(ExplorationState == ExplorationState.Menu);
    }//end Update

    #endregion //end Unity Control Methods

    #region Pause Menu


    public void Inventory()
    {
        // Open the inventory screen
        menuState = ExplorationMenuState.Inventory;
        OpenInventoryMenu();
    }

    /// <summary>
    /// Called by the save button to open the save screen
    /// </summary>
    public async void Save()
    {
        menuState = ExplorationMenuState.Save;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Save);
    }//end Save

    /// <summary>
    /// Called by the load button to open the load screen
    /// </summary>
    public async void Load()
    {
        menuState = ExplorationMenuState.Load;
        await saveFileScreen.OpenSaveFileScreen(SaveFileScreenMode.Load);
    }//end Load

    /// <summary>
    /// Called by the quit button to quit out of the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }//end Quit


    private async void OpenInventoryMenu()
    {
        // Enable the inventory screen
        inventoryMenuScreen.gameObject.SetActive(true);

        // Setup the item list in 'inventory' mode
        inventoryMenuScreen.SpawnItemList(ItemListMode.Inventory);

        await Task.Yield();

        // Set the selected object for UI navigation to the first item display in the item list
        EventSystem.current.SetSelectedGameObject(inventoryMenuScreen.GetTopItemDisplay());
    }


    private void CloseInventoryMenu()
    {
        // Ensure the inventory screen cleans itself up
        inventoryMenuScreen.ClearList();

        // Disable the inventory screen
        inventoryMenuScreen.gameObject.SetActive(false);
    }

    #endregion

    #region Customization Menu

    /// <summary>
    /// Open the customization menu
    /// </summary>
    public async void OpenCustomizationMenu()
    {
        // Enable the menu
        customizationMenu.SetActive(true);

        // Set the game state to be in the customziation menu
        ExplorationState = ExplorationState.Customization;

        // Ensure there are no pre-existing buttons on the screen
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Gives time for the children to be destroyed
        await Task.Yield();

        // Get all of the customization objects in the game
        Customization[] customizationOptions = DataManager.GetCustomizationOptions();

        // Add every customization object to the menu
        foreach (Customization option in customizationOptions)
        {
            CustomizationMenuButton button = Instantiate(buttonPrefab, buttonParent);
            button.Setup(option, this);
        }

        // If at least one button was created, set it as the currently selected UI element
        if (buttonParent.childCount > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttonParent.GetChild(0).gameObject);
        }

        // Pause the game
        Time.timeScale = 0f;
    }//end OpenCustomizationMenu

    /// <summary>
    /// Update the player's customization option and close the customization menu
    /// </summary>
    /// <param name="customization"></param>
    public void OnClickCustomizationOption(Customization _customization)
    {
        player.SetCustomization(_customization);
        CloseCustomizationMenu();
    }//end OnClickCustomizationOption

    /// <summary>
    /// Close the customization menu and unpause the game
    /// </summary>
    private void CloseCustomizationMenu()
    {
        customizationMenu.SetActive(false);

        ExplorationState = ExplorationState.Explore;
        Time.timeScale = 1f;
    }//end CloseCustomizationMenu

    #endregion

    #region Market Menu

    /// <summary>
    /// Open the market menu
    /// </summary>
    public async void OpenMarketMenu(MarketMenuState _marketMenuToOpen)
    {
        // Enable the menu
        marketMenu.SetActive(true);

        // Set the game state to be in the market menu
        ExplorationState = ExplorationState.Market;
        marketMenuState = MarketMenuState.Selection;

        // Ensure there are no pre-existing item displays on the screen
        marketItemList.ClearList();

        // Gives time for the children to be destroyed
        await Task.Yield();

        // Enable the buy and sell buttons so the player can decide which market menu they want to enter
        buySellButtonParent.SetActive(true);

        // If at least one button was created, set it as the currently selected UI element
        if (buttonParent.childCount > 0)
        {
            if(_marketMenuToOpen == MarketMenuState.Buy)
            {
                EventSystem.current.SetSelectedGameObject(buyButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(sellButton);
            }
        }

        // Pause the game
        Time.timeScale = 0f;
    }//end OpenMarketMenu

    /// <summary>
    /// Called by the Buy and Sell buttons to open their menus
    /// </summary>
    /// <param name="_openBuyMenu">If true, open the menu in 'Buy' mode. If false, open the menu in 'Sell' mode</param>
    public void OnClickMarketOption(bool _openBuyMenu)
    {
        // Hide the buy and sell buttons
        buySellButtonParent.SetActive(false);

        // Use the passed menu mode to set the market menu state
        if (_openBuyMenu)
        {
            marketMenuState = MarketMenuState.Buy;

            // Setup the item list
            marketItemList.SpawnItemList(ItemListMode.Buy);
        }
        else
        {
            marketMenuState = MarketMenuState.Sell;

            // Setup the item list
            marketItemList.SpawnItemList(ItemListMode.Sell);
        }

        // Set the selected object for UI navigation to the first item display in the item list
        EventSystem.current.SetSelectedGameObject(marketItemList.GetTopItemDisplay());
    }//end OnClickMarketOption

    /// <summary>
    /// Close the market menu and unpause the game
    /// </summary>
    #pragma warning disable CS1998
    public async void CloseMarketMenu()
    {
        // If the player closed the menu while selecting between the buy and sell options, the market menu should close
        if(marketMenuState == MarketMenuState.Selection)
        {
            // Disable the market UI
            marketMenu.SetActive(false);

            // Unpause the game
            ExplorationState = ExplorationState.Explore;
            Time.timeScale = 1f;
        }
        // If the player closed the menu while in a buying or selling, the market menu should return to the selection menu
        else
        {
            OpenMarketMenu(marketMenuState);
        }
    }//end CloseMarketMenu

    #endregion
}