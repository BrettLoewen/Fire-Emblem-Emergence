using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomizationMenuInteractable: Interactable
{
    #region Variables

    private ExplorationGameManager gameManager;

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
        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    public override void Interact()
    {
        gameManager.OpenCustomizationMenu();
    }

    #endregion
}