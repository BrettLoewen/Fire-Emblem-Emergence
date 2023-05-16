using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TooltipSelectableType { Generic, ItemDisplay }

public class TooltipSelectable: MonoBehaviour
{
    #region Variables

    [field: SerializeField] public TooltipSelectableType Type { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] [field: TextArea(3, 10)] public string Description { get; private set; }

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region



    #endregion
}