using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control a unit's animator component
/// </summary>
public class UnitAnimator: MonoBehaviour
{
    #region Variables

    [SerializeField] private float speedSmoothTime;   // The time it takes to adjust the player's run speed animator value

    [SerializeField] private UnitAnimationExtension animatorExtension;

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

    /// <summary>
    /// Sets the `speedPercent` value in the animator according to the passed values
    /// </summary>
    /// <param name="_currentSpeed">The current speed that the unit is moving at</param>
    /// <param name="_walkSpeed">The unit's walk speed (lower speed limit)</param>
    /// <param name="_sprintSpeed">The unit's sprint speed (upper speed limit)</param>
    /// <param name="_isSprinting">Says whether or not the unit is sprinting</param>
    public void SetSpeedPercent(float _currentSpeed, float _walkSpeed, float _sprintSpeed, bool _isSprinting)
    {
        // Calculate the speed percent value using the passed values
        float _speedPercent = ((_isSprinting) ? _currentSpeed / _sprintSpeed : _currentSpeed / _walkSpeed * .5f);

        // Set the animator's `speedPercent` value smoothly according to the passed values
        animatorExtension.Animator.SetFloat("speedPercent", _speedPercent, speedSmoothTime, Time.deltaTime);
    }//end SetSpeedPercent

    /// <summary>
    /// Sets the `speedPercent` value in the animator according to the passed value. 
    /// This is more direct than the other overload
    /// </summary>
    /// <param name="_percent">A value between 0f and 1f</param>
    public void SetSpeedPercent(float _percent)
    {
        // Set the animator's `speedPercent` value smoothly according to the passed value
        animatorExtension.Animator.SetFloat("speedPercent", _percent, speedSmoothTime, Time.deltaTime);
    }//end SetSpeedPercent

    #endregion
}