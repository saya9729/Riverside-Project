using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    #region Fields
    private static string _saveFileNamePlayer = "/savePlayer";
    private static string _saveFileNameEnvironment = "/saveEnvironment";
    private static string _saveFileExtension = ".sa";
    private static BinaryFormatter  _formatter = new BinaryFormatter();
    #endregion

    #region Singleton
    private static void Save(string p_savePath, object p_data)
    {
        FileStream fileStream = new FileStream(p_savePath, FileMode.Create);
        _formatter.Serialize(fileStream, p_data);
        fileStream.Close();

        Debug.Log("Save " + p_savePath);
    }
    private static dynamic Load(string p_savePath)
    {
        if (!FileSaveExist(p_savePath))
        {
            Debug.LogWarning("save file not found: " + p_savePath);
            return null;
        }
        FileStream fileStream = new FileStream(p_savePath, FileMode.Open);
        dynamic data = _formatter.Deserialize(fileStream);

        fileStream.Close();
        Debug.Log("Load " + p_savePath);
        return data;
    }
    private static void Delete(string p_savePath)
    {
        if (!FileSaveExist(p_savePath))
        {
            Debug.LogWarning("save file not found: " + p_savePath);
        }
        else
        {
            File.Delete(p_savePath);
            // refresh editor view
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
    private static bool FileSaveExist(string p_savePath)
    {
        if (!File.Exists(p_savePath))
        {
            Debug.LogWarning("save file not found: " + p_savePath);
            return false;
        }
        return true;
    }
    #endregion

    #region Player
    // Player
    public static void SavePlayer(Player.PlayerStatisticManager p_playerStatisticManager, float p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNamePlayer + p_playerCurrentLevel.ToString() + _saveFileExtension;
        PlayerData playerData = new PlayerData(p_playerStatisticManager);
        Save(savePath, playerData);
    }

    public static PlayerData LoadPlayer(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNamePlayer + p_playerCurrentLevel.ToString() + _saveFileExtension;
        return (Load(savePath) as PlayerData);
    }

    public static void DeletePlayer(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNamePlayer + p_playerCurrentLevel.ToString() + _saveFileExtension;
        Delete(savePath);
    }

    public static bool FileSavePlayerExist(int p_playerCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNamePlayer + p_playerCurrentLevel.ToString() + _saveFileExtension;
        return FileSaveExist(savePath);
    }
    #endregion

    #region Evironment
    // Evironment
    public static void SaveEnvironment(Player.KeyInteract[] p_keyInteracts, float p_environmentCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNameEnvironment + p_environmentCurrentLevel.ToString() + _saveFileExtension;
        EnvironmentData environmentData = new EnvironmentData(p_keyInteracts);
        Save(savePath, environmentData);
    }

    public static EnvironmentData LoadEnvironment(int p_environmentCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNameEnvironment + p_environmentCurrentLevel.ToString() + _saveFileExtension;
        return (Load(savePath) as EnvironmentData);
    }

    public static void DeleteEnvironment(int p_environmentCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNameEnvironment + p_environmentCurrentLevel.ToString() + _saveFileExtension;
        Delete(savePath);
    }

    public static bool FileSaveEnvironmentExist(int p_environmentCurrentLevel)
    {
        string savePath = Application.persistentDataPath + _saveFileNameEnvironment + p_environmentCurrentLevel.ToString() + _saveFileExtension;
        return FileSaveExist(savePath);
    }
    #endregion
}
