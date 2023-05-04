using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerManager : Singleton<PlayerManager>
{
    public static PlayerManager instance;

    public PlayerInputHandler inputHandler;
    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    public UnitAnimator playerAnimator;
    public UnitCustomizer playerCustomizer;
    public PlayerInteract playerInteract;

    protected override void Awake()
    {
        base.Awake();

        //Tell the other player scripts how to access this manager script
        inputHandler.playerManager = this;
        playerMovement.playerManager = this;
        playerCamera.playerManager = this;
        playerInteract.playerManager = this;
    }

    public async Task Setup()
    {
        while(SaveSystem.currentSaveData == null)
        {
            await Task.Yield();
        }

        SaveData saveData = SaveSystem.currentSaveData;

        transform.position = saveData.playerPosition;
        transform.rotation = saveData.playerRotation;

        playerCamera.Setup(saveData.playerCameraValues, saveData.playerPosition);

        await Task.Delay(100);
    }

    public void SetCustomization(Customization customization)
    {
        playerCustomizer.SetCustomization(customization);
    }
}
