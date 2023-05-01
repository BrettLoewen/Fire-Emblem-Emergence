using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager playerManager;
    private PlayerInputHandler inputHandler;
    private UnitAnimator playerAnimator;
    private PlayerCamera playerCamera;

    //Variables to control the movement speed
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float currentSpeed;

    //Variables to control the turn speed
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public Transform groundCheckPoint;
    public LayerMask groundMask;
    public float minDistanceOff = 0.05f;
      
    private CharacterController controller;

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
        //Get the various references needed to operate
        inputHandler = playerManager.inputHandler;
        playerCamera = playerManager.playerCamera;
        playerAnimator = playerManager.playerAnimator;
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
        if (gameManager.explorationState == ExplorationState.Explore)
        {
            // Move the player character according to input
            HandleMovement();

            // Store the player's current position so it can be saved
            SaveSystem.SetPlayerPositionAndRotation(transform.position, transform.rotation);
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    //
    private void HandleMovement()
    {
        //Get the direction of movement
        Vector2 moveInput = inputHandler.moveInput;
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        //If there is movement input
        if (direction.magnitude >= 0.1f)
        {
            //Get and smooth the angle for the movement direction relative to the camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.mainCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Rotate the player to the angle
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            //Turn the move angle into a new movement direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Set the current speed based on the sprint input
            if (inputHandler.sprintInput)
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            //Move the character
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            //Stop sprinting if there is no movement input
            inputHandler.sprintInput = false;

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
        playerAnimator.SetSpeedPercent(currentSpeed, walkSpeed, sprintSpeed, inputHandler.sprintInput);
    }

    //
    public void HandleGravity()
    {
        bool result = Physics.Raycast(groundCheckPoint.position, Vector3.down, out RaycastHit hitInfo, minDistanceOff, groundMask);

        if (result == false)
        { 
            float fallSpeed = 2f;

            controller.Move(Vector3.down * fallSpeed * Time.deltaTime);
        }

    }

    #endregion
}