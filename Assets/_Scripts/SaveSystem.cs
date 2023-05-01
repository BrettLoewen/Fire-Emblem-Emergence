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

    private static string FILE_PATH = Application.dataPath + "/Resources/Save Data/";
    public static SaveMetaData metaData { get; private set; } = null;// new SaveMetaData() { currentSaveFile = 0, saveFiles = new string[3] { "save1", "save2", "save3" } };
    public static SaveData currentSaveData { get; private set; } = null;  //new SaveData(Vector3.zero, "Exploration");

    public static async Task SaveData(string fileName)
    {
        if (metaData == null)
        {
            metaData = await LoadData(SAVE_META_DATA_FILE_NAME, true) as SaveMetaData;
        }

        string filePath = FILE_PATH + $"{fileName}.txt";
        string metaPath = FILE_PATH + $"{SAVE_META_DATA_FILE_NAME}.txt";

        string jsonData = JsonUtility.ToJson(currentSaveData, true);
        await File.WriteAllTextAsync(filePath, jsonData);
        
        jsonData = JsonUtility.ToJson(metaData, true);
        await File.WriteAllTextAsync(metaPath, jsonData);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private static async Task<object> LoadData(string fileName, bool isMetaData = false)
    {
        string path = FILE_PATH + $"{fileName}.txt";

        if (File.Exists(path) == false)
        {
            Debug.Log($"Save file \"{fileName}\" does not exist!");
            return null;
        }

        string jsonData = await File.ReadAllTextAsync(path);

        if(isMetaData)
        {
            SaveMetaData data = JsonUtility.FromJson<SaveMetaData>(jsonData);
            return data;
        }
        else
        {
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            return data;

        }
    }

    public static async Task<SaveData[]> LoadSaveFiles()
    {
        if(metaData == null)
        {
            metaData = await LoadData(SAVE_META_DATA_FILE_NAME, true) as SaveMetaData;
        }

        SaveData[] saveFiles = new SaveData[metaData.saveFiles.Length];

        for (int i = 0; i < saveFiles.Length; i++)
        {
            saveFiles[i] = await LoadData(metaData.saveFiles[i]) as SaveData;
        }

        return saveFiles;
    }

    public static async Task LoadCurrentSaveFile()
    {
        if (metaData == null)
        {
            metaData = await LoadData(SAVE_META_DATA_FILE_NAME, true) as SaveMetaData;
        }

        currentSaveData = await LoadData(metaData.saveFiles[metaData.currentSaveFile]) as SaveData;
        Debug.Log($"Current Save Data: {currentSaveData}");
    }

    public static void SetPlayerPosition(Vector3 position)
    {
        currentSaveData.playerPosition = position;
    }
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string savedAtTimestamp;
    public string activity;
    
    public SaveData(Vector3 position, string _activity)
    {
        playerPosition = position;
        savedAtTimestamp = DateTime.Now.ToString();
        activity = _activity;
    }

    public override string ToString()
    {
        return $"Position: {playerPosition}\nActivity: {activity}\nTimestamp: {savedAtTimestamp}";
    }
}

[System.Serializable]
public class SaveMetaData
{
    public int currentSaveFile;
    public string[] saveFiles;

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