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
    public CustomizationName itemName;
    public int itemIndex;
    public CustomizationCategory itemCategory;
}

/// <summary>
/// Used to easily differentiate between different areas of the modular character
/// </summary>
[System.Serializable]
public enum CustomizationCategory { Male, Female, All }

/// <summary>
/// Used to easily select customization areas
/// </summary>
[System.Serializable]
public enum CustomizationName
{
    MaleHeadAllElements,
    MaleHeadNoElements,
    MaleEyebrows,
    MaleFacialHair,
    MaleTorso,
    MaleArmUpperRight,
    MaleArmUpperLeft,
    MaleArmLowerRight,
    MaleArmLowerLeft,
    MaleHandRight,
    MaleHandLeft,
    MaleHips,
    MaleLegRight,
    MaleLegLeft,
    FemaleHeadAllElements,
    FemaleHeadNoElements,
    FemaleEyebrows,
    FemaleFacialHair,
    FemaleTorso,
    FemaleArmUpperRight,
    FemaleArmUpperLeft,
    FemaleArmLowerRight,
    FemaleArmLowerLeft,
    FemaleHandRight,
    FemaleHandLeft,
    FemaleHips,
    FemaleLegRight,
    FemaleLegLeft,
    Hair,
    Helmet,
    HeadCoveringsBaseHair,
    HeadCoveringsNoFacialHair,
    HeadCoveringsNoHair,
    ChestAttachment,
    BackAttachment,
    ShoulderAttachmentRight,
    ShoulderAttachmentLeft,
    ElbowAttachmentRight,
    ElbowAttachmentLeft,
    HipsAttachment,
    KneeAttachmentRight,
    KneeAttachmentLeft
}