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
    }

    /// <summary>
    /// Receive and store the input for sprinting
    /// </summary>
    /// <param name="_context"></param>
    public void OnSprintInput(InputAction.CallbackContext _context)
    {
        SprintInput = !SprintInput;
    }

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
    }

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
    }

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
    }
}
