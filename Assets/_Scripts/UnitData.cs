using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Unit
{
    public UnitData UnitData { get; private set; }


    public Unit(UnitData _data)
    {
        UnitData = _data;
    }
}


[CreateAssetMenu(fileName = "New Unit", menuName = "Resources/Unit")]
public class UnitData : ScriptableObject
{
    public string Name;
    public Customization Customization;
}