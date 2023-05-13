using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;

/// <summary>
/// Used to manage saving and loading data to/from files
/// </summary>
public static class SaveSystem
{
    // Strings to hold the file paths needed
    private const string SAVE_META_DATA_FILE_NAME = "meta";
    private const string SAVE_DATA_DIRECTORY_NAME = "SaveData";
    private static string DIRECTORY_PATH = $"{Application.persistentDataPath}/{SAVE_DATA_DIRECTORY_NAME}";
    private static string FILE_PATH = $"{Application.persistentDataPath}/{SAVE_DATA_DIRECTORY_NAME}/";

    private const char INVENTORY_ITEM_SEPARATOR = ',';

    public static SaveMetaData metaData { get; private set; } = null;       // The current save meta data object
    public static SaveData currentSaveData { get; private set; } = null;    // The current save data object

    #region Save

    /// <summary>
    /// Used to save the current save data object to a file
    /// </summary>
    /// <param name="fileIndex">The index of the file that the save data object should be saved to</param>
    /// <returns></returns>
    public static async Task SaveData(int fileIndex)
    {
        // Ensure the meta data is loaded
        await LoadMetaData();

        // Update the meta data's current file index incase they differ
        metaData.currentSaveFile = fileIndex;

        // Update the save data's timestamp
        currentSaveData.savedAtTimestamp = DateTime.Now.ToString();

        // Determine the file path of the save file
        string filePath = $"{FILE_PATH}{metaData.saveFiles[fileIndex]}.txt";

        // Convert the current save data object to a json string
        string jsonData = JsonUtility.ToJson(currentSaveData, true);

        // Write the json string to the file
        await File.WriteAllTextAsync(filePath, jsonData);

        // Save the meta data file incase it changed
        await SaveMetaData();
    }//end SaveData

    /// <summary>
    /// Save the meta data object to a file
    /// </summary>
    /// <returns></returns>
    private static async Task SaveMetaData()
    {
        // Determine the file path of the meta file
        string metaPath = $"{FILE_PATH}{SAVE_META_DATA_FILE_NAME}.txt";

        // Convert the meta data object to a json string
        string jsonData = JsonUtility.ToJson(metaData, true);

        // If the save meta file does not exist, a new one will be created by the write operation
        if(File.Exists(metaPath) == false)
        {
            // However, if the directory also does not exist, it needs to be created manually
            if(Directory.Exists(DIRECTORY_PATH) == false)
            {
                Directory.CreateDirectory(DIRECTORY_PATH);
            }
        }

        // Write the meta data json string to the file
        await File.WriteAllTextAsync(metaPath, jsonData);
    }//end SaveMetaData

    /// <summary>
    /// Update which save file is currently being used
    /// </summary>
    /// <param name="newCurrentSaveFile">The file index of the new current save file</param>
    /// <returns></returns>
    public static async Task SetCurrentSaveFile(int newCurrentSaveFile)
    {
        // Ensure the meta data is loaded
        await LoadMetaData();

        // Update which save file is currently being used
        metaData.currentSaveFile = newCurrentSaveFile;

        // Save the change to the meta data
        await SaveMetaData();
    }//end SetCurrentSaveFile

    /// <summary>
    /// Used to write a new game save file
    /// </summary>
    /// <param name="fileIndex">The index of the file the new game data will be written to</param>
    /// <returns></returns>
    public static async Task MakeNewGameSaveFile(int fileIndex)
    {
        // Ensure the meta data is loaded
        await LoadMetaData();

        // Update which save file is currently being used
        metaData.currentSaveFile = fileIndex;

        // Update the current save data to be a new game save file
        currentSaveData = new SaveData();

        // Save the new file data
        await SaveData(fileIndex);

        // Save the change to the meta data
        await SaveMetaData();
    }//end MakeNewGameSaveFile

    #endregion

    #region Load

    /// <summary>
    /// Load the passed save file and return its data
    /// </summary>
    /// <param name="fileIndex">The file index of the file to read from</param>
    /// <returns></returns>
    private static async Task<SaveData> LoadSaveData(int fileIndex)
    {
        // Ensure the meta data is loaded
        await LoadMetaData();

        // Determine the file path of the passed file index
        string path = $"{FILE_PATH}{metaData.saveFiles[fileIndex]}.txt";

        // If the file does not exist, return null
        if (File.Exists(path) == false)
        {
            Debug.Log($"Save file \"save{fileIndex}\" does not exist!");
            return null;
        }

        // Get the file's contents as a json string
        string jsonData = await File.ReadAllTextAsync(path);

        // Convert the json string into a save data object
        SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

        // Return the save data object
        return data;
    }//end LoadSaveData

    /// <summary>
    /// Load the meta data file
    /// </summary>
    /// <returns></returns>
    private static async Task LoadMetaData()
    {
        // If the metadata was loaded previously, we don't need to go further
        if(metaData != null)
        {
            return;
        }

        // Get the path to the save meta data file
        string path = $"{FILE_PATH}{SAVE_META_DATA_FILE_NAME}.txt";

        // If the file does not exist, create a new meta data object and save it to a file
        if (File.Exists(path) == false)
        {
            Debug.Log($"Save meta data file does not exist! Creating new file");
            metaData = new SaveMetaData();
            await SaveMetaData();
        }

        // If the file does exist
        // Get the data from the file as a json string
        string jsonData = await File.ReadAllTextAsync(path);

        // Convert the json string into a SaveMetaData object
        metaData = JsonUtility.FromJson<SaveMetaData>(jsonData);
    }//end LoadMetaData

    /// <summary>
    /// Load every save file and return them
    /// </summary>
    /// <returns></returns>
    public static async Task<SaveData[]> LoadSaveFiles()
    {
        // Ensure the metadata is loaded
        await LoadMetaData();

        // Create an array to hold the save data objects
        SaveData[] saveFiles = new SaveData[metaData.saveFiles.Length];

        // For each save file
        for (int i = 0; i < saveFiles.Length; i++)
        {
            // Load the save file's data and store it in the array
            saveFiles[i] = await LoadSaveData(i);
        }

        // Return the array of save data objects
        return saveFiles;
    }//end LoadSaveFiles

    /// <summary>
    /// Load the current save file (defined in the save meta data)
    /// </summary>
    /// <returns></returns>
    public static async Task LoadCurrentSaveFile()
    {
        // Set the current save data to null (so the game knows it is being loaded
        currentSaveData = null;

        // Ensure the meta data is loaded and up to date
        await LoadMetaData();

        // Load the save data object that the meta data says is the current one
        currentSaveData = await LoadSaveData(metaData.currentSaveFile);
    }//end LoadCurrentSaveFile

    /// <summary>
    /// Checks if the meta data's current save file exists
    /// </summary>
    /// <returns>True if the current save file does exist, and false if it does not</returns>
    public static async Task<bool> HasCurrentSaveFile()
    {
        // Try to load the current save file
        // If one does not exist, this will cause `currentSaveData` to become null
        await LoadCurrentSaveFile();

        // Return whether or not the current save data is null (whether or not it existed)
        return currentSaveData != null;
    }//end HasCurrentSaveFile

    #endregion

    #region Update Save Data

    /// <summary>
    /// Used to update the current save data object's information about the player's position and rotation
    /// </summary>
    /// <param name="_position">The player's position in the exploration scene</param>
    /// <param name="_rotation">The player's rotation in the exploration scene</param>
    public static void SetPlayerPositionAndRotation(Vector3 _position, Quaternion _rotation)
    {
        // Update the save data object's information about the player
        currentSaveData.playerPosition = _position;
        currentSaveData.playerRotation = _rotation;
    }//end SetPlayerPositionAndRotation

    /// <summary>
    /// Used to update the current save data object's information about the player's camera values
    /// </summary>
    /// <param name="_xValue">The player's camera x aim value in the exploration scene</param>
    /// <param name="_yValue">The player's camera y aim value in the exploration scene</param>
    public static void SetPlayerCameraValues(float _xValue, float _yValue)
    {
        // Update the save data object's information about the player
        currentSaveData.playerCameraValues = new Vector2(_xValue, _yValue);
    }//end SetPlayerCameraValues

    /// <summary>
    /// Used to update the current save data object's information about the player's customization settings
    /// </summary>
    /// <param name="_customizationObjectName">The name of the player's customization object</param>
    public static void SetPlayerCustomization(string _customizationObjectName)
    {
        // Update the save data object's information about the player
        currentSaveData.playerCustomization = _customizationObjectName;
    }//end SetPlayerCustomization

    /// <summary>
    /// Convert the player's inventory into a string representation and update the save data with it and the player's money
    /// </summary>
    /// <param name="_inventory"></param>
    /// <param name="_money"></param>
    public static void SetPlayerInventoryData(List<Item> _inventory, int _money)
    {
        // Start with an empty string
        string _inventoryAsString = "";

        // Add each item's string representation to the string
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventoryAsString += _inventory[i].ToString();

            if (i < _inventory.Count - 1)
            {
                _inventoryAsString += INVENTORY_ITEM_SEPARATOR;
            }
        }

        // Update the save data
        currentSaveData.playerInventory = _inventoryAsString;
        currentSaveData.playerMoney = _money;
    }//end SetPlayerInventoryData

    /// <summary>
    /// Convert the string representation of the player's inventory used for saving to a list of item objects
    /// </summary>
    /// <returns></returns>
    public static List<Item> GetPlayerInventory()
    {
        // Create a list to add items to and return
        List<Item> _inventory = new List<Item>();

        // Get each string that represents an item (the ToString value of Item)
        string[] _itemsAsString = currentSaveData.playerInventory.Split(INVENTORY_ITEM_SEPARATOR);
        
        // Get all of the item data objects in the game
        List<ItemData> _itemDatas = DataManager.GetItems();

        // Create a dictionary to map item data objects to their name
        Dictionary<string, ItemData> _itemDataStringMap = new Dictionary<string, ItemData>();

        // Generate the contents of the item data object - name map dictionary
        foreach (ItemData data in _itemDatas)
        {
            _itemDataStringMap.Add(data.Name, data);
        }

        // Use the dictionary generated above to convert each item string into an item and add it to the inventory
        foreach (string item in _itemsAsString)
        {
            _inventory.Add(new Item(_itemDataStringMap[item]));
        }

        // Return the generated list of items
        return _inventory;
    }//end GetPlayerInventory

    #endregion
}//end SaveSystem

/// <summary>
/// Used to store and save data about the player's game
/// </summary>
[Serializable]
public class SaveData
{
    public Vector3 playerPosition;      // Stores the position of the player in the Exploration scene
    public Quaternion playerRotation;   // Stores the rotation of the player in the Exploration scene
    public Vector2 playerCameraValues;  // Stores the rotation values of the player's camera in the Exploration scene
    public string playerCustomization;  // Stores the name of the player's customization object
    public string savedAtTimestamp;     // Stores the timestamp at which this save data was last updated
    public string activity;             // Stores the activity that the player was doing at the time of saving

    public string playerInventory;
    public int playerMoney;

    /// <summary>
    /// Used to create a new save data object.
    /// Defines default values for the save data object
    /// </summary>
    public SaveData()
    {
        playerPosition = new Vector3(-3.5f, 0.75f, -1.5f);
        playerRotation = new Quaternion(0f, 0.9f, 0f, 0.4f);
        playerCameraValues = new Vector2(66f, 0.35f);
        playerCustomization = "Knight";
        savedAtTimestamp = DateTime.Now.ToString();
        activity = "New Game";
        playerInventory = "";
        playerMoney = 5000;
    }//end constructor

    /// <summary>
    /// Converts the save data to a formatted string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Position: {playerPosition}\n" +
            $"Rotation: {playerRotation}\n" +
            $"Camera: {playerPosition}\n" +
            $"Customization: {playerCustomization}\n" +
            $"Activity: {activity}\n" +
            $"Timestamp: {savedAtTimestamp}\n" +
            $"Inventory: {playerInventory}\n" +
            $"Money: {playerMoney}";
    }//end ToString
}//end SaveData

/// <summary>
/// Used to store and save data about the player's save files
/// </summary>
[Serializable]
public class SaveMetaData
{
    public int currentSaveFile; // The index of the last loaded/saved save file
    public string[] saveFiles;  // An array of strings holding the name of every save file

    /// <summary>
    /// Used to create a new meta data object if needed
    /// </summary>
    public SaveMetaData()
    {
        currentSaveFile = 0;
        saveFiles = new string[3] { "save1", "save2", "save3" };
    }//end constructor

    /// <summary>
    /// Covnerts the save meta data object to a formatted string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string output = $"Current: {currentSaveFile}\nSave Files: {{";
        foreach (string s in saveFiles) 
        {
            output += $"{s}, ";
        }
        output += "}";
        return output;
    }//end ToString
}//end SaveData