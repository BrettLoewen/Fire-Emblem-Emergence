using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationExtension: MonoBehaviour
{
    #region Variables

    public Animator Animator { get; private set; }   // References the unit's animator component

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        // Get the animator component
        Animator = GetComponent<Animator>();
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