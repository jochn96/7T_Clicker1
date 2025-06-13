using System.IO;
using UnityEngine;

public class SaveDataToJSON : MonoBehaviour
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "UserData.json");

    public static void SaveUsers(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
    }

    public static PlayerData LoadUsers()
    {
        if (!File.Exists(filePath))
            return new PlayerData();

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
