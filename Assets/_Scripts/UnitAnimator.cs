using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager playerManager;

    public float speedSmoothTime;

    public Animator animator;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void SetSpeedPercent(float currentSpeed, float walkSpeed, float runSpeed, bool isSprinting)
    {
        float speedPercent = ((isSprinting) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
    }

    #endregion
}