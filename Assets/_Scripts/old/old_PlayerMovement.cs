//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    [HideInInspector] public PlayerManager playerManager;
//    private PlayerInputHandler inputHandler;
//    private PlayerCamera playerCamera;
//    private PlayerAnimator playerAnimator;

//    //Variables to control the movement speed
//    public float walkSpeed = 3f;
//    public float sprintSpeed = 6f;
//    public float currentSpeed;

//    //Variables to control the turn speed
//    public float turnSmoothTime = 0.1f;
//    private float turnSmoothVelocity;

//    //Variables to control strafing
//    public bool isStrafing;
//    private Transform lockedTarget;

//    //Variables to control jumping
//    public float gravity = -10f;
//    public float jumpHeight = 3;
//    private Vector3 velocity;

//    //Variables to check if the player is on the ground
//    private bool isGrounded;
//    public Transform groundCheckPoint;
//    public float groundCheckRadius;
//    public LayerMask groundMask;

//    private CharacterController controller;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //Get the various references needed to operate
//        inputHandler = playerManager.inputHandler;
//        playerCamera = playerManager.playerCamera;
//        playerAnimator = playerManager.playerAnimator;
//        controller = GetComponent<CharacterController>();

//        currentSpeed = walkSpeed;
//        isStrafing = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CheckGrounded();
//        HandleJumping();
//        HandleMovement();
//    }

//    //Handles movement. Approach differs based on whether the player is strafing or not
//    private void HandleMovement()
//    {
//        //Get the direction of movement
//        Vector2 moveInput = inputHandler.moveInput;
//        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

//        //If the player is strafing, strafe
//        if (isStrafing)
//        {
//            HandleStrafing(direction);
//        }
//        //If the player is not strafing, move normally
//        else
//        {
//            HandleMoving(direction);
//        }

//        //If the character should be moving
//        if(currentSpeed > 0f)
//        {
//            //Set the current speed based on the actual velocity of the character controller
//            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
//        }

//        //Animate the character according to the current speed
//        playerAnimator.SetSpeedPercent(currentSpeed, walkSpeed, sprintSpeed, inputHandler.sprintInput);
//    }

//    //Handles movement while strafing
//    private void HandleStrafing(Vector3 direction)
//    {
//        //Get and smooth the angle for the movement direction relative to the camera
//        //Vector3 targetDirection = lockedTarget.position - 
//        float targetAngle = playerCamera.mainCamera.transform.eulerAngles.y;
//        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

//        //Rotate the player to face the locked target
//        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
//        //transform.LookAt(new Vector3(lockedTarget.position.x, transform.position.y, lockedTarget.position.z));

//        //Move the player relative to the locked target
//        controller.Move(transform.rotation * direction.normalized * walkSpeed * Time.deltaTime);
//    }

//    //Handles the basic movement
//    private void HandleMoving(Vector3 direction)
//    {
//        //If there is movement input
//        if(direction.magnitude >= 0.1f)
//        {
//            //Get and smooth the angle for the movement direction relative to the camera
//            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.mainCamera.transform.eulerAngles.y;
//            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

//            //Rotate the player to the angle
//            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

//            //Turn the move angle into a new movement direction
//            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

//            //Set the current speed based on the sprint input
//            if (inputHandler.sprintInput)
//            {
//                currentSpeed = sprintSpeed;
//            }
//            else
//            {
//                currentSpeed = walkSpeed;
//            }

//            //Move the character
//            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
//        }
//        else
//        {
//            //Stop sprinting if there is no movement input
//            inputHandler.sprintInput = false;

//            //Set the current speed to 0 if there is no movement input
//            currentSpeed = 0f;
//        }
//    }

//    private void CheckGrounded()
//    {
//        //Check and store if the player is on the ground
//        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundMask);
//    }

//    private void HandleJumping()
//    {
//        //If the player is near the ground, force them into the ground
//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f;
//        }

//        //If the player wants to jump and is on the ground
//        if (inputHandler.jumpInput && isGrounded)
//        {
//            //Trigger the jump
//            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
//        }

//        //Stop the jump input
//        inputHandler.jumpInput = false;

//        //Apply gravity
//        velocity.y += gravity * Time.deltaTime;

//        //Move the character according to the jump velocity
//        controller.Move(velocity * Time.deltaTime);
//    }

//    public void SetTargetLockStatus(bool isTargetLocking, Transform target)
//    {
//        lockedTarget = target;

//        if(!isStrafing && isTargetLocking)
//        {
//            StartStrafing();
//        }

//        if(isStrafing && !isTargetLocking)
//        {
//            StopStrafing();
//        }
//    }

//    private void StartStrafing()
//    {
//        isStrafing = true;
//    }

//    private void StopStrafing()
//    {
//        isStrafing = false;
//    }
//}
