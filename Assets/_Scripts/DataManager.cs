using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to get and store game data
/// </summary>
public static class DataManager
{
    private const string UNIT_CUTOMIZATION_PATH = "Unit Customization"; // The name of the directory in the resources folder that holds customization objects
    private const string ITEM_PATH = "Items"; // The name of the directory in the resources folder that holds item data objects

    private static List<Item> playerInventory = new List<Item>(); // The player's inventory of items
    private static int playerMoney = -1;

    /// <summary>
    /// Gets and returns all customization objects in the game data
    /// </summary>
    /// <returns>Returns every customization object in the game data</returns>
    public static Customization[] GetCustomizationOptions()
    {
        // Load and return every customization object in the game data
        return Resources.LoadAll<Customization>(UNIT_CUTOMIZATION_PATH);
    }//end GetCustomizationOptions

    /// <summary>
    /// Get the customization object that has the passed file name
    /// </summary>
    /// <param name="_customizationObjectName">The file name of the customization object to be returned</param>
    /// <returns>The customization object that has the passed file name</returns>
    public static Customization GetCustomizationFromString(string _customizationObjectName)
    {
        // Load and return the customization object that has the passed file name
        return Resources.Load<Customization>($"{UNIT_CUTOMIZATION_PATH}/{_customizationObjectName}");
    }//end GetCustomizationFromString

    /// <summary>
    /// Gets and returns all item objects in the game data
    /// </summary>
    /// <returns>Returns every customization object in the game data</returns>
    public static List<ItemData> GetItems()
    {
        // Create a list to hold all of the items so they can be returned
        List<ItemData> items = new List<ItemData>();
        
        // Load every item data object in the game data
        items.AddRange(Resources.LoadAll<ItemData>(ITEM_PATH));

        // Load every weapon data object in the game data
        //items.AddRange(Resources.LoadAll<WeaponData>(WEAPON_PATH));

        // Return the generated list of items
        return items;
    }//end GetItems

    /// <summary>
    /// Get the player's inventory and return a shallow copy of it
    /// </summary>
    /// <returns></returns>
    public static Item[] GetPlayerInventory()
    {
        // If the player inventory is null, get the necessary information from the save system
        if(playerInventory == null || playerInventory.Count == 0)
        {
            List<Item> _savedInventory = SaveSystem.GetPlayerInventory();

            // If the save system inventory is null, make a new blank inventory
            if (_savedInventory == null)
            {
                Debug.LogWarning("Null inventory in save data!");

                _savedInventory = new List<Item>();
            }

            playerInventory = _savedInventory;
        }

        // Make a shallow copy of the player inventory and return it
        Item[] _copyOfInventory = new Item[playerInventory.Count];
        playerInventory.CopyTo(_copyOfInventory);
        return _copyOfInventory;
    }//end GetPlayerInventory


    public static void AddItemToPlayerInventory(Item _itemToAdd)
    {
        // Ensure the DataManager has an up to date version of the player inventory
        GetPlayerInventory();

        // Add the new item to the inventory
        playerInventory.Add(_itemToAdd);

        // Update the inventory save data
        SaveSystem.SetPlayerInventoryData(playerInventory, playerMoney);
    }//end AddItemToPlayerInventory


    public static void RemoveItemFromPlayerInventory(Item _itemToRemove)
    {
        // Ensure the DataManager has an up to date version of the player inventory
        GetPlayerInventory();

        // Remove the item from the inventory
        playerInventory.Remove(_itemToRemove);

        // Update the inventory save data
        SaveSystem.SetPlayerInventoryData(playerInventory, playerMoney);
    }//end RemoveItemFromPlayerInventory


    public static int GetPlayerMoney()
    {
        if(playerMoney < 0)
        {
            playerMoney = SaveSystem.currentSaveData.playerMoney;
        }

        return playerMoney;
    }


    public static bool ModifyPlayerMoney(int _amount)
    {
        if(playerMoney + _amount < 0)
        {
            return false;
        }

        playerMoney += _amount;
        return true;
    }
}