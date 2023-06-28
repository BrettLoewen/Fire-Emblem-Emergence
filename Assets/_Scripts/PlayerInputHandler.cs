using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput;   //Stores input for movement
    public Vector2 LookInput;   //Stores input for camera control
    public bool SprintInput;    //Stores input for sprinting
    public bool InteractInput;  //Stores input interacting
    public bool MenuInput;      //Stores input for the menu button (opening/closing the menu)
    public bool CancelInput;    //Stores input for canceling in a menu
    public bool LookRightInput;
    public bool LookLeftInput;

    /// <summary>
    /// Receive and store the input for movement
    /// </summary>
    /// <param name="_context"></param>
    public void OnMoveInput(InputAction.CallbackContext _context)
    {
        MoveInput = _context.ReadValue<Vector2>();
    }//end OnMoveInput

    /// <summary>
    /// Receive and store the input for looking
    /// </summary>
    /// <param name="_context"></param>
    public void OnLookInput(InputAction.CallbackContext _context)
    {
        LookInput = _context.ReadValue<Vector2>();
    }//end OnLookInput

    /// <summary>
    /// Receive and store the input for pressing right on the d-pad in a tactics scene
    /// </summary>
    /// <param name="_context"></param>
    public void OnLookRightInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            LookRightInput = true;
        }
    }//end OnLookRightInput

    /// <summary>
    /// Receive and store the input for pressing left on the d-pad in a tactics scene
    /// </summary>
    /// <param name="_context"></param>
    public void OnLookLefttInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            LookLeftInput = true;
        }
    }//end OnLookLefttInput

    /// <summary>
    /// Receive and store the input for sprinting
    /// </summary>
    /// <param name="_context"></param>
    public void OnSprintInput(InputAction.CallbackContext _context)
    {
        SprintInput = !SprintInput;
    }//end OnSprintInput

    /// <summary>
    /// Receive and store the input for interacting in the game world. Invoke methods associated with submitting in the menu
    /// </summary>
    /// <param name="_context"></param>
    public void OnInteractInput(InputAction.CallbackContext _context)
    {
        // Only allow interacting while the game is in exploration mode
        if(ExplorationGameManager.Instance.ExplorationState == ExplorationState.Explore)
        {
            if (_context.started)
            {
                InteractInput = true;
            }
        }
    }//end OnInteractInput

    /// <summary>
    /// Receive and store the input for the menu button
    /// </summary>
    /// <param name="_context"></param>
    public void OnMenuInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            MenuInput = true;
        }
    }//end OnMenuInput

    /// <summary>
    /// Receive and store the input for the cancel button
    /// </summary>
    /// <param name="_context"></param>
    public void OnCancelInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            CancelInput = true;
        }
    }//end OnCancelInput
}
