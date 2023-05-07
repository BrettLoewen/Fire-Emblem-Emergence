using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Used to control the player's camera
/// </summary>
public class PlayerCamera: MonoBehaviour
{
    #region Variables

    // References to other player components
    [HideInInspector] public PlayerManager PlayerManager;
    private PlayerInputHandler inputHandler;

    // Variables for cameras
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [field: SerializeField] public Camera MainCamera { get; private set; }

    private ExplorationGameManager gameManager; // The manager of the exploration scene

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        // Get the necessary references
        inputHandler = PlayerManager.InputHandler;
        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Only move the camera if the game is in exploration mode
        if(gameManager.ExplorationState == ExplorationState.Explore)
        {
            // Move the camera based on look input from the input handler
            thirdPersonCamera.m_XAxis.m_InputAxisValue = inputHandler.LookInput.x;// * currentSensitivity;
            thirdPersonCamera.m_YAxis.m_InputAxisValue = inputHandler.LookInput.y;// * currentSensitivity;

            // Store the player's current position so it can be saved
            SaveSystem.SetPlayerCameraValues(thirdPersonCamera.m_XAxis.Value, thirdPersonCamera.m_YAxis.Value);
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Setup the player camera accoridng to passed data from a save file
    /// </summary>
    /// <param name="_cameraValues">The aim values for the camera</param>
    /// <param name="_playerPosition">The position of the player</param>
    public void Setup(Vector2 _cameraValues, Vector3 _playerPosition)
    {
        // Update the camera's aim values
        thirdPersonCamera.m_XAxis.Value = _cameraValues.x;
        thirdPersonCamera.m_YAxis.Value = _cameraValues.y;

        // Move the camera to be where the player is
        transform.position = _playerPosition;
    }//end Setup

    #endregion
}