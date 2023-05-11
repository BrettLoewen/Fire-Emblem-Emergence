using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay: MonoBehaviour
{
    #region Variables

    private ItemData itemData;
    private ItemList itemList;

    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCostText;

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


    public void Setup(ItemData _itemData, ItemList _itemList)
    {
        itemData = _itemData;
        itemList = _itemList;

        itemIconImage.sprite = itemData.Sprite;
        itemNameText.text = itemData.Name;
        itemCostText.text = "" + itemData.BuyPrice;
    }

    #endregion
}