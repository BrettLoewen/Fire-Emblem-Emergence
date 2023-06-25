using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Unit
{
    public string Id { get; private set; }
    public UnitData UnitData { get; private set; }

    /// <summary>
    /// Create a new item using the passed unit data object
    /// </summary>
    /// <param name="_data">The data object that defines the properties of this unit</param>
    public Unit(UnitData _data)
    {
        // Create a uuid to uniquely identify this unit
        Id = System.Guid.NewGuid().ToString();

        UnitData = _data;

        // Create the items that this unit should have and add them to the player inventory
        for (int i = 0; i < UnitData.DefaultItems.Length; i++)
        {
            Item _item = new Item(UnitData.DefaultItems[i], Id);
            DataManager.AddItemToPlayerInventory(_item);
        }
    }//end constructor

    /// <summary>
    /// Returns an array of items containing every item from the player's inventory that this unit owns
    /// </summary>
    /// <returns></returns>
    public Item[] GetItems()
    {
        return DataManager.GetItemsForUnit(Id);
    }//end GetItems

    /// <summary>
    /// Units are equal to each other if their Ids are the same
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }

        Unit other = (Unit)obj;
        return Id == other.Id;
    }//end Equals

    // Here to prevent a code suggestion from triggering for Equals
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }//end GetHashCode
}

/// <summary>
/// Used to define units
/// </summary>
[CreateAssetMenu(fileName = "New Unit", menuName = "Resources/Unit")]
public class UnitData : ScriptableObject
{
    public string Name;                 // The name of the unit
    public Customization Customization; // An object that defines the apperance of the unit
    public ItemData[] DefaultItems;     // The items the unit will spawn with
}