using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// Used to manage the player and provide a connection point for its various components
/// 
/// Note: this class has commented out calls to the SaveSystem class. These calls have been commented out
/// to remove persistent saving from the game as I figured it was more trouble than it was worth. I have
/// left the code in the project in case my future self wants to further develop this project or use the
/// code for a different project
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    [field: SerializeField] public PlayerInputHandler InputHandler { get; private set; } // Receives and makes available player input
    [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }   // Uses input to move the player character
    [field: SerializeField] public PlayerCamera PlayerCamera { get; private set; }       // Uses input to control the player's camera
    [field: SerializeField] public UnitAnimator PlayerAnimator { get; private set; }     // Used to make the player's model play animations
    [field: SerializeField] public UnitCustomizer PlayerCustomizer { get; private set; } // Used to make the player's model display correctly
    [field: SerializeField] public PlayerInteract PlayerInteract { get; private set; }   // Uses input to allow the player to interact with interactables

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        // Setup the Singleton found in the base class
        base.Awake();

        // Tell the other player scripts how to access this manager script
        PlayerMovement.PlayerManager = this;
        PlayerCamera.PlayerManager = this;
        PlayerInteract.PlayerManager = this;
    }//end Awake

    /// <summary>
    /// Initialize the player according to the current save file
    /// </summary>
    /// <returns></returns>
    public async Task Setup()
    {
        // Wait until the save file has been loaded before using it to initialize the player
        //while(SaveSystem.currentSaveData == null)
        //{
        //    await Task.Yield();
        //}

        // Get the current save file
        //SaveData _saveData = SaveSystem.currentSaveData;

        //Debug.Log($"Player save data: \n{_saveData}");

        // Place the player at the correct position and rotation
        //transform.position = _saveData.playerPosition;
        //transform.rotation = _saveData.playerRotation;

        //// Make the camera look in the right direction
        //PlayerCamera.Setup(_saveData.playerCameraValues, _saveData.playerPosition);

        //// If the save file has customization info, tell the unit customizer to use that customization info
        //if(_saveData.playerCustomization != null && _saveData.playerCustomization != "")
        //{
        //    PlayerCustomizer.SetCustomization(DataManager.GetCustomizationFromString(_saveData.playerCustomization));
        //}

        // Wait some time to guarantee the position and camera setting has time to take effect
        await Task.Delay(100);
    }//end Setup

    /// <summary>
    /// Update the customization object that the player is using
    /// </summary>
    /// <param name="_customization"></param>
    public void SetCustomization(Customization _customization)
    {
        // Tell the customizer to use the passed customization object
        PlayerCustomizer.SetCustomization(_customization);

        // Update the save data to use the passed customization object
        //SaveSystem.SetPlayerCustomization(_customization.name);
    }//end SetCustomization
}
