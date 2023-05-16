using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Tooltip: Singleton<Tooltip>
{
    #region Variables

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public bool IsActive { get; private set; }
    private GameObject currentSelectedObject;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if(selectedObject.Equals(currentSelectedObject) == false)
            {
                currentSelectedObject = selectedObject;
                UpdateTooltip();
            }
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region


    public void EnableTooltip()
    {
        IsActive = true;
        tooltipObject.SetActive(true);

        currentSelectedObject = EventSystem.current.currentSelectedGameObject;

        UpdateTooltip();
    }


    public void DisableTooltip()
    {
        IsActive = false;

        currentSelectedObject = null;

        tooltipObject.SetActive(false);
    }


    private void UpdateTooltip()
    {
        if(currentSelectedObject == null)
        {
            tooltipObject.SetActive(false);
            return;
        }

        TooltipSelectable selectable = currentSelectedObject.GetComponent<TooltipSelectable>();

        if(selectable == null)
        {
            tooltipObject.SetActive(false);
            return;
        }

        tooltipObject.SetActive(true);

        RectTransform selectableTransform = selectable.transform as RectTransform;
        RectTransform tooltipTransform = transform as RectTransform;
        Debug.Log(selectableTransform.position);
        //tooltipTransform.rect.Set(selectableTransform.rect.xMin, selectableTransform.rect.yMax, tooltipTransform.rect.width, tooltipTransform.rect.height);
        //tooltipTransform.ForceUpdateRectTransforms();

        tooltipTransform.position = new Vector2(selectableTransform.position.x, selectableTransform.position.y) + (Vector2.down * selectableTransform.rect.height);
        Debug.Log(tooltipTransform.position);

        TooltipSelectableType type = selectable.Type;

        if (type == TooltipSelectableType.Generic)
        {
            tooltipText.text = $"<b>{selectable.Title}</b>\n\n{selectable.Description}";
        }
        else if(type == TooltipSelectableType.ItemDisplay)
        {
            ItemDisplay display = selectable.GetComponent<ItemDisplay>();
            ItemData item = display.GetItem().ItemData;

            // If the item 
            if(item.ItemType == ItemType.Generic)
            {
                tooltipText.text = $"<b>{item.Name}</b>\n\n{item.Description}";
            }
            else if(item.ItemType == ItemType.Weapon)
            {
                WeaponData weapon = item as WeaponData;
                tooltipText.text = $"<b>{item.Name}</b>\n\nMight: {weapon.Might}\nWeight: {weapon.Weight}\n\n{item.Description}";
            }
        }
    }

    #endregion
}