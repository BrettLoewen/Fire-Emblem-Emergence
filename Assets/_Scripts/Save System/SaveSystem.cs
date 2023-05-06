using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Threading.Tasks;
using System;

public static class SaveSystem
{
    private const string SAVE_META_DATA_FILE_NAME = "meta";
    private const string SAVE_DATA_DIRECTORY_NAME = "SaveData";
    private static string DIRECTORY_PATH = $"{Application.persistentDataPath}/{SAVE_DATA_DIRECTORY_NAME}";
    private static string FILE_PATH = $"{Application.persistentDataPath}/{SAVE_DATA_DIRECTORY_NAME}/";
    public static SaveMetaData metaData { get; private set; } = null;
    public static SaveData currentSaveData { get; private set; } = null;

    #region Save

    public static async Task SaveData(int fileIndex)
    {
        await LoadMetaData();

        metaData.currentSaveFile = fileIndex;

        currentSaveData.savedAtTimestamp = DateTime.Now.ToString();

        string filePath = FILE_PATH + $"{metaData.saveFiles[fileIndex]}.txt";

        string jsonData = JsonUtility.ToJson(currentSaveData, true);
        await File.WriteAllTextAsync(filePath, jsonData);

        await SaveMetaData();
    }

    private static async Task SaveMetaData()
    {
        string metaPath = $"{FILE_PATH}{SAVE_META_DATA_FILE_NAME}.txt";

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

        await File.WriteAllTextAsync(metaPath, jsonData);
    }

    public static async Task SetCurrentSaveFile(int newCurrentSaveFile)
    {
        await LoadMetaData();

        metaData.currentSaveFile = newCurrentSaveFile;

        await SaveMetaData();
    }

    public static async Task MakeNewGameSaveFile(int fileIndex)
    {
        await LoadMetaData();

        metaData.currentSaveFile = fileIndex;

        currentSaveData = new SaveData();

        await SaveData(fileIndex);

        await SaveMetaData();
    }

#endregion

    #region Load

    private static async Task<SaveData> LoadSaveData(int fileIndex)
    {
        await LoadMetaData();

        string path = FILE_PATH + $"{metaData.saveFiles[fileIndex]}.txt";

        if (File.Exists(path) == false)
        {
            Debug.Log($"Save file \"save{fileIndex}\" does not exist!");
            return null;
        }

        string jsonData = await File.ReadAllTextAsync(path);

        SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
        return data;
    }

    private static async Task LoadMetaData()
    {
        // If the metadata was loaded previously, we don't need to go further
        if(metaData != null)
        {
            return;
        }

        // Get the path to the save meta data file
        string path = $"{FILE_PATH}{SAVE_META_DATA_FILE_NAME}.txt";

        // If the file does not exist, create a new file
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
    }

    public static async Task<SaveData[]> LoadSaveFiles()
    {
        // Ensure the metadata is loaded
        await LoadMetaData();

        SaveData[] saveFiles = new SaveData[metaData.saveFiles.Length];

        for (int i = 0; i < saveFiles.Length; i++)
        {
            saveFiles[i] = await LoadSaveData(i);
        }

        return saveFiles;
    }

    public static async Task LoadCurrentSaveFile()
    {
        currentSaveData = null;

        await LoadMetaData();

        currentSaveData = await LoadSaveData(metaData.currentSaveFile);
    }

    public static async Task<bool> HasCurrentSaveFile()
    {
        await LoadCurrentSaveFile();

        return currentSaveData != null;
    }

#endregion

    #region Update Save Data

    public static void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        currentSaveData.playerPosition = position;
        currentSaveData.playerRotation = rotation;
    }

    public static void SetPlayerCameraValues(float xValue, float yValue)
    {
        currentSaveData.playerCameraValues = new Vector2(xValue, yValue);
    }

    public static void SetPlayerCustomization(string customizationObjectName)
    {
        currentSaveData.playerCustomization = customizationObjectName;
    }

    #endregion
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector2 playerCameraValues;
    public string playerCustomization;
    public string savedAtTimestamp;
    public string activity;

    public SaveData()
    {
        playerPosition = new Vector3(-3.5f, 0.75f, -1.5f);
        playerRotation = new Quaternion(0f, 0.9f, 0f, 0.4f);
        playerCameraValues = new Vector2(66f, 0.35f);
        playerCustomization = "Knight";
        savedAtTimestamp = DateTime.Now.ToString();
        activity = "New Game";
    }

    public override string ToString()
    {
        return $"Position: {playerPosition}\nRotation: {playerRotation}\nCamera: {playerPosition}\nCustomization: {playerCustomization}\nActivity: {activity}\nTimestamp: {savedAtTimestamp}";
    }
}

[System.Serializable]
public class SaveMetaData
{
    public int currentSaveFile;
    public string[] saveFiles;

    public SaveMetaData()
    {
        currentSaveFile = 0;
        saveFiles = new string[3] { "save1", "save2", "save3" };
    }

    public override string ToString()
    {
        string output = $"Current: {currentSaveFile}\nSave Files: {{";
        foreach (string s in saveFiles) 
        {
            output += $"{s}, ";
        }
        output += "}";
        return output;
    }
}