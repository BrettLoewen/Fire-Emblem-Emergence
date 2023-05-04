using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCustomizer : MonoBehaviour
{
    #region Variables

    [SerializeField] private Material materialForChildren;

    [SerializeField] private Customization customization;

    // list of enabed objects on character
    private List<GameObject> enabledObjects = new List<GameObject>();

    [SerializeField] private Dictionary<string, List<GameObject>> maleOptions = new Dictionary<string, List<GameObject>>();
    [SerializeField] private Dictionary<string, List<GameObject>> femaleOptions = new Dictionary<string, List<GameObject>>();
    [SerializeField] private Dictionary<string, List<GameObject>> allOptions = new Dictionary<string, List<GameObject>>();

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

    public void SetCustomization(Customization _customization)
    {
        customization = _customization;
        UseCustomOptions();
        UpdateMaterialsInChildren();
    }

    [ContextMenu("Utils/Update Materials In Children")]
    private void UpdateMaterialsInChildren()
    {
        Renderer[] children = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            rend.material = customization.material;
        }
    }

    [ContextMenu("Utils/Use Custom Options")]
    private void UseCustomOptions()
    {
        BuildLists();

        // loop through the customization options
        for (int i = 0; i < customization.options.Length; i++)
        {
            // get the customization option
            CustomizationOption option = customization.options[i];

            // work with the correct dictionary based on the category of the customization option
            switch (option.itemCategory)
            {
                case CustomizationCategory.Male:
                    ActivateItem(maleOptions[option.itemName][option.itemIndex]);
                    break;
                case CustomizationCategory.Female:
                    ActivateItem(femaleOptions[option.itemName][option.itemIndex]);
                    break;
                case CustomizationCategory.All:
                    ActivateItem(allOptions[option.itemName][option.itemIndex]);
                    break;
            }
        }
    }

    [ContextMenu("Utils/Enable All Children")]
    private void EnableAllChildrenDriver()
    {
        EnableAllChildren(transform);
    }

    // recursive method called by EnableAllChildrenDriver
    private void EnableAllChildren(Transform parent)
    {
        foreach (Transform trans in parent)
        {
            trans.gameObject.SetActive(true);
            EnableAllChildren(trans);
        }
    }

    // enable game object and add it to the enabled objects list
    void ActivateItem(GameObject go)
    {
        // enable item
        go.SetActive(true);

        // add item to the enabled items list
        enabledObjects.Add(go);
    }

    [ContextMenu("Utils/Build Lists")]
    // build all item lists for use in randomization
    private void BuildLists()
    {
        // build out male lists
        BuildList(maleOptions, "Male_Head_All_Elements");
        BuildList(maleOptions, "Male_Head_No_Elements");
        BuildList(maleOptions, "Male_01_Eyebrows");
        BuildList(maleOptions, "Male_02_FacialHair");
        BuildList(maleOptions, "Male_03_Torso");
        BuildList(maleOptions, "Male_04_Arm_Upper_Right");
        BuildList(maleOptions, "Male_05_Arm_Upper_Left");
        BuildList(maleOptions, "Male_06_Arm_Lower_Right");
        BuildList(maleOptions, "Male_07_Arm_Lower_Left");
        BuildList(maleOptions, "Male_08_Hand_Right");
        BuildList(maleOptions, "Male_09_Hand_Left");
        BuildList(maleOptions, "Male_10_Hips");
        BuildList(maleOptions, "Male_11_Leg_Right");
        BuildList(maleOptions, "Male_12_Leg_Left");

        // build out female lists
        BuildList(femaleOptions, "Female_Head_All_Elements");
        BuildList(femaleOptions, "Female_Head_No_Elements");
        BuildList(femaleOptions, "Female_01_Eyebrows");
        BuildList(femaleOptions, "Female_02_FacialHair");
        BuildList(femaleOptions, "Female_03_Torso");
        BuildList(femaleOptions, "Female_04_Arm_Upper_Right");
        BuildList(femaleOptions, "Female_05_Arm_Upper_Left");
        BuildList(femaleOptions, "Female_06_Arm_Lower_Right");
        BuildList(femaleOptions, "Female_07_Arm_Lower_Left");
        BuildList(femaleOptions, "Female_08_Hand_Right");
        BuildList(femaleOptions, "Female_09_Hand_Left");
        BuildList(femaleOptions, "Female_10_Hips");
        BuildList(femaleOptions, "Female_11_Leg_Right");
        BuildList(femaleOptions, "Female_12_Leg_Left");

        // build out all gender lists
        BuildList(allOptions, "All_01_Hair");
        BuildList(allOptions, "All_Helmet");
        BuildList(allOptions, "HeadCoverings_Base_Hair");
        BuildList(allOptions, "HeadCoverings_No_FacialHair");
        BuildList(allOptions, "HeadCoverings_No_Hair");
        BuildList(allOptions, "All_03_Chest_Attachment");
        BuildList(allOptions, "All_04_Back_Attachment");
        BuildList(allOptions, "All_05_Shoulder_Attachment_Right");
        BuildList(allOptions, "All_06_Shoulder_Attachment_Left");
        BuildList(allOptions, "All_07_Elbow_Attachment_Right");
        BuildList(allOptions, "All_08_Elbow_Attachment_Left");
        BuildList(allOptions, "All_09_Hips_Attachment");
        BuildList(allOptions, "All_10_Knee_Attachement_Right");
        BuildList(allOptions, "All_11_Knee_Attachement_Left");
        BuildList(allOptions, "Elf_Ear");
    }

    private void BuildList(Dictionary<string, List<GameObject>> options, string characterPart)
    {
        List<GameObject> targetList = new List<GameObject>();
        Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>(true);

        // declare target root transform
        Transform targetRoot = null;

        // find character parts parent object in the scene
        foreach (Transform t in rootTransform)
        {
            if (t.gameObject.name == characterPart)
            {
                targetRoot = t;
                break;
            }
        }

        // if the dictionary is being re - defined, remove old defintions
        if (options.ContainsKey(characterPart))
        {
            options.Remove(characterPart);
        }

        // cycle through all child objects of the parent object
        for (int i = 0; i < targetRoot.childCount; i++)
        {
            // get child gameobject index i
            GameObject go = targetRoot.GetChild(i).gameObject;

            // disable child object
            go.SetActive(false);

            // add object to the targeted object list
            targetList.Add(go);

            // collect the material for the random character, only if null in the inspector;
            if (!materialForChildren)
            {
                if (go.GetComponent<SkinnedMeshRenderer>())
                {
                    materialForChildren = go.GetComponent<SkinnedMeshRenderer>().material;
                }
            }
        }

        // add the constructed list to the dictionary
        options.Add(characterPart, targetList);
    }

    #endregion
}