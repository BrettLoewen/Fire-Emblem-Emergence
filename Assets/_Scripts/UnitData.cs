using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Unit
{
    public UnitData UnitData { get; private set; }
    private List<Item> items;

    public Unit(UnitData _data)
    {
        UnitData = _data;

        items = new List<Item>();
        for (int i = 0; i < UnitData.DefaultItems.Length; i++)
        {
            Item _item = new Item(UnitData.DefaultItems[i]);
            items.Add(_item);
        }
    }

    public Item[] GetItems()
    {
        return items.ToArray();
    }
}


[CreateAssetMenu(fileName = "New Unit", menuName = "Resources/Unit")]
public class UnitData : ScriptableObject
{
    public string Name;
    public Customization Customization;
    public ItemData[] DefaultItems;
}