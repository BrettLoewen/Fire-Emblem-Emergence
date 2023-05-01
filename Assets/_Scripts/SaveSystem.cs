using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Threading.Tasks;
using System;

public static class SaveSystem
{
    private static string FILE_PATH = Application.dataPath + "/Resources/Save Data/";

    public static async Task SaveData()
    {
        await SaveJSON();

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public static async Task LoadData()
    {
        await LoadJSON();
    }

    private static async Task SaveJSON()
    {
        SaveData data = new SaveData(new Vector3(0, -1, 367.5f), "Exploration");
        Debug.Log(data);
        string jsonData = JsonUtility.ToJson(data);
        await File.WriteAllTextAsync(FILE_PATH + "data_test.txt", jsonData);
    }

    private static async Task LoadJSON()
    {
        string jsonData = await File.ReadAllTextAsync(FILE_PATH + "data_test.txt");
        SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
        Debug.Log($"Loaded Data: {data}");
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