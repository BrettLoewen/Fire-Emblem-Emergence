using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// Used to control a unit's animator component
/// </summary>
public class UnitAnimator: MonoBehaviour
{
    #region Variables

    [SerializeField] private float speedSmoothTime;   // The time it takes to adjust the player's run speed animator value

    private string speedPercentString = "speedPercent";
    private string attackString = "attack";
    private string deathString = "death";

    [SerializeField] private UnitAnimationExtension animatorExtension;
    public Transform CharacterTransform { get; private set; }

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        CharacterTransform = animatorExtension.transform;
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

    #region Animation Control

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
        animatorExtension.Animator.SetFloat(speedPercentString, _speedPercent, speedSmoothTime, Time.deltaTime);
    }//end SetSpeedPercent

    /// <summary>
    /// Sets the `speedPercent` value in the animator according to the passed value. 
    /// This is more direct than the other overload
    /// </summary>
    /// <param name="_percent">A value between 0f and 1f</param>
    public void SetSpeedPercent(float _percent)
    {
        // Set the animator's `speedPercent` value smoothly according to the passed value
        animatorExtension.Animator.SetFloat(speedPercentString, _percent, speedSmoothTime, Time.deltaTime);
    }//end SetSpeedPercent

    /// <summary>
    /// Player the animator's attack animation
    /// </summary>
    public async Task TriggerAttack()
    {
        // Apply root motion so the attack animation plays properly
        animatorExtension.Animator.applyRootMotion = true;

        // Trigger the animator's attack trigger
        animatorExtension.Animator.SetTrigger(attackString);

        // Wait for the attack animation to finish
        await Task.Delay(1333);

        // Stop root motion so other animations play properly
        animatorExtension.Animator.applyRootMotion = false;
    }//end TriggerAttack

    /// <summary>
    /// Player the animator's death animation
    /// </summary>
    public void TriggerDeath()
    {
        // Apply root motion so the death animation plays properly
        animatorExtension.Animator.applyRootMotion = true;

        // Trigger the animator's death trigger
        animatorExtension.Animator.SetTrigger(deathString);
    }//end TriggerDeath

    #endregion
}