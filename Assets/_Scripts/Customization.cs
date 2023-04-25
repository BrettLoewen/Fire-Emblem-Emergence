using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customization", menuName = "Resources/Unit Customization")]
public class Customization: ScriptableObject
{
    public Material material;

    public CustomizationOption[] options;
}

[System.Serializable]
public struct CustomizationOption
{
    public string itemName;
    public int itemIndex;
    public CustomizationCategory itemCategory;
}

[System.Serializable]
public enum CustomizationCategory { Male, Female, All }