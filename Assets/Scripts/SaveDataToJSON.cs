using System.IO;
using UnityEngine;

public class SaveDataToJSON : MonoBehaviour
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "UserData.json");
    //Application.dataPath, "JSON/UserData.json" //에셋 위치 : 개발중에 사용하기 적합
    //Application.persistentDataPath, "UserData.json"  //영구 저장 위치 : 빌드 할때 사용하기 적합
    //나중에 제출할 때 영구저장위치로 변경 필요

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
