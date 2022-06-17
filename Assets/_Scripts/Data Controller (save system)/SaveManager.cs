using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string _saveFileName = "/save.sa";
    private static string _savePath = Application.persistentDataPath + _saveFileName;
    private static BinaryFormatter  _formatter = new BinaryFormatter();

    public static void SavePlayer(Player.PlayerStatisticManager p_playerStatisticManager)
    {
        FileStream fileStream = new FileStream(_savePath, FileMode.Create);
        PlayerData playerData = new PlayerData(p_playerStatisticManager);

        _formatter.Serialize(fileStream, playerData);
        fileStream.Close();

        Debug.Log("Save " + _savePath);
    }

    public static PlayerData LoadPlayer()
    {
        if (!FileSaveExist()) 
        {
            Debug.LogWarning("save file not found: " + _savePath );
            return null;
        }
        FileStream fileStream = new FileStream(_savePath, FileMode.Open);

        PlayerData playerData = _formatter.Deserialize(fileStream) as PlayerData;

        fileStream.Close();
        Debug.Log("Load " + _savePath);
        return playerData;
    }

    public static void DeletePlayer()
    {
        if (!FileSaveExist())
        {
            Debug.LogWarning("save file not found: " + _savePath);
        }
        else
        {
            File.Delete(_savePath);
            // refresh editor view
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

    public static bool FileSaveExist()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("save file not found: " + _savePath);
            return false;
        }
        return true;
    }

}
