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
    private const string WEAPON_PATH = "Items/Weapons"; // The name of the directory in the resources folder that holds weapon data objects

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
}