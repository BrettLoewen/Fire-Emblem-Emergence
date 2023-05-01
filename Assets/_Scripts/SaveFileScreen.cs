using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public enum SaveFileScreenMode { Save, Load }

public class SaveFileScreen: MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform saveFileParent;
    [SerializeField] private SaveFileBar saveFileBarPrefab;
    [SerializeField] private TextMeshProUGUI screenModeText;

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

    #region

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
        screenModeText.text = mode.ToString();

        // Default to the save file that is currently being used (last file saved to)
        int currentSaveFile = SaveSystem.metaData.currentSaveFile;
        EventSystem.current.SetSelectedGameObject(bars[currentSaveFile].gameObject);
    }

    public void CloseSaveFileScreen()
    {
        // Deactivate the header text
        screenModeText.gameObject.SetActive(false);

        // Destroy the save file bars
        foreach (Transform child in saveFileParent)
        {
            Destroy(child.gameObject);
        }
    }

    public async void OnSaveFileBarClick(int index)
    {
        if(mode == SaveFileScreenMode.Load)
        {
            Debug.Log($"Trying to load from save file {index + 1}");
        }
        else
        {
            Debug.Log($"Trying to save to save file {index + 1}");
            await SaveSystem.SaveData(index);
            await OpenSaveFileScreen(mode);
        }
    }

    #endregion
}