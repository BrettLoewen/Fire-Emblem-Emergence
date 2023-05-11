using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Used to allow the player to interact with the exploration scene
/// </summary>
public class PlayerInteract: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager PlayerManager;   // Used to connect to the other player components
    private PlayerInputHandler inputHandler;                // Used to get the player's inputs

    private Interactable targetInteractable; // The nearest valid interactable to the player

    private ExplorationGameManager gameManager; // The manager of the exploration scene

    // The UI elements for the interaction popup
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionPromptText;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        // Ensure the interaction popup starts disabled
        interactionPrompt.SetActive(false);
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
        // Determine which (if any) interactable is the current target (but only if the scene is in explore mode)
        if(gameManager.ExplorationState == ExplorationState.Explore)
        {
            CalculateTargetInteractable();
        }

        // When the player presses the interact button
        if (inputHandler.InteractInput)
        {
            // Cancel the button press
            inputHandler.InteractInput = false;

            // If there is something to interact with and the scene is in explore mode
            if (targetInteractable != null && gameManager.ExplorationState == ExplorationState.Explore)
            {
                // Interact with the target interactable
                targetInteractable.Interact();
            }
        }

        // If there is a target interactable
        if(targetInteractable != null && gameManager.ExplorationState == ExplorationState.Explore)
        {
            // Enable the interaction popup
            interactionPrompt.SetActive(true);
            interactionPromptText.text = targetInteractable.GetInteractionText();
        }
        // If there is not a target interactable
        else
        {
            // Disable the interaction popup
            interactionPrompt.SetActive(false);
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Determine which, if any, interactable should be interacted with if the player pressed the interact button
    /// </summary>
    private void CalculateTargetInteractable()
    {
        // Declare the working variables
        Interactable _targetInteractable = null;
        float _shortestDistance = Mathf.Infinity;

        // Loop through every interactable in the scene
        foreach(Interactable interactable in Interactable.Interactables)
        {
            // Calculate the possibility of interacting with this interactable
            InteractionResult result = interactable.CanInteract(transform.position);

            // If the player is close enough to interact with it
            if(result.canInteract)
            {
                // If it is the closest interactable so far
                if(result.interactDistance < _shortestDistance)
                {
                    // Store it as the closest interactable so far
                    _shortestDistance = result.interactDistance;
                    _targetInteractable = interactable;
                }
            }
        }

        // Set the closest valid interactable to be the current target interactable
        targetInteractable = _targetInteractable;
    }//end CalculateTargetInteractable

    #endregion
}