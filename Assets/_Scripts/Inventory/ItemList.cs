using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

/// <summary>
/// Used to easily get different behaviour from the item list
/// </summary>
public enum ItemListMode { Buy, Sell, Inventory, UnitDetails }

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

            // 
            Item[] _items = new Item[_itemDatas.Count];

            //// Add every item data object to the menu
            //foreach (ItemData itemData in _itemDatas)
            //{
            //    Item _item = new Item(itemData);
            //    ItemDisplay _display = Instantiate(itemDisplayPrefab, itemDisplayContent);
            //    _display.Setup(_item, this);
            //}

            for (int i = 0; i < _itemDatas.Count; i++)
            {
                _items[i] = new Item(_itemDatas[i]);
            }

            SetupItemDisplays(_items);
        }
        else if(Mode == ItemListMode.Sell || Mode == ItemListMode.Inventory)
        {
            // Get the player's inventory
            Item[] _inventory = DataManager.GetPlayerInventory();

            // Add every item data object to the menu
            SetupItemDisplays(_inventory);
        }
    }


    public void SpawnItemList(ItemListMode _mode, Item[] _items)
    {
        // Ensure the mode is stored for later
        Mode = _mode;

        // Ensure the list is empty
        ClearList();

        // Enable the list
        background.enabled = true;

        if (Mode == ItemListMode.UnitDetails)
        {
            SetupItemDisplays(_items);
        }
    }


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
            _displays[i].SetNavigationLinks(_displays[up], _displays[down]);
        }
    }


    public void ClearList()
    {
        // Ensure there are no pre-existing buttons on the screen
        foreach (Transform child in itemDisplayContent)
        {
            Destroy(child.gameObject);
        }

        // Disable the background
        background.enabled = false;
    }

    public GameObject GetTopItemDisplay()
    {
        if(itemDisplayContent.childCount > 0)
        {
            return itemDisplayContent.GetChild(0).gameObject;
        }

        return null;
    }


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
    }


    public async void OnClickItemDisplay(Item _item, ItemDisplay _display)
    {
        if (Mode == ItemListMode.Buy)
        {
            bool hasEnough = DataManager.ModifyPlayerMoney(-_item.ItemData.BuyPrice);

            if(hasEnough)
            {
                DataManager.AddItemToPlayerInventory(_item);
            }

            MoneyDisplay.UpdateMoneyDisplay();
        }
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
    }

    #endregion
}