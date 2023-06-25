using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDetailsScreen : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private UnitCustomizer unitDisplay;
    [SerializeField] private ItemList unitInventoryDisplay;

    private Unit unit;

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
    /// Used to setup the screen so it references the correct Unit
    /// </summary>
    public void Setup(Unit _unit)
    {
        unit = _unit;

        unitNameText.text = unit.UnitData.Name;
        unitDisplay.SetCustomization(unit.UnitData.Customization);

        unitInventoryDisplay.SpawnItemList(ItemListMode.UnitDetails, unit.GetItems());
    }//end Setup

    #endregion
}
