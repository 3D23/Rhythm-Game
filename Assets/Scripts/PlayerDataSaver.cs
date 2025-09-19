using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerDataSaver : IGameDataSaver
{
    private readonly IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;
    private bool _isSaving = false;

    public PlayerDataSaver(IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
        Application.quitting += OnApplicationQuitting;
    }

    private void OnApplicationQuitting()
    {
        SaveBeforeExit().Forget();
    }

    private async UniTaskVoid SaveBeforeExit()
    {
        if (_isSaving) return;
        _isSaving = true;

        try
        {
            await _dataRepository.Save();
            Debug.Log("Данные успешно сохранены перед выходом");
        }
        catch (Exception)
        {
            Debug.Log("Сохранение отменено");
        }
        finally
        {
            _isSaving = false;
        }
    }

    public async UniTask Save()
    {
        await _dataRepository.Save();
    }

    public void Dispose()
    {
        Application.quitting -= OnApplicationQuitting;
        GC.SuppressFinalize(this);
    }
}