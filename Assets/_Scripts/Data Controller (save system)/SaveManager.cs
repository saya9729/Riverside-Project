using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string _saveFileName = "/save.sa";
    private static string _savePath = Application.persistentDataPath + _saveFileName;
    
    public static void SavePlayer(Player.PlayerStatisticManager p_playerStatisticManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream fileStream = new FileStream(_savePath, FileMode.Create);
        PlayerData playerData = new PlayerData(p_playerStatisticManager);

        formatter.Serialize(fileStream, playerData);
        fileStream.Close();

        Debug.Log("Save " + _savePath);
    }

    public static PlayerData LoadPlayer()
    {
        if (!File.Exists(_savePath)) 
        {
            Debug.LogWarning("save file not found: " + _savePath );
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(_savePath, FileMode.Open);

        PlayerData playerData = formatter.Deserialize(fileStream) as PlayerData;

        fileStream.Close();
        Debug.Log("Load " + _savePath);
        return playerData;
    }

}
