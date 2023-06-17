using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Unit
{
    public string Id { get; private set; }
    public UnitData UnitData { get; private set; }

    public Unit(UnitData _data)
    {
        Id = System.Guid.NewGuid().ToString();

        UnitData = _data;

        for (int i = 0; i < UnitData.DefaultItems.Length; i++)
        {
            Item _item = new Item(UnitData.DefaultItems[i], Id);
            DataManager.AddItemToPlayerInventory(_item);
        }
    }

    public Item[] GetItems()
    {
        return DataManager.GetItemsForUnit(Id);
    }

    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }

        Unit other = (Unit)obj;
        return Id == other.Id;
    }

    // Here to prevent a code suggestion from triggering for Equals
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


[CreateAssetMenu(fileName = "New Unit", menuName = "Resources/Unit")]
public class UnitData : ScriptableObject
{
    public string Name;
    public Customization Customization;
    public ItemData[] DefaultItems;
}