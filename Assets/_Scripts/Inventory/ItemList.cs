using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList: MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform itemDisplayContent;
    [SerializeField] private ItemDisplay itemDisplayPrefab;

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

    #region List Control


    public void SpawnItemList()
    {
        // Get all of the item data objects in the game
        List<ItemData> items = DataManager.GetItems();

        // Add every item data object to the menu
        foreach (ItemData item in items)
        {
            ItemDisplay display = Instantiate(itemDisplayPrefab, itemDisplayContent);
            display.Setup(item, this);
        }
    }


    public void ClearList()
    {
        // Ensure there are no pre-existing buttons on the screen
        foreach (Transform child in itemDisplayContent)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject GetTopItemDisplay()
    {
        if(itemDisplayContent.childCount > 0)
        {
            return itemDisplayContent.GetChild(0).gameObject;
        }

        return null;
    }

    #endregion
}