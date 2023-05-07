using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to define customization settings
/// </summary>
[CreateAssetMenu(fileName = "New Customization", menuName = "Resources/Unit Customization")]
public class Customization: ScriptableObject
{
    public Material Material; // Controls which material is used

    public CustomizationOption[] Options; // Controls which objects are enabled
}

/// <summary>
/// Represents a modular part within the unit
/// </summary>
[System.Serializable]
public struct CustomizationOption
{
    public string itemName;
    public int itemIndex;
    public CustomizationCategory itemCategory;
}

/// <summary>
/// Used to easily differentiate between different areas of the modular character
/// </summary>
[System.Serializable]
public enum CustomizationCategory { Male, Female, All }