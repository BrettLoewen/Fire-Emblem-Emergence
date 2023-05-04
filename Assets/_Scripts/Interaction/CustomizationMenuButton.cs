using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomizationMenuButton: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI optionText;

    private ExplorationGameManager gameManager;
    private Customization customization;

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

    public void Setup(Customization _customization, ExplorationGameManager _gameManager)
    {
        optionText.text = _customization.name;

        customization = _customization;

        gameManager = _gameManager;
    }

    public void OnClick()
    {
        gameManager.OnClickCustomizationOption(customization);
    }

    #endregion
}