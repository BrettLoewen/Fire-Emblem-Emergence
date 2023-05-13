using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from Interactable to allow the player to open a market menu
/// </summary>
public class MarketInteractable : Interactable
{
    #region Variables

    private ExplorationGameManager gameManager; // The manager of the exploration scene

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        // Get the necessary reference
        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {

    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Override the base interact method to open the market menu
    /// </summary>
    public override void Interact()
    {
        gameManager.OpenMarketMenu(MarketMenuState.Buy);
    }//end Interact

    #endregion
}