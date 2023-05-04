using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [HideInInspector] public PlayerManager playerManager;

    public Vector2 moveInput; //Stores input for movement
    public Vector2 lookInput; //Stores input for camera control
    public bool sprintInput; //Stores input for sprinting
    public bool interactInput; //Stores input interacting
    public bool menuInput; //Stores input for the menu button (opening/closing the menu)
    public bool cancelInput; //Stores input for canceling in a menu

    //Receive and store the input for movement
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //Receive and store the input for looking
    public void OnLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    //Receive and store the input for sprinting
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        sprintInput = !sprintInput;
    }

    //Receive and store the input for interacting in the game world. Invoke methods associated with submitting in the menu
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(ExplorationGameManager.Instance.explorationState == ExplorationState.Explore)
        {
            if (context.started)
            {
                interactInput = true;
            }
        }
    }

    //Receive and store the input for the menu button
    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            menuInput = true;
        }
    }

    //Receive and store the input for the cancel button
    public void OnCancelInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cancelInput = true;
        }
    }
}
