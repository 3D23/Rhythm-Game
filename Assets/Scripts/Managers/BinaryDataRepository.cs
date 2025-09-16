using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class BinaryDataRepository : IGameDataRepository<PlayerData, PlayerData.PlayerDataFields>
{
    private PlayerData _data;
    private const string BINARY_SAVE_FILE = "playerData.sav";

    public BinaryDataRepository() { }

    public Task Load()
    {
        string file = Path.Combine(Application.persistentDataPath, BINARY_SAVE_FILE);
        if (!File.Exists(file))
        {
            _data = new PlayerData();
            return Task.CompletedTask;
        }
        BinaryFormatter binaryFormatter = new();
        byte[] dataBytes = File.ReadAllBytes(file);
        using MemoryStream ms = new(dataBytes);
        try
        {
            _data = binaryFormatter.Deserialize(ms) as PlayerData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��������������� �����: {ex.Message}");
            _data = new PlayerData();
        }
        Debug.Log($"������ ��������� �� {file}");
        return Task.CompletedTask;
    }

    public Task Save()
    {
        string file = Path.Combine(Application.persistentDataPath, BINARY_SAVE_FILE);
        BinaryFormatter binaryFormatter = new();
        using MemoryStream ms = new();
        binaryFormatter.Serialize(ms, _data);
        byte[] dataBytes = ms.ToArray();
        File.WriteAllBytes(file, dataBytes);
        Debug.Log($"������ ��������� � {file}");
        return Task.CompletedTask;
    }

    public PlayerData Get() => _data;

    public void Set(PlayerData.PlayerDataFields key, object value) =>
        _data.Set(key, value);
}