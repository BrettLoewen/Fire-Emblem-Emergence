using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager playerManager;
    private PlayerInputHandler inputHandler;

    // Variables for cameras
    public CinemachineFreeLook thirdPersonCamera;
    public Camera mainCamera;

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
        // Get the necessary references
        inputHandler = playerManager.inputHandler;
        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Only move the camera if the game is in exploration mode
        if(gameManager.explorationState == ExplorationState.Explore)
        {
            // Move the camera based on look input from the input handler
            thirdPersonCamera.m_XAxis.m_InputAxisValue = inputHandler.lookInput.x;// * currentSensitivity;
            thirdPersonCamera.m_YAxis.m_InputAxisValue = inputHandler.lookInput.y;// * currentSensitivity;
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region



    #endregion
}