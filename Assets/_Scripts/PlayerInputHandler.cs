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
    //public bool jumpInput; //Stores input for jumping
    //public float targetLockInput; //Stores input for target locking
    public bool interactInput; //Stores input interacting
    public bool menuInput; //Stores input for the menu button (opening/closing the menu)
    //public bool rightBumperInput; //Stores input for the right bumper
    //public bool leftBumperInput; //Stores input for the left bumper
    //public bool rightUIInput; //Stores input for the right trigger and right stick moving right (for UI)
    //public bool leftUIInput; //Stores input for the left trigger and right stick moving left (for UI)

    //public UnityAction onSubmit; //Action to invoke for UI stuff
    //public UnityAction onRightUI; //Action to invoke for UI stuff
    //public UnityAction onLeftUI; //Action to invoke for UI stuff

    //Receive and store the input for movement
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        //Prevent input from being gathered and stored while in the pause menu
        //if (PauseMenu.IsPaused)
        //{
        //    moveInput = Vector2.zero;
        //}
    }

    //Receive and store the input for looking
    public void OnLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

        //Prevent input from being gathered and stored while in the pause menu
        //if (PauseMenu.IsPaused)
        //{
        //    lookInput = Vector2.zero;
        //}
    }

    //Receive and store the input for sprinting
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        sprintInput = !sprintInput;

        //Prevent input from being gathered and stored while in the pause menu
        //if (PauseMenu.IsPaused)
        //{
        //    sprintInput = false;
        //}
    }

    //Receive and store the input for interacting in the game world. Invoke methods associated with submitting in the menu
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //If the onSubmit action has methods tied to it, call them
            //if (onSubmit != null)
            //{
            //    onSubmit.Invoke();
            //}

            interactInput = true;
        }

        //Prevent input from being gathered and stored while in the pause menu
        //if (PauseMenu.IsPaused)
        //{
        //    interactInput = false;
        //}
    }

    //Receive and store the input for the menu button
    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            menuInput = true;
        }
    }

    //Receive and store the input for the right bumper
    //public void OnRightBumperInput(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        rightBumperInput = true;
    //    }
    //}

    ////Receive and store the input for the left bumper
    //public void OnLeftBumperInput(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        leftBumperInput = true;
    //    }
    //}

    ////Receive and store the input for the right trigger and right stick moving right
    //public void OnRightUIInput(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        //rightUIInput = true;
    //        if (onRightUI != null)
    //        {
    //            onRightUI.Invoke();
    //        }
    //    }
    //}

    ////Receive and store the input for the left trigger and right stick moving left
    //public void OnLeftUIInput(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        //leftUIInput = true;
    //        if (onLeftUI != null)
    //        {
    //            onLeftUI.Invoke();
    //        }
    //    }
    //}
}
