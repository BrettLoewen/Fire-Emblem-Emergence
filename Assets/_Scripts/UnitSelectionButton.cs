using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSelectionButton: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI unitNameText;

    private Button button;

    private Unit unit;
    private ExplorationGameManager gameManager;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
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


    public void Setup(Unit _unit)
    {
        gameManager = ExplorationGameManager.Instance;
        unit = _unit;

        unitNameText.text = unit.UnitData.Name;
    }

    /// <summary>
    /// Called by the `Select` event trigger on the game object. Used to signal when this object is navigated
    /// </summary>
    public void OnSelect()
    {
        gameManager.OnSelectUnit(unit);
    }//end OnSelect

    /// <summary>
    /// Called by the button's `OnClick` event. Used to signal that this object was clicked
    /// </summary>
    public void OnClick()
    {
        gameManager.OnClickUnit();
    }//end OnClick

    /// <summary>
    /// Used to manage the navigation paths of the unit selection buttons within a list
    /// </summary>
    /// <param name="_up">The button "above" this one (the button to select when up is pressed)</param>
    /// <param name="_down">The button "below" this one (the button to select when down is pressed)</param>
    public void SetNavigationLinks(UnitSelectionButton _up, UnitSelectionButton _down)
    {
        // Get the button's navigation object
        Navigation navigation = button.navigation;

        // Set the navigation object according to the passed information
        navigation.selectOnUp = _up.button;
        navigation.selectOnDown = _down.button;

        // Update the button's navigation to use the new values
        button.navigation = navigation;
    }//end SetNavigationLinks

    #endregion
}