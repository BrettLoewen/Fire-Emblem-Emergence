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
    [SerializeField] private Dictionary<string, List<GameObject>> maleObjects = new Dictionary<string, List<GameObject>>();
    [SerializeField] private Dictionary<string, List<GameObject>> femaleObjects = new Dictionary<string, List<GameObject>>();
    [SerializeField] private Dictionary<string, List<GameObject>> allObjects = new Dictionary<string, List<GameObject>>();

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
        BuildList(maleObjects, "Male_Head_All_Elements");
        BuildList(maleObjects, "Male_Head_No_Elements");
        BuildList(maleObjects, "Male_01_Eyebrows");
        BuildList(maleObjects, "Male_02_FacialHair");
        BuildList(maleObjects, "Male_03_Torso");
        BuildList(maleObjects, "Male_04_Arm_Upper_Right");
        BuildList(maleObjects, "Male_05_Arm_Upper_Left");
        BuildList(maleObjects, "Male_06_Arm_Lower_Right");
        BuildList(maleObjects, "Male_07_Arm_Lower_Left");
        BuildList(maleObjects, "Male_08_Hand_Right");
        BuildList(maleObjects, "Male_09_Hand_Left");
        BuildList(maleObjects, "Male_10_Hips");
        BuildList(maleObjects, "Male_11_Leg_Right");
        BuildList(maleObjects, "Male_12_Leg_Left");

        // Build out female lists
        BuildList(femaleObjects, "Female_Head_All_Elements");
        BuildList(femaleObjects, "Female_Head_No_Elements");
        BuildList(femaleObjects, "Female_01_Eyebrows");
        BuildList(femaleObjects, "Female_02_FacialHair");
        BuildList(femaleObjects, "Female_03_Torso");
        BuildList(femaleObjects, "Female_04_Arm_Upper_Right");
        BuildList(femaleObjects, "Female_05_Arm_Upper_Left");
        BuildList(femaleObjects, "Female_06_Arm_Lower_Right");
        BuildList(femaleObjects, "Female_07_Arm_Lower_Left");
        BuildList(femaleObjects, "Female_08_Hand_Right");
        BuildList(femaleObjects, "Female_09_Hand_Left");
        BuildList(femaleObjects, "Female_10_Hips");
        BuildList(femaleObjects, "Female_11_Leg_Right");
        BuildList(femaleObjects, "Female_12_Leg_Left");

        // Build out all gender lists
        BuildList(allObjects, "All_01_Hair");
        BuildList(allObjects, "All_Helmet");
        BuildList(allObjects, "HeadCoverings_Base_Hair");
        BuildList(allObjects, "HeadCoverings_No_FacialHair");
        BuildList(allObjects, "HeadCoverings_No_Hair");
        BuildList(allObjects, "All_03_Chest_Attachment");
        BuildList(allObjects, "All_04_Back_Attachment");
        BuildList(allObjects, "All_05_Shoulder_Attachment_Right");
        BuildList(allObjects, "All_06_Shoulder_Attachment_Left");
        BuildList(allObjects, "All_07_Elbow_Attachment_Right");
        BuildList(allObjects, "All_08_Elbow_Attachment_Left");
        BuildList(allObjects, "All_09_Hips_Attachment");
        BuildList(allObjects, "All_10_Knee_Attachement_Right");
        BuildList(allObjects, "All_11_Knee_Attachement_Left");
        BuildList(allObjects, "Elf_Ear");
    }//end BuildLists

    /// <summary>
    /// Build the list for the passed character part within the passed dictionary or parts
    /// </summary>
    /// <param name="_objects">The dictionary the list will be added into</param>
    /// <param name="_characterPart">The character part to generate the list of modular parts for</param>
    private void BuildList(Dictionary<string, List<GameObject>> _objects, string _characterPart)
    {
        List<GameObject> _targetList = new List<GameObject>();
        Transform[] _rootTransform = gameObject.GetComponentsInChildren<Transform>(true);

        // Declare target root transform
        Transform _targetRoot = null;

        // Find character parts parent object in the scene
        foreach (Transform t in _rootTransform)
        {
            if (t.gameObject.name == _characterPart)
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

    #endregion
}