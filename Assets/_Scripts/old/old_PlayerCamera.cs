//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

//public class PlayerCamera : MonoBehaviour
//{
//    [HideInInspector] public PlayerManager playerManager;
//    private PlayerInputHandler inputHandler;
//    //private PlayerMovement playerMovement;

//    //Variables for cameras
//    public CinemachineFreeLook thirdPersonCamera;
//    public CinemachineVirtualCamera overheadCamera;
//    public Camera mainCamera;

//    //Variables for storing look targets
//    public Transform playerLookTarget;
//    private Transform currentLookTarget;

//    private bool isTargetLocking;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //Get the necessary references
//        inputHandler = playerManager.inputHandler;
//        //playerMovement = playerManager.playerMovement;

//        //Set the camera to look at the player
//        currentLookTarget = playerLookTarget;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //Set the look target of the camera
//        thirdPersonCamera.LookAt = currentLookTarget;
//        overheadCamera.LookAt = currentLookTarget;

//        //Move the camera based on look input from the input handler
//        thirdPersonCamera.m_XAxis.m_InputAxisValue = inputHandler.lookInput.x;// * currentSensitivity;
//        thirdPersonCamera.m_YAxis.m_InputAxisValue = inputHandler.lookInput.y;// * currentSensitivity;
//    }

//    public void SetTargetLockStatus(bool isTargetLocking, Transform targetLockLookTransform)
//    {
//        //If the player is not currently locked on a target
//        if (!isTargetLocking)
//        {
//            //Look at the player
//            currentLookTarget = playerLookTarget;

//            //Make the third person camera the active camera
//            thirdPersonCamera.m_Priority = 20;
//            overheadCamera.m_Priority = 10;
//        }
//        //If the player is currently locked on a target
//        else
//        {
//            //Look at the target's look point
//            currentLookTarget = targetLockLookTransform;

//            //Make the overhead camera the active camera
//            thirdPersonCamera.m_Priority = 10;
//            overheadCamera.m_Priority = 20;
//        }

//        //If the player has stopped target locking
//        if(this.isTargetLocking && !isTargetLocking)
//        {
//            thirdPersonCamera.ForceCameraPosition(overheadCamera.transform.position, overheadCamera.transform.rotation);
//        }

//        //Update the target locking variable
//        this.isTargetLocking = isTargetLocking;
//    }
//}
