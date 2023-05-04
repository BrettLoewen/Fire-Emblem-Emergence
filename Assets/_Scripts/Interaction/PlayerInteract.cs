using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract: MonoBehaviour
{
    #region Variables

    [HideInInspector] public PlayerManager playerManager;
    private PlayerInputHandler inputHandler;

    private Interactable targetInteractable;

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