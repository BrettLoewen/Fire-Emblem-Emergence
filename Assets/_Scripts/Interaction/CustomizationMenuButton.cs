using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Used to represent a customization object that the player cna pick from
/// </summary>
public class CustomizationMenuButton: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI optionText; // Displays the name of the customization object

    private ExplorationGameManager gameManager; // The manager of the exploration scene
    private Customization customization;        // The customization object that is being displayed

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
    /// Used to passed the necessary information to this object
    /// </summary>
    /// <param name="_customization">The customization object that will be displayed</param>
    /// <param name="_gameManager">The manager of this component</param>
    public void Setup(Customization _customization, ExplorationGameManager _gameManager)
    {
        // Set the display text to the customization object's name
        optionText.text = _customization.name;

        // Store the passed information for later
        customization = _customization;
        gameManager = _gameManager;
    }//end Setup

    /// <summary>
    /// Called by the button when it is clicked. Tells the manager it was clicked
    /// </summary>
    public void OnClick()
    {
        gameManager.OnClickCustomizationOption(customization);
    }//end OnClick

    #endregion
}