using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control how a unit looks using a modular character
/// </summary>
public class UnitCustomizer : MonoBehaviour
{
    #region Variables

    [SerializeField] private Customization customization;   // The customization object which defines how the unit should be customized

    private List<GameObject> enabledObjects = new List<GameObject>();   // List of enabed objects on character

    // Used to get and enable the correct objects
    [SerializeField] private Dictionary<CustomizationName, List<GameObject>> maleObjects = new Dictionary<CustomizationName, List<GameObject>>();
    [SerializeField] private Dictionary<CustomizationName, List<GameObject>> femaleObjects = new Dictionary<CustomizationName, List<GameObject>>();
    [SerializeField] private Dictionary<CustomizationName, List<GameObject>> allObjects = new Dictionary<CustomizationName, List<GameObject>>();

    // Used to reference body parts of the modular character
    private string[] CUSTOMIZATION_NAMES = { 
        "Male_Head_All_Elements",
        "Male_Head_No_Elements",
        "Male_01_Eyebrows",
        "Male_02_FacialHair",
        "Male_03_Torso",
        "Male_04_Arm_Upper_Right",
        "Male_05_Arm_Upper_Left",
        "Male_06_Arm_Lower_Right",
        "Male_07_Arm_Lower_Left",
        "Male_08_Hand_Right",
        "Male_09_Hand_Left",
        "Male_10_Hips",
        "Male_11_Leg_Right",
        "Male_12_Leg_Left",
        "Female_Head_All_Elements",
        "Female_Head_No_Elements",
        "Female_01_Eyebrows",
        "Female_02_FacialHair",
        "Female_03_Torso",
        "Female_04_Arm_Upper_Right",
        "Female_05_Arm_Upper_Left",
        "Female_06_Arm_Lower_Right",
        "Female_07_Arm_Lower_Left",
        "Female_08_Hand_Right",
        "Female_09_Hand_Left",
        "Female_10_Hips",
        "Female_11_Leg_Right",
        "Female_12_Leg_Left",
        "All_01_Hair",
        "All_Helmet",
        "HeadCoverings_Base_Hair",
        "HeadCoverings_No_FacialHair",
        "HeadCoverings_No_Hair",
        "All_03_Chest_Attachment",
        "All_04_Back_Attachment",
        "All_05_Shoulder_Attachment_Right",
        "All_06_Shoulder_Attachment_Left",
        "All_07_Elbow_Attachment_Right",
        "All_08_Elbow_Attachment_Left",
        "All_09_Hips_Attachment",
        "All_10_Knee_Attachement_Right",
        "All_11_Knee_Attachement_Left"
    };

    #endregion end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {

    }
    // end Awake

    // Start is called before the first frame update
    void Start()
    {
        // Use the options defined in the customization object
        UseCustomOptions();
    }
    // end Start

    // Update is called once per frame
    void Update()
    {
    
    }
    // end Update

    #endregion end Unity Control Methods

    #region Customization

    /// <summary>
    /// Update the customizer to use the passed customization object
    /// </summary>
    /// <param name="_customization">The new customization object the customizer should use</param>
    public void SetCustomization(Customization _customization)
    {
        customization = _customization; // Update the customization object using the passed object
        UseCustomOptions();             // Update the enabled objects according to the new customization settings
        UpdateMaterialsInChildren();    // Update the materials of the enabled objects to the new customization settings
    }// SetCustomization

    /// <summary>
    /// Update the materials of the modular objects used by the customizer
    /// </summary>
    [ContextMenu("Utils/Update Materials In Children")]
    private void UpdateMaterialsInChildren()
    {
        // Get the renderers in the modular character
        Renderer[] _children = transform.GetComponentsInChildren<Renderer>();

        // Update every renderer to use the customization object's material
        foreach (Renderer rend in _children)
        {
            rend.material = customization.Material;
        }
    }//end UpdateMaterialsInChildren

    /// <summary>
    /// Enable and Disable the correct modular objects to match the customization settings
    /// </summary>
    [ContextMenu("Utils/Use Custom Options")]
    private void UseCustomOptions()
    {
        // Ensure the object lists are correct and every modular object is disabled
        BuildLists();

        // Loop through the customization options
        for (int i = 0; i < customization.Options.Length; i++)
        {
            // Get the customization option for the current index
            CustomizationOption option = customization.Options[i];

            // Work with the correct dictionary based on the category of the customization option
            // to enable the correct item in the list of modular objects
            switch (option.itemCategory)
            {
                case CustomizationCategory.Male:
                    ActivateItem(maleObjects[option.itemName][option.itemIndex]);
                    break;
                case CustomizationCategory.Female:
                    ActivateItem(femaleObjects[option.itemName][option.itemIndex]);
                    break;
                case CustomizationCategory.All:
                    ActivateItem(allObjects[option.itemName][option.itemIndex]);
                    break;
            }
        }
    }//end UseCustomOptions

    /// <summary>
    /// Enable all modular objects. A driver method meant to be exposed in the context menu
    /// </summary>
    [ContextMenu("Utils/Enable All Children")]
    private void EnableAllChildrenDriver()
    {
        // Calls the actual method
        EnableAllChildren(transform);
    }//end EnableAllChildrenDriver

    /// <summary>
    /// Recursive method called by EnableAllChildrenDriver to enable all modular objects
    /// </summary>
    /// <param name="_parent">The transform to start the recursive search from</param>
    private void EnableAllChildren(Transform _parent)
    {
        // For every child of the passed transform
        foreach (Transform trans in _parent)
        {
            // Enable the object
            trans.gameObject.SetActive(true);

            // Enable any children this transform has
            EnableAllChildren(trans);
        }
    }//end EnableAllChildren

    /// <summary>
    /// Use the currently enabled modular parts to fill out the script's customization object
    /// </summary>
    [ContextMenu("Utils/Write To Customization Object")]
    private void WriteCustomizationOptionsToObject()
    {
        // Get all of the modular parts that are currently enabled
        SkinnedMeshRenderer[] activeCharacterParts = GetComponentsInChildren<SkinnedMeshRenderer>();

        // Prepare the customization object for new data
        customization.Options = new CustomizationOption[activeCharacterParts.Length];

        // Loop through each modular part that needs to be added to the customization object
        int index = 0;
        foreach (SkinnedMeshRenderer characterPart in activeCharacterParts)
        {
            // Get the necessary info from the modular character about the part
            string partName = characterPart.transform.parent.name;
            int partNumber = characterPart.transform.GetSiblingIndex();
            char partCategory = partName[0];

            // Get the customization name enum value for this modular part
            int partIndex = System.Array.IndexOf(CUSTOMIZATION_NAMES, partName);
            CustomizationName customizationName = (CustomizationName)partIndex;

            // In case an enabled modular part is not included the options, log it
            if (partIndex == -1)
            {
                Debug.LogWarning($"Part enum does not exist! Name: |{partName}|");
            }

            // Get the customization category for this modular part
            CustomizationCategory customizationCategory = CustomizationCategory.All;
            switch(partCategory)
            {
                case 'A':
                    customizationCategory = CustomizationCategory.All;
                    break;
                case 'M':
                    customizationCategory = CustomizationCategory.Male;
                    break;
                case 'F':
                    customizationCategory = CustomizationCategory.Female;
                    break;
            }

            // Create a new option for this modular part at the correct index
            customization.Options[index] = new CustomizationOption() { itemName = customizationName, itemIndex = partNumber, itemCategory = customizationCategory };

            index++;
        }
    }//end WriteCustomizationOptionsToObject

    /// <summary>
    /// Enable game object and add it to the enabled objects list
    /// </summary>
    /// <param name="_go">The gameobject to be enabled</param>
    void ActivateItem(GameObject _go)
    {
        // Enable item
        _go.SetActive(true);

        // Add item to the enabled items list
        enabledObjects.Add(_go);
    }

    /// <summary>
    /// Setup the dictionaries used to reference the modular objects
    /// </summary>
    [ContextMenu("Utils/Build Lists")]
    private void BuildLists()
    {
        // Build out male lists
        BuildList(maleObjects, CustomizationName.MaleHeadAllElements);
        BuildList(maleObjects, CustomizationName.MaleHeadNoElements);
        BuildList(maleObjects, CustomizationName.MaleEyebrows);
        BuildList(maleObjects, CustomizationName.MaleFacialHair);
        BuildList(maleObjects, CustomizationName.MaleTorso);
        BuildList(maleObjects, CustomizationName.MaleArmUpperRight);
        BuildList(maleObjects, CustomizationName.MaleArmUpperLeft);
        BuildList(maleObjects, CustomizationName.MaleArmLowerRight);
        BuildList(maleObjects, CustomizationName.MaleArmLowerLeft);
        BuildList(maleObjects, CustomizationName.MaleHandRight);
        BuildList(maleObjects, CustomizationName.MaleHandLeft);
        BuildList(maleObjects, CustomizationName.MaleHips);
        BuildList(maleObjects, CustomizationName.MaleLegRight);
        BuildList(maleObjects, CustomizationName.MaleLegLeft);

        // Build out female lists
        BuildList(femaleObjects, CustomizationName.FemaleHeadAllElements);
        BuildList(femaleObjects, CustomizationName.FemaleHeadNoElements);
        BuildList(femaleObjects, CustomizationName.FemaleEyebrows);
        BuildList(femaleObjects, CustomizationName.FemaleFacialHair);
        BuildList(femaleObjects, CustomizationName.FemaleTorso);
        BuildList(femaleObjects, CustomizationName.FemaleArmUpperRight);
        BuildList(femaleObjects, CustomizationName.FemaleArmUpperLeft);
        BuildList(femaleObjects, CustomizationName.FemaleArmLowerRight);
        BuildList(femaleObjects, CustomizationName.FemaleArmLowerLeft);
        BuildList(femaleObjects, CustomizationName.FemaleHandRight);
        BuildList(femaleObjects, CustomizationName.FemaleHandLeft);
        BuildList(femaleObjects, CustomizationName.FemaleHips);
        BuildList(femaleObjects, CustomizationName.FemaleLegRight);
        BuildList(femaleObjects, CustomizationName.FemaleLegLeft);

        // Build out all gender lists
        BuildList(allObjects, CustomizationName.Hair);
        BuildList(allObjects, CustomizationName.Helmet);
        BuildList(allObjects, CustomizationName.HeadCoveringsBaseHair);
        BuildList(allObjects, CustomizationName.HeadCoveringsNoFacialHair);
        BuildList(allObjects, CustomizationName.HeadCoveringsNoHair);
        BuildList(allObjects, CustomizationName.ChestAttachment);
        BuildList(allObjects, CustomizationName.BackAttachment);
        BuildList(allObjects, CustomizationName.ShoulderAttachmentRight);
        BuildList(allObjects, CustomizationName.ShoulderAttachmentLeft);
        BuildList(allObjects, CustomizationName.ElbowAttachmentRight);
        BuildList(allObjects, CustomizationName.ElbowAttachmentLeft);
        BuildList(allObjects, CustomizationName.HipsAttachment);
        BuildList(allObjects, CustomizationName.KneeAttachmentRight);
        BuildList(allObjects, CustomizationName.KneeAttachmentLeft);
    }//end BuildLists

    /// <summary>
    /// Build the list for the passed character part within the passed dictionary or parts
    /// </summary>
    /// <param name="_objects">The dictionary the list will be added into</param>
    /// <param name="_characterPart">The character part to generate the list of modular parts for</param>
    private void BuildList(Dictionary<CustomizationName, List<GameObject>> _objects, CustomizationName _characterPart)
    {
        List<GameObject> _targetList = new List<GameObject>();
        Transform[] _rootTransform = gameObject.GetComponentsInChildren<Transform>(true);
        string _characterPartString = CustomizationNameToString(_characterPart);

        // Declare target root transform
        Transform _targetRoot = null;

        // Find character parts parent object in the scene
        foreach (Transform t in _rootTransform)
        {
            if (t.gameObject.name == _characterPartString)
            {
                _targetRoot = t;
                break;
            }
        }

        // If the dictionary is being re-defined, remove old defintions
        if (_objects.ContainsKey(_characterPart))
        {
            _objects.Remove(_characterPart);
        }

        // Cycle through all child objects of the parent object
        for (int i = 0; i < _targetRoot.childCount; i++)
        {
            // Get child gameobject index i
            GameObject _go = _targetRoot.GetChild(i).gameObject;

            // Disable child object
            _go.SetActive(false);

            // Add object to the targeted object list
            _targetList.Add(_go);
        }

        // Add the constructed list to the dictionary
        _objects.Add(_characterPart, _targetList);
    }//end Build List

    /// <summary>
    /// Converts the customization name enum to the appropriate string (object name in the modular character) and return the string
    /// </summary>
    /// <param name="customizationName">The customization name enum value to convert to a string</param>
    /// <returns></returns>
    private string CustomizationNameToString(CustomizationName customizationName)
    {
        return CUSTOMIZATION_NAMES[(int)customizationName];
    }//end CustomizationNameToString

    #endregion
}