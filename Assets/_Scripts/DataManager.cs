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
    private const string UNIT_PATH = "Units"; // The name of the directory in the resources folder that holds unit data objects

    private static List<Item> playerInventory = new List<Item>(); // The player's inventory of items
    private static int playerMoney = 5000;
    private static List<Unit> playerUnits = new List<Unit>();

    #region Customization

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

    #endregion

    #region Inventory

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

    /// <summary>
    /// Adds the passed item to the player's list of items
    /// </summary>
    /// <param name="_itemToAdd"></param>
    public static void AddItemToPlayerInventory(Item _itemToAdd)
    {
        // Ensure the DataManager has an up to date version of the player inventory
        GetPlayerInventory();

        // Add the new item to the inventory
        playerInventory.Add(_itemToAdd);

        // Update the inventory save data
        SaveSystem.SetPlayerInventoryData(playerInventory, playerMoney);
    }//end AddItemToPlayerInventory

    /// <summary>
    /// Removes the passed item from the player's list of items
    /// </summary>
    /// <param name="_itemToRemove"></param>
    public static void RemoveItemFromPlayerInventory(Item _itemToRemove)
    {
        // Ensure the DataManager has an up to date version of the player inventory
        GetPlayerInventory();

        // Remove the item from the inventory
        playerInventory.Remove(_itemToRemove);

        // Update the inventory save data
        SaveSystem.SetPlayerInventoryData(playerInventory, playerMoney);
    }//end RemoveItemFromPlayerInventory

    /// <summary>
    /// Modifies the item whose ID matches the passed item ID to update it's owner ID
    /// </summary>
    /// <param name="_itemId"></param>
    /// <param name="_newOwnerId"></param>
    public static void UpdateItemOwner(string _itemId, string _newOwnerId)
    {
        // Loop through each item until the matching one is found
        foreach (Item item in playerInventory)
        {
            if (item.Id == _itemId)
            {
                // Update the item's owner ID
                item.SetOwnerId(_newOwnerId);
                return;
            }
        }
    }//end UpdateItemOwner

    /// <summary>
    /// Returns the amount of money the player currently has
    /// </summary>
    /// <returns></returns>
    public static int GetPlayerMoney()
    {
        // If the player has < 0 money, then the save data has not been read from yet, so read from it
        if(playerMoney < 0)
        {
            playerMoney = SaveSystem.currentSaveData.playerMoney;
        }

        return playerMoney;
    }//end GetPlayerMoney

    /// <summary>
    /// Updates the player's money with the passed amount. To decrease, pass a negative value. Returns false if the modification is impossible.
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns>True if the modification succeeded and false if it did not (modification would cause negative money)</returns>
    public static bool ModifyPlayerMoney(int _amount)
    {
        // If the modification would cause the player to have negative money, return false
        if(playerMoney + _amount < 0)
        {
            return false;
        }

        playerMoney += _amount;
        return true;
    }//end ModifyPlayerMoney

    #endregion

    #region Units

    /// <summary>
    /// Gets all unit data objects in the game data, creates unit objects for them, and returns them
    /// </summary>
    /// <returns></returns>
    public static List<Unit> GetUnits()
    {
        // If the unit data has not been loaded yet, load it
        if (playerUnits == null || playerUnits.Count == 0)
        {
            List<Unit> units = new List<Unit>();

            // Load and return every unit data object in the game data
            UnitData[] unitDatas = Resources.LoadAll<UnitData>(UNIT_PATH);

            // Create a unit object for every unit data object
            for (int i = 0; i < unitDatas.Length; i++)
            {
                units.Add(new Unit(unitDatas[i]));
            }

            playerUnits = units;
        }

        return playerUnits;
    }//end GetUnits

    /// <summary>
    /// Get the items that the passed unit (by ID) owns by searching the player's inventory
    /// </summary>
    /// <param name="_unitId"></param>
    /// <returns></returns>
    public static Item[] GetItemsForUnit(string _unitId)
    {
        List<Item> _itemsForUnit = new List<Item>();

        // Loop through every item in the player's inventory to find the ones the passed unit owns
        foreach (Item item in playerInventory)
        {
            if(item.OwnerId == _unitId)
            {
                _itemsForUnit.Add(item);
            }    
        }

        return _itemsForUnit.ToArray();
    }//end GetItemsForUnit

    /// <summary>
    /// Return the name of the unit whose id matches the passed id
    /// </summary>
    /// <param name="_unitId"></param>
    /// <returns></returns>
    public static string GetNameOfUnit(string _unitId)
    {
        // Ensure the unit data has been loaded
        if (playerUnits == null || playerUnits.Count == 0)
        {
            GetUnits();
        }

        // Loop through every unit to find the matching one
        foreach (Unit unit in playerUnits)
        {
            if(unit.Id == _unitId)
            {
                return unit.UnitData.name;
            }
        }

        return null;
    }//end GetNameOfUnit

    #endregion
}