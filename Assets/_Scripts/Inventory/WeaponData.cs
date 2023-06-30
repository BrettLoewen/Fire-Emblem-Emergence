using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to define the category this weapon is in
/// </summary>
[System.Serializable]
public enum WeaponType { Sword, Lance, Axe, Bow, Brawl, Rune }

/// <summary>
/// Used to define which stats should be used for damage calculation
/// </summary>
[System.Serializable]
public enum DamageType { Physical, Energy, Highest }

/// <summary>
/// Used to define information about a weapon
/// </summary>
[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    public WeaponType WeaponType; // The category this weapon is a part of
    public DamageType DamageType; // The category of damage this weapon will inflict

    public int Might;   // Used for damage calculation
    public int Weight;  // Used for speed calculation
    public int Range = 1;
}