using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ExplorationState { Setup, Explore, Menu }

public class ExplorationGameManager: Singleton<ExplorationGameManager>
{
    #region Variables

    [SerializeField] private PlayerManager player;
    private PlayerInputHandler playerInput;

    public ExplorationState explorationState { get; private set; }

    [SerializeField] private GameObject pauseMenu;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();

        explorationState = ExplorationState.Setup;

        playerInput = player.inputHandler;
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
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
                Time.timeScale = 0f;
            }
            // If the game was in menu mode, switch to exploration mode
            else if(explorationState == ExplorationState.Menu)
            {
                explorationState = ExplorationState.Explore;
                Time.timeScale = 1f;
            }
        }

        // If the game is in menu mode, enable the menu
        pauseMenu.SetActive(explorationState == ExplorationState.Menu);
    }//end Update

    #endregion //end Unity Control Methods

    #region Pause Menu

    public async void Save()
    {
        await SaveSystem.SaveData();
    }

    public async void Load()
    {
        await SaveSystem.LoadData();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    #endregion
}