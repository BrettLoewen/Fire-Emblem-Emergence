using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay: MonoBehaviour
{
    #region Variables

    private Item item;
    private ItemList itemList;

    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCostText;
    private Button button;

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

    /// <summary>
    /// Called by ItemList to setup the item's display
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_itemList"></param>
    public void Setup(Item _item, ItemList _itemList)
    {
        // Store the passed data for later
        item = _item;
        itemList = _itemList;

        // Setup the display according to item values
        itemIconImage.sprite = item.ItemData.Sprite;
        itemNameText.text = item.ItemData.Name;

        // Setup the display according to item values depending on the mode of the item list
        if(itemList.Mode == ItemListMode.Buy)
        {
            itemCostText.text = "" + item.ItemData.BuyPrice;
        }
        else if(itemList.Mode == ItemListMode.Sell)
        {
            itemCostText.text = "" + item.ItemData.SellPrice;
        }
        else if(itemList.Mode == ItemListMode.Inventory)
        {
            itemCostText.gameObject.SetActive(false);
        }
        else if(itemList.Mode == ItemListMode.UnitDetails)
        {
            itemCostText.gameObject.SetActive(false);
        }
    }//end Setup

    /// <summary>
    /// Called by the `Select` event trigger on the game object. Used to signal when this object is navigated to so the scroll view can be updated
    /// </summary>
    public void OnSelect()
    {
        // Tell the ItemList that this ItemDisplay was navigated to
        itemList.OnSelectItemDisplay(transform);
    }//end OnSelect

    /// <summary>
    /// Called by the button's `OnClick` event. Used to signal to the item list that this item display was clicked
    /// </summary>
    public void OnClick()
    {
        itemList.OnClickItemDisplay(item, this);
    }//end OnClick

    /// <summary>
    /// Used to manage the navigation paths of the item displays within the item list
    /// </summary>
    /// <param name="_up">The button "above" this one (the button to select when up is pressed)</param>
    /// <param name="_down">The button "below" this one (the button to select when down is pressed)</param>
    public void SetNavigationLinks(ItemDisplay _up, ItemDisplay _down)
    {
        // Get the button's navigation object
        Navigation navigation = button.navigation;

        // Set the navigation object according to the passed information
        navigation.selectOnUp = _up.button;
        navigation.selectOnDown = _down.button;

        // Update the button's navigation to use the new values
        button.navigation = navigation;
    }//end SetNavigationLinks


    public Item GetItem()
    {
        return item;
    }

    #endregion
}