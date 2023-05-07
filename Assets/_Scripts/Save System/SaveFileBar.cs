using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Used to display a given save file data object to the player
/// </summary>
public class SaveFileBar: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI activityText;  // Displays the player's activity
    [SerializeField] private TextMeshProUGUI timestampText; // Displays the timestamp at which the save file was last updated
    [SerializeField] private GameObject emptySaveFileText;  // Used when representing a save file that does not exist

    public Button button { get; private set; }  // The button component

    private SaveData data;                  // The save data object being represented
    private int index;                      // The file index of the save data object
    private SaveFileScreen saveFileScreen;  // The save file screen this is a part of

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

    /// <summary>
    /// Used to setup the bar with the necessary information
    /// </summary>
    /// <param name="_data">The save data object to display</param>
    /// <param name="_index">The file index of the passed save data object</param>
    /// <param name="_saveFileScreen">The screen this component is a part of</param>
    public void Setup(SaveData _data, int _index, SaveFileScreen _saveFileScreen)
    {
        // Store the passed information
        data = _data;
        index = _index;
        saveFileScreen = _saveFileScreen;

        // If the save data object does not exist, display that
        if(data == null)
        {
            emptySaveFileText.SetActive(true);
            activityText.gameObject.SetActive(false);
            timestampText.gameObject.SetActive(false);
        }
        // If the save data object does exist, display it
        else
        {
            emptySaveFileText.SetActive(false);
            activityText.gameObject.SetActive(true);
            timestampText.gameObject.SetActive(true);

            activityText.text = data.activity;
            timestampText.text = data.savedAtTimestamp;
        }

        // Store a reference to the button component
        button = GetComponent<Button>();
    }//end Setup

    /// <summary>
    /// Used to manage the navigation paths of the save file bars within the save file screen
    /// </summary>
    /// <param name="_up">The button "above" this one (the button to select when up is pressed)</param>
    /// <param name="_down">The button "below" this one (the button to select when down is pressed)</param>
    public void SetNavigationLinks(SaveFileBar _up, SaveFileBar _down)
    {
        // Get the button's navigation object
        Navigation navigation = button.navigation;

        // Set the navigation object according to the passed information
        navigation.selectOnUp = _up.button;
        navigation.selectOnDown = _down.button;

        // Update the button's navigation to use the new values
        button.navigation = navigation;
    }//end SetNavigationLinks

    /// <summary>
    /// Called by the button when it is clicked
    /// </summary>
    public async void OnClick()
    {
        // Tell the save file screen that this save file bar was clicked and whether or not its save data exists
        await saveFileScreen.OnSaveFileBarClick(index, data == null);
    }//end OnClick

    #endregion
}