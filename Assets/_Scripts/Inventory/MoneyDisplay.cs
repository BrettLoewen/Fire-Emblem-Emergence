using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay: Singleton<MoneyDisplay>
{
    #region Variables

    private TextMeshProUGUI moneyText;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();

        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update


    private void OnEnable()
    {
        UpdateMoneyDisplay();
    }

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Used to update the money text staticly
    /// </summary>
    public static void UpdateMoneyDisplay()
    {
        Instance.moneyText.text = $"{DataManager.GetPlayerMoney()}";
    }//end UpdateMoneyDisplay

    #endregion
}