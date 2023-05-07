using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class which allows the player to interact with it
/// </summary>
public class Interactable: MonoBehaviour
{
    #region Variables

    // A static list to hold all interactables in the scene so they can be easily accessed
    public static List<Interactable> Interactables { get; private set; } = new List<Interactable>();

    [SerializeField] private Transform interactionPoint;    // The point to perform the distance check from
    [SerializeField] private float interactionRadius;       // The maximum distance away the player can be while still interacting with this object

    [SerializeField] private string interactionText; // What to display in the interaction popup

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected virtual void Awake()
    {
        // Add this interactable to the static list when it spawns
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

    // OnDestroy is called when the gameobject is destroyed
    protected virtual void OnDestroy()
    {
        // Remove this interactables from the static list when it is destroyed
        Interactables.Remove(this);
    }//end OnDestroy

    #endregion //end Unity Control Methods

    #region Interaction

    /// <summary>
    /// Placeholder virtual method called by PlayerInteract
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Placeholder Interaction. Please override the 'Interact' method");
    }//end Interact

    /// <summary>
    /// Calculate the possibility of interaction between this object and the player and return the result
    /// </summary>
    /// <param name="_playerPosition">The current position of the player's feet</param>
    /// <returns></returns>
    public InteractionResult CanInteract(Vector3 _playerPosition)
    {
        // Calculate the flat distance between the interaction point and the player
        float distance = Helpers.FlatDistance(_playerPosition, interactionPoint.position);

        // Create a result object using the distance and return it
        return new InteractionResult(distance <= interactionRadius, distance);
    }//end CanInteract

    /// <summary>
    /// Return the object's interaction text
    /// </summary>
    /// <returns></returns>
    public string GetInteractionText()
    {
        return interactionText;
    }//end GetInteractionText

    #endregion

    /// <summary>
    /// Visualize the interaction area for this interactable in the engine
    /// </summary>
    private void OnDrawGizmos()
    {
        // Only do so if there is valid information to work with
        if(interactionPoint != null && interactionRadius > 0f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
        }
    }//end OnDrawGizmos
}

/// <summary>
/// Represents the possibility of the player interacting with a given interactable
/// </summary>
public struct InteractionResult
{
    public bool canInteract;        // Whether or not the player is close enough to the interactable
    public float interactDistance;  // The distance between the player and the interactable

    public InteractionResult(bool inRadius, float distance)
    {
        canInteract = inRadius;
        interactDistance = distance;
    }//end constructor
}//end InteractionResult