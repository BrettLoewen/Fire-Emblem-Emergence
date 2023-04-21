using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularCharacter
{
    public class UnitCustomizer : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Material materialForChildren;

        // list of enabed objects on character
        private List<GameObject> enabledObjects = new List<GameObject>();

        // character object lists
        private CharacterObjectGroups male = new CharacterObjectGroups(); // male list
        private CharacterObjectGroups female = new CharacterObjectGroups();   // female list
        private CharacterObjectListsAllGender allGender = new CharacterObjectListsAllGender();    // universal list

        [SerializeField] private CustomizationOption head;
        [SerializeField] private CustomizationOption eyebrow;
        [SerializeField] private CustomizationOption facialHair;
        [SerializeField] private CustomizationOption torso;
        [SerializeField] private CustomizationOption arm_Upper_Right;
        [SerializeField] private CustomizationOption arm_Upper_Left;
        [SerializeField] private CustomizationOption arm_Lower_Right;
        [SerializeField] private CustomizationOption arm_Lower_Left;
        [SerializeField] private CustomizationOption hand_Right;
        [SerializeField] private CustomizationOption hand_Left;
        [SerializeField] private CustomizationOption hips;
        [SerializeField] private CustomizationOption leg_Right;
        [SerializeField] private CustomizationOption leg_Left;

        #endregion //end Variables

        #region Unity Control Methods

        // Awake is called before Start before the first frame update
        void Awake()
        {

        }//end Awake

        // Start is called before the first frame update
        void Start()
        {
            //// rebuild all lists
            //BuildLists();

            //// disable any enabled objects before clear
            //if (enabledObjects.Count != 0)
            //{
            //    foreach (GameObject g in enabledObjects)
            //    {
            //        g.SetActive(false);
            //    }
            //}

            //// clear enabled objects list
            //enabledObjects.Clear();
        }//end Start

        // Update is called once per frame
        void Update()
        {

        }//end Update

        #endregion //end Unity Control Methods

        #region

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Utils/Update Materials In Children")]
        private void UpdateMaterialsInChildren()
        {
            Renderer[] children = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in children)
            {
                rend.material = materialForChildren;
            }
        }

        [ContextMenu("Utils/Use Custom Options")]
        private void UseCustomOptions()
        {
            BuildLists();

            // set default male character
            ActivateItem(male.headAllElements[head.itemIndex]);
            ActivateItem(male.eyebrow[head.itemIndex]);
            ActivateItem(male.facialHair[head.itemIndex]);
            ActivateItem(male.torso[head.itemIndex]);
            ActivateItem(male.arm_Upper_Right[head.itemIndex]);
            ActivateItem(male.arm_Upper_Left[head.itemIndex]);
            ActivateItem(male.arm_Lower_Right[head.itemIndex]);
            ActivateItem(male.arm_Lower_Left[head.itemIndex]);
            ActivateItem(male.hand_Right[head.itemIndex]);
            ActivateItem(male.hand_Left[head.itemIndex]);
            ActivateItem(male.hips[head.itemIndex]);
            ActivateItem(male.leg_Right[head.itemIndex]);
            ActivateItem(male.leg_Left[head.itemIndex]);
        }

        [ContextMenu("Utils/Use Default Male Customization")]
        private void UseDefaultMaleCustomization()
        {
            BuildLists();

            // set default male character
            ActivateItem(male.headAllElements[0]);
            ActivateItem(male.eyebrow[0]);
            ActivateItem(male.facialHair[0]);
            ActivateItem(male.torso[0]);
            ActivateItem(male.arm_Upper_Right[0]);
            ActivateItem(male.arm_Upper_Left[0]);
            ActivateItem(male.arm_Lower_Right[0]);
            ActivateItem(male.arm_Lower_Left[0]);
            ActivateItem(male.hand_Right[0]);
            ActivateItem(male.hand_Left[0]);
            ActivateItem(male.hips[0]);
            ActivateItem(male.leg_Right[0]);
            ActivateItem(male.leg_Left[0]);
        }

        [ContextMenu("Utils/Enable All Children")]
        private void EnableAllChildrenDriver()
        {
            EnableAllChildren(transform);
        }
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

        //[ContextMenu("Utils/Build Lists")]
        // build all item lists for use in randomization
        private void BuildLists()
        {
            //build out male lists
            BuildList(male.headAllElements, "Male_Head_All_Elements");
            BuildList(male.headNoElements, "Male_Head_No_Elements");
            BuildList(male.eyebrow, "Male_01_Eyebrows");
            BuildList(male.facialHair, "Male_02_FacialHair");
            BuildList(male.torso, "Male_03_Torso");
            BuildList(male.arm_Upper_Right, "Male_04_Arm_Upper_Right");
            BuildList(male.arm_Upper_Left, "Male_05_Arm_Upper_Left");
            BuildList(male.arm_Lower_Right, "Male_06_Arm_Lower_Right");
            BuildList(male.arm_Lower_Left, "Male_07_Arm_Lower_Left");
            BuildList(male.hand_Right, "Male_08_Hand_Right");
            BuildList(male.hand_Left, "Male_09_Hand_Left");
            BuildList(male.hips, "Male_10_Hips");
            BuildList(male.leg_Right, "Male_11_Leg_Right");
            BuildList(male.leg_Left, "Male_12_Leg_Left");

            //build out female lists
            BuildList(female.headAllElements, "Female_Head_All_Elements");
            BuildList(female.headNoElements, "Female_Head_No_Elements");
            BuildList(female.eyebrow, "Female_01_Eyebrows");
            BuildList(female.facialHair, "Female_02_FacialHair");
            BuildList(female.torso, "Female_03_Torso");
            BuildList(female.arm_Upper_Right, "Female_04_Arm_Upper_Right");
            BuildList(female.arm_Upper_Left, "Female_05_Arm_Upper_Left");
            BuildList(female.arm_Lower_Right, "Female_06_Arm_Lower_Right");
            BuildList(female.arm_Lower_Left, "Female_07_Arm_Lower_Left");
            BuildList(female.hand_Right, "Female_08_Hand_Right");
            BuildList(female.hand_Left, "Female_09_Hand_Left");
            BuildList(female.hips, "Female_10_Hips");
            BuildList(female.leg_Right, "Female_11_Leg_Right");
            BuildList(female.leg_Left, "Female_12_Leg_Left");

            // build out all gender lists
            BuildList(allGender.all_Hair, "All_01_Hair");
            BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
            BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
            BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
            BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
            BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
            BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
            BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
            BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
            BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
            BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
            BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
            BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
            BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
            BuildList(allGender.elf_Ear, "Elf_Ear");
        }

        // called from the BuildLists method
        void BuildList(List<GameObject> targetList, string characterPart)
        {
            Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();

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

            // clears targeted list of all objects
            targetList.Clear();

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
                        materialForChildren = go.GetComponent<SkinnedMeshRenderer>().material;
                }
            }
        }

        #endregion
    }

    // classe for keeping the lists organized, allows for simple switching from male/female objects
    [System.Serializable]
    public class CharacterObjectGroups
    {
        public List<GameObject> headAllElements;
        public List<GameObject> headNoElements;
        public List<GameObject> eyebrow;
        public List<GameObject> facialHair;
        public List<GameObject> torso;
        public List<GameObject> arm_Upper_Right;
        public List<GameObject> arm_Upper_Left;
        public List<GameObject> arm_Lower_Right;
        public List<GameObject> arm_Lower_Left;
        public List<GameObject> hand_Right;
        public List<GameObject> hand_Left;
        public List<GameObject> hips;
        public List<GameObject> leg_Right;
        public List<GameObject> leg_Left;
    }

    // classe for keeping the lists organized, allows for organization of the all gender items
    [System.Serializable]
    public class CharacterObjectListsAllGender
    {
        public List<GameObject> headCoverings_Base_Hair;
        public List<GameObject> headCoverings_No_FacialHair;
        public List<GameObject> headCoverings_No_Hair;
        public List<GameObject> all_Hair;
        public List<GameObject> all_Head_Attachment;
        public List<GameObject> chest_Attachment;
        public List<GameObject> back_Attachment;
        public List<GameObject> shoulder_Attachment_Right;
        public List<GameObject> shoulder_Attachment_Left;
        public List<GameObject> elbow_Attachment_Right;
        public List<GameObject> elbow_Attachment_Left;
        public List<GameObject> hips_Attachment;
        public List<GameObject> knee_Attachement_Right;
        public List<GameObject> knee_Attachement_Left;
        public List<GameObject> all_12_Extra;
        public List<GameObject> elf_Ear;
    }

    [System.Serializable]
    public class CustomizationOption
    {
        public int itemIndex;
    }
}