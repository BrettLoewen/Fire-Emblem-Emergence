using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store a represent an item within an inventory
/// </summary>
[System.Serializable]
public class Item
{
    public string Id { get; private set; }
    public ItemData ItemData { get; private set; }  // The data object for this item
    public string OwnerId { get; private set; }

    /// <summary>
    /// Create a new item using the passed item data object
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_ownerId">The Id of the Unit that is holding this item. Defaults to null</param>
    public Item(ItemData _data, string _ownerId = null)
    {
        Id = System.Guid.NewGuid().ToString();

        ItemData = _data;

        OwnerId = _ownerId;
    }//end constructor


    public void SetOwnerId(string _newOwnerId)
    {
        OwnerId = _newOwnerId;
    }


    public override string ToString()
    {
        return $"{ItemData.Name}";
    }//end ToString

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        Item other = (Item)obj;
        return Id == other.Id;
    }

    // Here to prevent a code suggestion from triggering for Equals
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>
/// Used to quickly identify the type of item this is
/// </summary>
[System.Serializable]
public enum ItemType { Generic, Weapon }

/// <summary>
/// Used to define items and provide a base class for more advanced item types
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string Name;                             // The name of the item to display
    [TextArea(3, 10)] public string Description;    // A short description of what the item is/does
    public Sprite Sprite;                           // The visual sprite that will be displayed in menus (simple sprite for type)

    public ItemType ItemType;   // There are different categories of items (generic, weapon)

    public int BuyPrice;    // The amount of gold the player will need to give up to buy this item from a vendor
    public int SellPrice;   // The amount of gold the player will get if they sell this item to a vendor
}