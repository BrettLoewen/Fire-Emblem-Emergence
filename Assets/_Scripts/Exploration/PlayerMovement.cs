using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move the player character
/// </summary>
public class PlayerMovement: MonoBehaviour
{
    #region Variables

    // References to other player control components
    [HideInInspector] public PlayerManager PlayerManager;
    private PlayerInputHandler inputHandler;
    private UnitAnimator playerAnimator;
    private PlayerCamera playerCamera;

    // Variables to control the movement speed
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float currentSpeed;

    // Variables to control the turn speed
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    // Variables to keep the player on the ground
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float minDistanceOff = 0.05f;
      
    // The character controller responsible for actually moving
    private CharacterController controller;

    // The manager for the exploration scene
    private ExplorationGameManager gameManager;
      
    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        // Get the various references needed to operate
        inputHandler = PlayerManager.InputHandler;
        playerCamera = PlayerManager.PlayerCamera;
        playerAnimator = PlayerManager.PlayerAnimator;
        controller = GetComponent<CharacterController>();

        currentSpeed = walkSpeed;

        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Keep the player character on the ground
        HandleGravity();

        // If the game is in explore mode, then the player should be able to move their character
        if (gameManager.ExplorationState == ExplorationState.Explore)
        {
            // Move the player character according to input
            HandleMovement();

            // Store the player's current position so it can be saved
            SaveSystem.SetPlayerPositionAndRotation(transform.position, transform.rotation);
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Move the player according to player input
    /// </summary>
    private void HandleMovement()
    {
        //Get the direction of movement
        Vector2 _moveInput = inputHandler.MoveInput;
        Vector3 _direction = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        //If there is movement input
        if (_direction.magnitude >= 0.1f)
        {
            //Get and smooth the angle for the movement direction relative to the camera
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + playerCamera.MainCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Rotate the player to the angle
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            //Turn the move angle into a new movement direction
            Vector3 _moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Set the current speed based on the sprint input
            if (inputHandler.SprintInput)
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            //Move the character
            controller.Move(_moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            //Stop sprinting if there is no movement input
            inputHandler.SprintInput = false;

            //Set the current speed to 0 if there is no movement input
            currentSpeed = 0f;
        }

        //If the character should be moving
        if (currentSpeed > 0f)
        {
            //Set the current speed based on the actual velocity of the character controller
            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        }

        //Animate the character according to the current speed
        playerAnimator.SetSpeedPercent(currentSpeed, walkSpeed, sprintSpeed, inputHandler.SprintInput);
    }

    /// <summary>
    /// Keep the player on the ground
    /// </summary>
    public void HandleGravity()
    {
        // Determine whether or not the player is currently on the ground
        bool _result = Physics.Raycast(groundCheckPoint.position, Vector3.down, minDistanceOff, groundMask);

        // If the player is not currently on the ground, move it downward
        if (_result == false)
        { 
            float _fallSpeed = 2f;

            controller.Move(Vector3.down * _fallSpeed * Time.deltaTime);
        }

    }

    #endregion
}