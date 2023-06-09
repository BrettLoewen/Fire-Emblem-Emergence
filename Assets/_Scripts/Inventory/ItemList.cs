using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

/// <summary>
/// Used to easily get different behaviour from the item list
/// </summary>
public enum ItemListMode { Buy, Sell, Inventory, UnitDetails, UnitInventoryUnit, UnitInventoryPlayer }

public class ItemList: MonoBehaviour
{
    #region Variables

    public ItemListMode Mode { get; private set; }

    [SerializeField] private Transform itemDisplayContent;
    [SerializeField] private ItemDisplay itemDisplayPrefab;
    private ScrollRect scrollView;
    private Rect scrollRect;
    private Image background;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        // Get the necessary reference
        scrollView = GetComponent<ScrollRect>();
        scrollRect = (scrollView.transform as RectTransform).rect;
        background = GetComponent<Image>();
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

    #region List Control

    /// <summary>
    /// Generate the item displays based on the passed mode
    /// </summary>
    /// <param name="_mode"></param>
    public void SpawnItemList(ItemListMode _mode)
    {
        // Ensure the mode is stored for later
        Mode = _mode;

        // Ensure the list is empty
        ClearList();

        // Enable the list
        background.enabled = true;

        if(Mode == ItemListMode.Buy)
        {
            // Get all of the item data objects in the game
            List<ItemData> _itemDatas = DataManager.GetItems();

            Item[] _items = new Item[_itemDatas.Count];

            // Create the items
            for (int i = 0; i < _itemDatas.Count; i++)
            {
                _items[i] = new Item(_itemDatas[i]);
            }

            // Add every item to the menu
            SetupItemDisplays(_items);
        }
        else if(Mode == ItemListMode.Sell || Mode == ItemListMode.Inventory)
        {
            // Get the player's inventory
            Item[] _inventory = DataManager.GetPlayerInventory();

            // Add every item to the menu
            SetupItemDisplays(_inventory);
        }
    }//end SpawnItemList

    /// <summary>
    /// Generate the item displays based on the passed mode and a given list of items
    /// </summary>
    /// <param name="_mode"></param>
    /// <param name="_items"></param>
    public void SpawnItemList(ItemListMode _mode, Item[] _items)
    {
        // Ensure the mode is stored for later
        Mode = _mode;

        // Ensure the list is empty
        ClearList();

        // Enable the list
        background.enabled = true;

        // Add every item to the menu
        SetupItemDisplays(_items);
    }//end SpawnItemList

    /// <summary>
    /// Create the item displays for a given array of items
    /// </summary>
    /// <param name="_items"></param>
    private void SetupItemDisplays(Item[] _items)
    {
        List<ItemDisplay> _displays = new List<ItemDisplay>();

        // Add every item data object to the menu
        foreach (Item item in _items)
        {
            ItemDisplay _display = Instantiate(itemDisplayPrefab, itemDisplayContent);
            _display.Setup(item, this);
            _displays.Add(_display);
        }

        // For every item display that was created
        for (int i = 0; i < _displays.Count; i++)
        {
            // Calculate the index of the item display that is below and above it
            // Math below is to make sure it wraps properly
            int end = _displays.Count - 1;
            int up = i > 0 ? i - 1 : end;
            int down = i < end ? i + 1 : 0;

            // Tell the item display to setup its UI navigation links according to the above calculations
            _displays[i].SetNavigationLinksVertical(_displays[up], _displays[down]);
        }
    }//end SetupItemDisplays

    /// <summary>
    /// Delete every item display currently in the item list
    /// </summary>
    public void ClearList()
    {
        // Ensure there are no pre-existing buttons on the screen
        foreach (Transform child in itemDisplayContent)
        {
            Destroy(child.gameObject);
        }

        // Disable the background
        background.enabled = false;
    }//end ClearList

    /// <summary>
    /// Returns the item display at the top of the item list
    /// </summary>
    /// <returns></returns>
    public GameObject GetTopItemDisplay()
    {
        if(itemDisplayContent.childCount > 0)
        {
            return itemDisplayContent.GetChild(0).gameObject;
        }

        return null;
    }//end GetTopItemDisplay

    /// <summary>
    /// Dynamically scroll the item list according to which item display is currently being selected
    /// </summary>
    /// <param name="_transform"></param>
    public void OnSelectItemDisplay(Transform _transform)
    {
        float _scroll = 1 - (_transform.GetSiblingIndex() / (float)itemDisplayContent.childCount);
        float _bottomBound = 0.2f;
        float _topBound = 0.9f;


        if(_scroll < _bottomBound)
        {
            _scroll = 0f;
        }

        if(_scroll > _topBound)
        {
            _scroll = 1f;
        }

        scrollView.verticalNormalizedPosition = _scroll;
    }//end OnSelectItemDisplay

    /// <summary>
    /// Perform different actions when an item display is selected passed on the current mode of the item list
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_display"></param>
    public async void OnClickItemDisplay(Item _item, ItemDisplay _display)
    {
        // If the mode is Buy, attempt to purchase the item and add it to the player's inventory
        if (Mode == ItemListMode.Buy)
        {
            bool hasEnough = DataManager.ModifyPlayerMoney(-_item.ItemData.BuyPrice);

            if(hasEnough)
            {
                DataManager.AddItemToPlayerInventory(_item);
            }

            MoneyDisplay.UpdateMoneyDisplay();
        }
        // If the mode is Sell, remove the player's item and update the item list to remove that item
        else if (Mode == ItemListMode.Sell)
        {
            int _childIndex = _display.transform.GetSiblingIndex();

            DataManager.RemoveItemFromPlayerInventory(_item);

            DataManager.ModifyPlayerMoney(_item.ItemData.SellPrice);

            SpawnItemList(Mode);

            await Task.Yield();

            if (itemDisplayContent.childCount > 0)
            {
                _childIndex--;
                _childIndex = Mathf.Clamp(_childIndex, 0, itemDisplayContent.childCount);

                EventSystem.current.SetSelectedGameObject(itemDisplayContent.GetChild(_childIndex).gameObject);
            }
            else
            {
                ExplorationGameManager.Instance.CloseMarketMenu();
            }

            MoneyDisplay.UpdateMoneyDisplay();
        }
        // If the mode was in the unit's inventory, update the item's owner ID
        else if(Mode == ItemListMode.UnitInventoryUnit || Mode == ItemListMode.UnitInventoryPlayer)
        {
            ExplorationGameManager.Instance.UpdateItemOwner(_item.Id, Mode);
        }
    }//end OnClickItemDisplay

    /// <summary>
    /// Return the item displays
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetChildren()
    {
        List<GameObject> _children = new List<GameObject>();

        foreach (Transform child in itemDisplayContent)
        {
            _children.Add(child.gameObject);
        }

        return _children.ToArray();
    }//end GetChildren

    /// <summary>
    /// Will connect the navigation of this ItemList's ItemDisplays to the ItemDisplays of the passed ItemList
    /// </summary>
    /// <param name="_listToConnect">The ItemList to connect to this list to</param>
    public void ConnectHorizontallyToItemList(ItemList _listToConnect)
    {
        GameObject[] _childrenToConnect = _listToConnect.GetChildren();

        foreach (Transform _child in itemDisplayContent)
        {
            ItemDisplay _itemDisplay = _child.GetComponent<ItemDisplay>();

            ItemDisplay _connectedItemDisplay = _childrenToConnect[0].GetComponent<ItemDisplay>();

            _itemDisplay.SetNavigationLinksHorizontal(_connectedItemDisplay, _connectedItemDisplay);
        }

        foreach (GameObject _connectedChild in _childrenToConnect)
        {
            ItemDisplay _connectedItemDisplay = _connectedChild.GetComponent<ItemDisplay>();

            ItemDisplay _itemDisplay = GetTopItemDisplay().GetComponent<ItemDisplay>();

            _connectedItemDisplay.SetNavigationLinksHorizontal(_itemDisplay, _itemDisplay);
        }
    }//end ConnectHorizontallyToItemList

    #endregion
}