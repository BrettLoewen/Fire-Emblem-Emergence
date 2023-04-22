using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerInputHandler inputHandler;
    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    //public PlayerCombat playerCombat;
    public UnitAnimator playerAnimator;
    //public PlayerInteract playerInteract;
    //public PlayerEquipment playerEquipment;
    //public PlayerStats playerStats;
    //public PlayerHUD playerHUD;

    private void Awake()
    {
        //Create a singleton reference for the player manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one PlayerManager in the scene!");
        }

        //Tell the other player scripts how to access this manager script
        inputHandler.playerManager = this;
        playerMovement.playerManager = this;
        playerCamera.playerManager = this;
        //playerAnimator.playerManager = this;
        //playerCombat.playerManager = this;
        //playerInteract.playerManager = this;
        //playerEquipment.playerManager = this;
        //playerStats.playerManager = this;
        //playerHUD.playerManager = this;
    }
}
