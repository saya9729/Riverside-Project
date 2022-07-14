using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string _saveFileName = "/save";
    private static string _saveFileExtension = ".sa";
    private static BinaryFormatter  _formatter = new BinaryFormatter();

    public static void SavePlayer(Player.PlayerStatisticManager p_playerStatisticManager, float p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileName + p_playerCurrentLevel.ToString() + _saveFileExtension;
        FileStream fileStream = new FileStream(savePath, FileMode.Create);
        PlayerData playerData = new PlayerData(p_playerStatisticManager);

        _formatter.Serialize(fileStream, playerData);
        fileStream.Close();

        Debug.Log("Save " + savePath);
    }

    public static PlayerData LoadPlayer(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileName + p_playerCurrentLevel.ToString() + _saveFileExtension;
        if (!FileSaveExist(p_playerCurrentLevel)) 
        {
            Debug.LogWarning("save file not found: " + savePath);
            return null;
        }
        FileStream fileStream = new FileStream(savePath, FileMode.Open);

        PlayerData playerData = _formatter.Deserialize(fileStream) as PlayerData;

        fileStream.Close();
        Debug.Log("Load " + savePath);
        return playerData;
    }

    public static void DeletePlayer(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileName + p_playerCurrentLevel.ToString() + _saveFileExtension;
        if (!FileSaveExist(p_playerCurrentLevel))
        {
            Debug.LogWarning("save file not found: " + savePath);
        }
        else
        {
            File.Delete(savePath);
            // refresh editor view
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

    public static bool FileSaveExist(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileName + p_playerCurrentLevel.ToString() + _saveFileExtension;
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("save file not found: " + savePath);
            return false;
        }
        return true;
    }

}
