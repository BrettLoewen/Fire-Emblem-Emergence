////using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Interactable : MonoBehaviour
//{
//    //Variables to find the player so it can interact with this gameobject
//    public Transform interactPoint;
//    public float interactRadius = 1.5f;
//    public LayerMask playerMask;

//    protected virtual void Update()
//    {
//        if(interactPoint == null)
//        {
//            interactPoint = transform;
//        }

//        CheckForPlayer();
//    }

//    //A method meant to be overridden by classes that will inherit from interactable
//    public virtual void Interact(PlayerInteract player)
//    {
//        Debug.Log("Interacted with player");
//    }

//    //Find the player and add this interactable to its list of interactables if it is in range
//    protected void CheckForPlayer()
//    {
//        Collider[] colliders = Physics.OverlapSphere(interactPoint.position, interactRadius, playerMask);
//        foreach (Collider collider in colliders)
//        {
//            PlayerInteract player = collider.GetComponent<PlayerInteract>();
//            if (player != null)
//            {
//                player.AddInteractable(this);
//            }
//        }
//    }

//    //Visualize the player detection zone
//    protected void OnDrawGizmos()
//    {
//        if (interactPoint == null)
//        {
//            interactPoint = transform;
//        }

//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
//    }
//}
