using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager playerManager;
    private PlayerInputHandler inputHandler;

    private Interactable targetInteractable;

    private ExplorationGameManager gameManager;

    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionPromptText;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        interactionPrompt.SetActive(false);
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = playerManager.inputHandler;
        gameManager = ExplorationGameManager.Instance;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        if(gameManager.explorationState == ExplorationState.Explore)
        {
            CalculateTargetInteractable();
        }

        if (inputHandler.interactInput)
        {
            inputHandler.interactInput = false;

            if (targetInteractable != null && gameManager.explorationState == ExplorationState.Explore)
            {
                targetInteractable.Interact();
            }
        }

        if(targetInteractable != null)
        {
            interactionPrompt.SetActive(true);
            interactionPromptText.text = targetInteractable.GetInteractionText();
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }//end Update

    #endregion //end Unity Control Methods

    #region

    private void CalculateTargetInteractable()
    {
        Interactable m_targetInteractable = null;
        float m_shortestDistance = Mathf.Infinity;

        foreach(Interactable interactable in Interactable.Interactables)
        {
            InteractionResult result = interactable.CanInteract(transform.position);

            if(result.canInteract)
            {
                if(result.interactDistance < m_shortestDistance)
                {
                    m_shortestDistance = result.interactDistance;
                    m_targetInteractable = interactable;
                }
            }
        }

        targetInteractable = m_targetInteractable;
    }

    #endregion
}