using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame(SaveData Data)
    {
        string Json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(SavePath, Json);
        Debug.Log("Game Saved to: " + SavePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string Json = File.ReadAllText(SavePath);
            SaveData Data = JsonUtility.FromJson<SaveData>(Json);
            return Data;
        }

        return null;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public static bool IsSaveDataExists()
    {
        if (File.Exists(SavePath))
            return true;

        return false;
    }
}
