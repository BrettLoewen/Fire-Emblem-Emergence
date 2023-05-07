using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

/// <summary>
/// Used to track what should happen when the save file bar's are clicked
/// </summary>
public enum SaveFileScreenMode { Save, Load, NewGame }

/// <summary>
/// Used to display and control the player's save files
/// </summary>
public class SaveFileScreen: MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform saveFileParent;          // The parent transform for the save file bars
    [SerializeField] private SaveFileBar saveFileBarPrefab;     // The prefab for the save file bar
    [SerializeField] private TextMeshProUGUI screenModeText;    // The header text object used to display which mode the screen is in

    // Stores the mode the screen is currently in
    private SaveFileScreenMode mode;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
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

    #region Screen Management

    /// <summary>
    /// Used to open the save file screen with the correct save data
    /// </summary>
    /// <param name="_mode">The mode the screen should act in</param>
    /// <returns></returns>
    public async Task OpenSaveFileScreen(SaveFileScreenMode _mode)
    {
        // Store whether the menu is currently being used for saving or loading
        mode = _mode;

        // Destroy any old save file bars 
        foreach (Transform child in saveFileParent)
        {
            Destroy(child.gameObject);
        }

        // Load the save files
        SaveData[] data = await SaveSystem.LoadSaveFiles();

        // Create an empty list to store references to the save file bars as they are instantiated
        List<SaveFileBar> bars = new List<SaveFileBar>();

        // For every save file in the data
        for (int i = 0; i < data.Length; i++)
        {
            SaveFileBar bar = Instantiate(saveFileBarPrefab, saveFileParent);   // Create a save file bar for the save file
            bar.Setup(data[i], i, this);                                        // Give the save tile bar its save data
            bars.Add(bar);                                                      // Add the bar to the list for later
        }

        // For every save file bar that was created
        for (int i = 0; i < bars.Count; i++)
        {
            // Calculate the index of the save file bar that is below and above it
            // Math below is to make sure it wraps properly
            int end = bars.Count - 1;
            int up = i > 0 ? i - 1 : end;
            int down = i < end ? i + 1 : 0;

            // Tell the save file bar to setup its UI navigation links according to the above calculations
            bars[i].SetNavigationLinks(bars[up], bars[down]);
        }

        // Setup the header text so the user knows which save/load mode they are in
        screenModeText.gameObject.SetActive(true);
        string modeText = "";
        switch(mode)
        {
            case SaveFileScreenMode.Load:
                modeText = "Load";
                break;
            case SaveFileScreenMode.Save:
                modeText = "Save";
                break;
            case SaveFileScreenMode.NewGame:
                modeText = "New Game";
                break;
        }
        screenModeText.text = modeText;

        // Default to the save file that is currently being used (last file saved to)
        int currentSaveFile = SaveSystem.metaData.currentSaveFile;
        EventSystem.current.SetSelectedGameObject(bars[currentSaveFile].gameObject);
    }//end OpenSaveFileScreen

    /// <summary>
    /// Used to close the save file screen
    /// </summary>
    public void CloseSaveFileScreen()
    {
        // Deactivate the header text
        screenModeText.gameObject.SetActive(false);

        // Destroy the save file bars
        foreach (Transform child in saveFileParent)
        {
            Destroy(child.gameObject);
        }
    }//end CloseSaveFileScreen

    /// <summary>
    /// Called by the save file bar button when it is clicked
    /// </summary>
    /// <param name="index">The index of the save file the button represents</param>
    /// <param name="dataIsNull">Whether or not the save file passed to the button exists</param>
    /// <returns></returns>
    public async Task OnSaveFileBarClick(int index, bool dataIsNull)
    {
        // If the screen is in load mode, load into the correct scene (assuming the save file exists)
        if(mode == SaveFileScreenMode.Load)
        {
            Debug.Log($"Trying to load from save file {index + 1}");
            // If the save file exists
            if(dataIsNull == false)
            {
                // Set this save file to be the current save file
                await SaveSystem.SetCurrentSaveFile(index);
                await Task.Delay(100);

                //Make sure the game is no longer paused
                Time.timeScale = 1f;

                // Load into the correct scene
                LevelManager.Instance.LoadScene(Scenes.HubWorld);
            }
        }
        // If the screen is in save mode, save the current save data
        else if(mode == SaveFileScreenMode.Save)
        {
            Debug.Log($"Trying to save to save file {index + 1}");
            // Save the data
            await SaveSystem.SaveData(index);

            // Refresh the save file screen
            await OpenSaveFileScreen(mode);
        }
        // If the screen is in new game mode, create a new game save file and load into the correct scene
        else if(mode == SaveFileScreenMode.NewGame)
        {
            Debug.Log($"Trying to make a new save file using save file {index + 1}");
            // Create the new game save file
            await SaveSystem.MakeNewGameSaveFile(index);
            await Task.Delay(100);

            // Make sure the game is no longer paused
            Time.timeScale = 1f;

            // Load into the correct scene
            LevelManager.Instance.LoadScene(Scenes.HubWorld);
        }
    }//end OnSaveFileBarClick

    #endregion
}