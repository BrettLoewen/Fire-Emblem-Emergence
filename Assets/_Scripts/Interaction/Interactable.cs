using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    #region Variables

    public static List<Interactable> Interactables { get; private set; } = new List<Interactable>();

    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected virtual void Awake()
    {
        Interactables.Add(this);
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    protected virtual void OnDestroy()
    {
        Interactables.Remove(this);
    }

    #endregion //end Unity Control Methods

    #region Interaction

    public virtual void Interact()
    {
        Debug.Log("Placeholder Interaction. Please override the 'Interact' method");
    }

    public InteractionResult CanInteract(Vector3 playerPosition)
    {
        float distance = Helpers.FlatDistance(playerPosition, interactionPoint.position);

        return new InteractionResult(distance <= interactionRadius, distance);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if(interactionPoint != null && interactionRadius > 0f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
        }
    }
}

public struct InteractionResult
{
    public bool canInteract;
    public float interactDistance;

    public InteractionResult(bool inRadius, float distance)
    {
        canInteract = inRadius;
        interactDistance = distance;
    }
}